-- Check if the database 'transport_db' exists
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'transport_db')
BEGIN
    PRINT 'Database transport_db already exists';
END
ELSE
BEGIN
    EXEC('CREATE DATABASE [transport_db]');
    PRINT 'Database transport_db created';
    
    EXEC('
        USE [transport_db];
        
        -- Create Holidays table
        CREATE TABLE Holidays (
            HolidayId INT IDENTITY(1,1) PRIMARY KEY,
            StartDate DATE NOT NULL,
            EndDate DATE NOT NULL,
            Description NVARCHAR(100) NOT NULL,
            IsSchoolBreak BIT NOT NULL,
            CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
            ModifiedAt DATETIME2 NOT NULL DEFAULT GETDATE()
        );

        -- Create TransportCompanies table
        CREATE TABLE TransportCompanies (
            CompanyId INT IDENTITY(1,1) PRIMARY KEY,
            Name NVARCHAR(100) NOT NULL,
            CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
            ModifiedAt DATETIME2 NOT NULL DEFAULT GETDATE()
        );

        -- Create Stops table
        CREATE TABLE Stops (
            StopId INT IDENTITY(1,1) PRIMARY KEY,
            Name NVARCHAR(100) NOT NULL,
            ShortCode NVARCHAR(10) NOT NULL UNIQUE,
            Latitude DECIMAL(9,6) NOT NULL,
            Longitude DECIMAL(9,6) NOT NULL,
            CompanyId INT NOT NULL FOREIGN KEY REFERENCES TransportCompanies(CompanyId),
            CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
            ModifiedAt DATETIME2 NOT NULL DEFAULT GETDATE()
        );

        -- Create Routes table
        CREATE TABLE Routes (
            RouteId INT IDENTITY(1,1) PRIMARY KEY,
            RouteNumber NVARCHAR(20) NOT NULL,
            CompanyId INT NOT NULL FOREIGN KEY REFERENCES TransportCompanies(CompanyId),
            ValidFromDate DATE NOT NULL,
            ValidUntilDate DATE NOT NULL,
            OperatesOnWeekdays BIT NOT NULL DEFAULT 1,
            OperatesOnWeekends BIT NOT NULL DEFAULT 0,
            OperatesOnHolidays BIT NOT NULL DEFAULT 0,
            OperatesDuringSchoolBreaks BIT NOT NULL DEFAULT 1,
            CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
            ModifiedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
            CONSTRAINT UQ_Route_Number_Date UNIQUE (RouteNumber, ValidFromDate, ValidUntilDate)
        );

        -- Create RouteStops table
        CREATE TABLE RouteStops (
            RouteStopId INT IDENTITY(1,1) PRIMARY KEY,
            RouteId INT NOT NULL FOREIGN KEY REFERENCES Routes(RouteId),
            StopId INT NOT NULL FOREIGN KEY REFERENCES Stops(StopId),
            ScheduledTime TIME NOT NULL,
            StopSequence INT NOT NULL,
            CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
            ModifiedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
            CONSTRAINT UQ_Route_Stop_Sequence UNIQUE (RouteId, StopSequence)
        );
    ');
    PRINT 'Tables created successfully';

    EXEC('
        USE [transport_db];
        
        -- Populate TransportCompanies table
        INSERT INTO TransportCompanies (Name) VALUES
        (N''FH Transit'');

        -- Populate Stops table
        INSERT INTO Stops (Name, ShortCode, Latitude, Longitude, CompanyId) VALUES
        (N''Main Station'', N''MSTN'', 48.2082, 16.3738, 1),
        (N''Downtown'', N''DTWN'', 48.2100, 16.3770, 1),
        (N''Airport'', N''AIRP'', 48.1200, 16.5600, 1);

        -- Populate Routes table
        INSERT INTO Routes (RouteNumber, CompanyId, ValidFromDate, ValidUntilDate, OperatesOnWeekdays, OperatesOnWeekends, OperatesOnHolidays, OperatesDuringSchoolBreaks) VALUES
        (N''101'', 1, ''2024-01-01'', ''2024-12-31'', 1, 1, 0, 1),
        (N''202'', 1, ''2024-01-01'', ''2024-12-31'', 1, 0, 1, 0);

        -- Populate Holidays table
        INSERT INTO Holidays (StartDate, EndDate, Description, IsSchoolBreak) VALUES
        (''2024-01-01'', ''2024-01-01'', N''New Year'', 0),
        (''2024-12-25'', ''2024-12-25'', N''Christmas'', 0),
        (''2024-04-01'', ''2024-04-01'', N''Easter'', 0);

        -- Populate RouteStops table
        INSERT INTO RouteStops (RouteId, StopId, ScheduledTime, StopSequence) VALUES
        (1, 2, ''08:15:00'', 2),
        (1, 3, ''08:30:00'', 3),
        (2, 1, ''09:00:00'', 1),
        (2, 3, ''09:30:00'', 2);
    ');
    PRINT 'Tables populated with sample data';
END
