-- Use the [master] database to create a new database called [PMDB].
USE [master]
GO

-- Create the [PMDB] database.
CREATE DATABASE [PMDB]
GO

-- Switch to the newly created [PMDB] database.
USE [PMDB]
GO

-- Create a schema called [PM] within the [PMDB] database.
CREATE SCHEMA [PM]
GO

-- Create a table called [Companies] within the [PM] schema.
CREATE TABLE [PM].[Companies](
    [CRNNO] [int] NOT NULL,
    [CompanyName] [varchar](50) NOT NULL,
    PRIMARY KEY ([CRNNO])
)
GO

-- Create a table called [Managers] within the [PM] schema.
CREATE TABLE [PM].[Managers](
    [Id] [int] NOT NULL,
    [Email] [varchar](100) NOT NULL,
    PRIMARY KEY ([Id])
)
GO
 
-- Create a table called [Projects] within the [PM] schema.
CREATE TABLE [PM].[Projects](
    [PRJNO] [int] NOT NULL,
    [Title] [varchar](100) NOT NULL,
    [ManagerId] [int] NOT NULL,
    [StartDate] [datetime2](7) NOT NULL,
    [InitialCost] [decimal](18, 2) NOT NULL,
    [Parked] [bit] NOT NULL,
    [CRNNO] [int] NOT NULL,
    FOREIGN KEY([CRNNO]) REFERENCES [PM].[Companies] ([CRNNO]),
    FOREIGN KEY([ManagerId]) REFERENCES [PM].[Managers] ([Id]),
    PRIMARY KEY ([PRJNO])
)
GO

-- Create a table called [Technologies] within the [PM] schema.
CREATE TABLE [PM].[Technologies](
    [Id] [int] NOT NULL,
    [Name] [varchar](100) NOT NULL,
    PRIMARY KEY ([Id])
)
GO

-- Create a table called [ProjectTechnologies] within the [PM] schema.
CREATE TABLE [PM].[ProjectTechnologies](
    [PRJNO] [int] NOT NULL,
    [TechnologyId] [int] NOT NULL,
    FOREIGN KEY([PRJNO]) REFERENCES [PM].[Projects] ([PRJNO]),
    FOREIGN KEY([TechnologyId]) REFERENCES [PM].[Technologies] ([Id]),
    PRIMARY KEY ([PRJNO], [TechnologyId])
)
GO
