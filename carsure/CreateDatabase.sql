USE [master]
GO

-- Drop the database if it exists
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'CarInsurance')
BEGIN
    ALTER DATABASE [CarInsurance] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [CarInsurance];
END
GO

-- Create the database
CREATE DATABASE [CarInsurance] 
ON PRIMARY 
(
    NAME = N'CarInsurance', 
    FILENAME = N'C:\Users\Tutankamon\Desktop\muhammed-yasar\ProjectCar\ASP.NET-MVC-EntityFramework-Assignment\Car-net\Car-net\App_Data\CarInsurance.mdf',
    SIZE = 8MB,
    FILEGROWTH = 10%
)
LOG ON 
(
    NAME = N'CarInsurance_log', 
    FILENAME = N'C:\Users\Tutankamon\Desktop\muhammed-yasar\ProjectCar\ASP.NET-MVC-EntityFramework-Assignment\Car-net\Car-net\App_Data\CarInsurance_log.ldf',
    SIZE = 8MB,
    FILEGROWTH = 10%
);
GO

-- Create the Insurees table
USE [CarInsurance]
GO

CREATE TABLE [dbo].[Insurees](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [FirstName] [nvarchar](50) NOT NULL,
    [LastName] [nvarchar](50) NOT NULL,
    [EmailAddress] [nvarchar](100) NOT NULL,
    [DateOfBirth] [datetime2](7) NOT NULL,
    [CarYear] [int] NOT NULL,
    [CarMake] [nvarchar](50) NOT NULL,
    [CarModel] [nvarchar](50) NOT NULL,
    [SpeedingTickets] [int] NOT NULL,
    [HasDUI] [bit] NOT NULL,
    [IsFullCoverage] [bit] NOT NULL,
    [Quote] [decimal](18, 2) NOT NULL,
    CONSTRAINT [PK_Insurees] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO 