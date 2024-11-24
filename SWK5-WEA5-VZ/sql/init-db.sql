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
        CREATE TABLE [dbo].[Holidays] (
            [Id] INT IDENTITY(1,1) NOT NULL,
            [Name] NVARCHAR(50) NOT NULL,
            [Date] DATE NOT NULL,
            CONSTRAINT [PK_Holidays] PRIMARY KEY CLUSTERED ([Id])
        );
        
        -- Create Routes table
        CREATE TABLE [dbo].[Routes] (
            [Id] INT IDENTITY(1,1) NOT NULL,
            [RouteNumber] NVARCHAR(10) NOT NULL,
            [ValidFrom] DATE NOT NULL,
            [ValidTo] DATE NOT NULL,
            CONSTRAINT [PK_Routes] PRIMARY KEY CLUSTERED ([Id])
        );
        
        -- Create Stops table
        CREATE TABLE [dbo].[Stops] (
            [Id] INT IDENTITY(1,1) NOT NULL,
            [RouteId] INT NOT NULL,
            [StopName] NVARCHAR(100) NOT NULL,
            [ArrivalTime] TIME NOT NULL,
            CONSTRAINT [PK_Stops] PRIMARY KEY CLUSTERED ([Id]),
            CONSTRAINT [FK_Stops_Routes] FOREIGN KEY ([RouteId]) REFERENCES [dbo].[Routes]([Id])
        );
    ');
    PRINT 'Tables Holidays, Routes, and Stops created';
    
    EXEC('
        USE [transport_db];
        
        -- Populate Holidays table
        INSERT INTO [dbo].[Holidays] ([Name], [Date]) VALUES
        (N''New Year'', ''2024-01-01''),
        (N''Christmas'', ''2024-12-25''),
        (N''Easter'', ''2024-04-01'');
        
        -- Populate Routes table
        INSERT INTO [dbo].[Routes] ([RouteNumber], [ValidFrom], [ValidTo]) VALUES
        (N''101'', ''2024-01-01'', ''2024-12-31''),
        (N''202'', ''2024-01-01'', ''2024-12-31''),
        (N''303'', ''2024-01-01'', ''2024-12-31'');
        
        -- Populate Stops table
        INSERT INTO [dbo].[Stops] ([RouteId], [StopName], [ArrivalTime]) VALUES
        (1, N''Main Station'', ''08:00:00''),
        (1, N''Downtown'', ''08:15:00''),
        (1, N''Airport'', ''08:30:00''),
        (2, N''Central Park'', ''09:00:00''),
        (2, N''City Center'', ''09:15:00''),
        (3, N''University'', ''10:00:00'');
    ');
    PRINT 'Tables filled with sample data';
END
