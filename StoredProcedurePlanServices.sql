DROP PROCEDURE dbo.PlanServices;
DROP PROCEDURE dbo.PlanBigMaintenance;
DROP PROCEDURE dbo.PlanSmallMaintenance;
DROP PROCEDURE dbo.AddCleaningService;
DROP PROCEDURE dbo.AddRepairService;

GO
CREATE PROCEDURE dbo.AddRepairService 
@startDate DATE,
@tram int,
@defect NVARCHAR (1000),
@type int
AS
BEGIN
	DECLARE @service int;
	--Create service
	INSERT INTO Service (StartDate, TramFk) VALUES (@startDate, @tram);
	--Get serviceId
	SELECT @service = ServicePk FROM [Service] WHERE StartDate = @startDate AND TramFk = @tram;
	--Create Repair
	INSERT INTO Repair (ServiceFk, Defect, Type) VALUES (@service, @defect, @type);
END

GO
CREATE PROCEDURE dbo.AddCleaningService 
@startDate DATE,
@tram int,
@size int,
@remarks NVARCHAR (1000)
AS
BEGIN
	DECLARE @service int;
	--Create service
	INSERT INTO [Service] (StartDate, TramFk) VALUES (@startDate, @tram);
	--Get serviceId
	SELECT @service = ServicePk FROM [Service] WHERE StartDate = @startDate AND TramFk = @tram;
	--Create Repair
	INSERT INTO Clean (ServiceFk, Size, Remarks) VALUES (@service, @size, @remarks);
END

GO
CREATE PROCEDURE dbo.PlanBigMaintenance 
@amountDays int
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @startDate DATE,
			@endDate DATE,
			@currentDate DATE,
			@previousBigService DATE,
			@previousSmallService DATE,
			@tram int,
			@serviceCount int,
			@totalServiceCount int = 0;
	
	--Datums uitrekenen
	SET @startDate = GetDate();
	SET @currentDate = @startDate
	SET @endDate = DATEADD(d, @amountDays, @startdate);
	SET @previousBigService = DATEADD(m, -6, @startdate);
	
	--Check of alle mogelijke posities zijn ingedeeld
	SELECT @totalServiceCount = COUNT(S.StartDate)
	FROM [Service] S
	INNER JOIN [Repair] R ON S.ServicePk = R.ServiceFk
	WHERE S.StartDate <= @endDate AND S.StartDate > @startDate AND R.Defect = 'Big Planned Maintenance';

	--Alle trams ophalen die of een big maintenace meer als een half jaar geleden of er nog geen gehad hebben
	DECLARE @BigMaintenance AS CURSOR;
	SET @BigMaintenance = CURSOR FOR 
		SELECT TramPk
		FROM Tram T
		FULL JOIN [Service] S ON T.TramPK = S.TramFk
		FULL JOIN [Repair] R ON S.ServicePk = R.ServiceFk
		WHERE (S.EndDate < @previousBigService AND R.Defect = 'Big Planned Maintenance') OR S.Startdate IS NULL OR R.Defect = 'Small Planned Maintenance'
		ORDER BY TramPk;
		
	OPEN @BigMaintenance;

	FETCH NEXT FROM @BigMaintenance INTO @tram;

	WHILE (@@FETCH_STATUS = 0 AND @totalServiceCount < @amountDays)
	BEGIN
		WHILE (@currentDate <= @endDate AND @totalServiceCount < @amountDays)
		BEGIN
			--Check of er nog een nog een plaats vrij is die dag
			SELECT @serviceCount = COUNT(S.StartDate)
			FROM [Service] S
			INNER JOIN [Repair] R ON S.ServicePk = R.ServiceFk
			WHERE S.StartDate = @currentDate AND R.Defect = 'Big Planned Maintenance';

			if (@serviceCount < 1)--Kan 1 per dag
			BEGIN
				EXEC AddRepairService @currentDate, @tram, 'Big Planned Maintenance', 1;
				BREAK;
			END
			SET @currentDate = DATEADD(d, 1, @currentDate);

			--Check of alle mogelijke posities zijn ingedeeld
			SELECT @totalServiceCount = COUNT(S.StartDate)
			FROM [Service] S
			INNER JOIN [Repair] R ON S.ServicePk = R.ServiceFk
			WHERE S.StartDate <= @endDate AND S.StartDate > @startDate AND R.Defect = 'Big Planned Maintenance';
		END
		SET @currentDate = @startDate;
		FETCH NEXT FROM @BigMaintenance INTO @tram;
	END
END

GO
CREATE PROCEDURE dbo.PlanSmallMaintenance 
@amountDays int
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @startDate DATE,
			@endDate DATE,
			@currentDate DATE,
			@previousSmallService DATE,
			@tram int,
			@serviceCount int,
			@totalServiceCount int = 0;
	
	--Datums uitrekenen
	SET @startDate = GetDate();
	SET @currentDate = @startDate
	SET @endDate = DATEADD(d, @amountDays, @startdate);
	SET @previousSmallService = DATEADD(m, -3, @startdate);
	
	--Check of alle mogelijke posities zijn ingedeeld
	SELECT @totalServiceCount = COUNT(S.StartDate)
	FROM [Service] S
	INNER JOIN [Clean] C ON S.ServicePk = C.ServiceFk
	WHERE S.StartDate <= @endDate AND S.StartDate > @startDate AND C.Remarks = 'Small Planned Maintenance';

	--Alle trams ophalen die of een small maintenace meer als een kwart jaar geleden of er nog geen gehad hebben
	DECLARE @SmallMaintenance AS CURSOR;
	SET @SmallMaintenance = CURSOR FOR 
		SELECT TramPk
		FROM Tram T
		FULL JOIN [Service] S ON T.TramPK = S.TramFk
		FULL JOIN [Clean] C ON S.ServicePk = C.ServiceFk
		WHERE (S.EndDate < @previousSmallService AND C.Remarks = 'Small Planned Maintenance') OR S.Startdate IS NULL OR C.Remarks = 'Big Planned Maintenance'
		ORDER BY TramPk;
		
	OPEN @SmallMaintenance;

	FETCH NEXT FROM @SmallMaintenance INTO @tram;

	WHILE (@@FETCH_STATUS = 0 AND @totalServiceCount < (@amountDays * 3))
	BEGIN
		WHILE (@currentDate <= @endDate AND @totalServiceCount < (@amountDays * 3))
		BEGIN
			--Check of er nog een nog een plaats vrij is die dag
			SELECT @serviceCount = COUNT(S.StartDate)
			FROM [Service] S
			INNER JOIN [Clean] C ON S.ServicePk = C.ServiceFk
			WHERE S.StartDate = @currentDate AND C.Remarks = 'Small Planned Maintenance';

			if (@serviceCount < 3)--Kan 3 per dag
			BEGIN
				EXEC AddCleaningService @currentDate, @tram, 1, 'Small Planned Maintenance';
				BREAK;
			END
			SET @currentDate = DATEADD(d, 1, @currentDate);

			--Check of alle mogelijke posities zijn ingedeeld
			SELECT @totalServiceCount = COUNT(S.StartDate)
			FROM [Service] S
			INNER JOIN [Clean] C ON S.ServicePk = C.ServiceFk
			WHERE S.StartDate <= @endDate AND S.StartDate > @startDate AND C.Remarks = 'Small Planned Maintenance';
		END
		SET @currentDate = @startDate;
		FETCH NEXT FROM @SmallMaintenance INTO @tram;
	END
END

GO
CREATE PROCEDURE dbo.PlanServices 
@forAmountDays int
AS
BEGIN
	--Plan Big Maintenaces
	EXEC PlanBigMaintenance @amountDays = @forAmountDays;
	--Plan Small Maintenaces
	EXEC PlanSmallMaintenance @amountDays = @forAmountDays;
END