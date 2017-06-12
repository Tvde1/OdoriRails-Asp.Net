DROP PROCEDURE IF EXISTS dbo.PlanServices;

GO
CREATE PROCEDURE dbo.PlanServices 
@amountDays int
AS
BEGIN
	DECLARE @startDate DATE,
			@endDate DATE,
			@previousBigService DATE,
			@previousSmallService DATE,
			@amountDays int = 5,
			@tram int;
	
	--Datums uitrekenen
	SET @startDate = GetDate();
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
		
		if ()
		
		
	END
END



SELECT S.StartDate
FROM [Service] S
FULL JOIN [Repair] R ON S.ServicePk = R.ServiceFk
WHERE S.Startdate >= @startDate AND S.StartDate <= @endDate AND R.Defect = 'Big Planned Maintenance'

SELECT S.StartDate
FROM [Service] S
FULL JOIN [Repair] R ON S.ServicePk = R.ServiceFk
WHERE S.Startdate >= @startDate AND S.StartDate <= @endDate AND R.Defect = 'Small Planned Maintenance'