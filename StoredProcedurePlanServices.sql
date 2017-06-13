DROP PROCEDURE dbo.PlanServices;
DROP PROCEDURE dbo.AddRepairService;

GO
CREATE PROCEDURE dbo.AddRepairService 
@startDate DATE,
@tram int,
@defect NVARCHAR (1000),
@type NVARCHAR (1000)
AS
BEGIN
	DECLARE @service int;
	--Create service
	INSERT INTO Service (StartDate, TramFk) VALUES (@startDate, @tram);
	--Get serviceId
	SELECT @service FROM Service WHERE StartDate = @startDate AND TramFk = @tram;
	--Create Repair
	INSERT INTO Repair (ServiceFk, Defect, Type) VALUES (@service, @defect, @type);
END


GO
CREATE PROCEDURE dbo.PlanServices 
@amountDays int
AS
BEGIN
	DECLARE @startDate DATE,
			@endDate DATE,
			@currentDate DATE,
			@previousBigService DATE,
			@previousSmallService DATE,
			@tram int,
			@serviceCount int;
	
	--Datums uitrekenen
	SET @startDate = GetDate();
	SET @currentDate = @startDate
	SET @endDate = DATEADD(d, @amountDays, @startdate);
	SET @previousBigService = DATEADD(m, -6, @startdate);
	SET @previousSmallService = DATEADD(m, -3, @startdate);
	
	--Alle trams ophalen die of een big maintenace meer als een half jaar geleden of er nog geen gehad hebben
	DECLARE @BigMaintenance AS CURSOR;
	SET @BigMaintenance = CURSOR FOR 
		SELECT TramPk
		FROM Tram T
		FULL JOIN [Service] S ON T.TramPK = S.TramFk
		FULL JOIN [Repair] R ON S.ServicePk = R.ServiceFk
		WHERE (S.EndDate < @previousBigService AND R.Defect = 'Big Planned Maintenance') OR S.Startdate IS NULL
		ORDER BY TramPk;
		
	OPEN @BigMaintenance;
		
	WHILE (@@FETCH_STATUS = 0)
	BEGIN
		FETCH NEXT FROM @BigMaintenance INTO @tram;
		
		WHILE (@currentDate <= @endDate)
		BEGIN
			--Check of er nog een nog een plaats vrij is die dag
			SELECT @serviceCount = COUNT(S.StartDate)
			FROM [Service] S
			FULL JOIN [Repair] R ON S.ServicePk = R.ServiceFk
			WHERE S.StartDate = @currentDate AND R.Defect = 'Big Planned Maintenance';
			
			if (@serviceCount < 1)--Kan 1 per dag
			BEGIN
				EXEC AddRepairService @currentDate, @tram, 'Big Planned Maintenance', 'Maintenance'
				BREAK;
			END
			SET @currentDate = DATEADD(d, 1, @currentDate);
		END
	END

	CLOSE @BigMaintenance;
	
	--Alle trams ophalen die of een small maintenace meer als een kwart jaar geleden of er nog geen gehad hebben
	DECLARE @SmallMaintenace AS CURSOR;

	SET @SmallMaintenace = CURSOR FOR 
		SELECT TramPk
		FROM Tram T
		FULL JOIN [Service] S ON T.TramPK = S.TramFk
		FULL JOIN [Repair] R ON S.ServicePk = R.ServiceFk
		WHERE (S.EndDate < @previousSmallService AND R.Defect = 'Small Planned Maintenance') OR S.Startdate IS NULL
		ORDER BY TramPk;
		
	OPEN @SmallMaintenace;
		
	WHILE (@@FETCH_STATUS = 0)
	BEGIN
		FETCH NEXT FROM @SmallMaintenace INTO @tram;
		
		WHILE (@currentDate <= @endDate)
		BEGIN
			--Check of er nog een nog een plaats vrij is die dag
			SELECT @serviceCount = COUNT(S.StartDate)
			FROM [Service] S
			FULL JOIN [Repair] R ON S.ServicePk = R.ServiceFk
			WHERE S.StartDate = @currentDate AND R.Defect = 'Big Planned Maintenance';
			
			if (@serviceCount < 3)--Kan 3 per dag
			BEGIN
				EXEC AddRepairService @currentDate, @tram, 'Small Planned Maintenance', 'Maintenance'
				BREAK;
			END
			SET @currentDate = DATEADD(d, 1, @currentDate);
		END
	END
END