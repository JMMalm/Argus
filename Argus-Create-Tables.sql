/******************************************************************************
						ARGUS - Create Tables

DESCRIPTION:
This script will create the tables needed for the Argus application.

NOTES:
* This script assumes the Argus database has already been created. Any new
database will do; Argus' database has no special requirements.
* Argus' database was developed using SQL Server v13, but there is nothing unique
about its database or tables that prohibit migrating to other SQL providers.

BY:
* Jordon Malm (jordon.malm@gmail.com), 2019-06-24

******************************************************************************/
USE [Argus]
GO

CREATE TABLE [dbo].[Applications] (
    [Id]               INT          IDENTITY (1, 1) NOT NULL,
    [Name]             VARCHAR (50) NOT NULL,
    [ProductOwnerName] VARCHAR (50) NOT NULL,
    [TeamName]         VARCHAR (50) NOT NULL,
    [Url]              VARCHAR (50) NOT NULL,
    [IsEnabled]        BIT          NOT NULL,
    [DateModified]     DATETIME     NOT NULL,
    PRIMARY KEY ([Id])
);

CREATE TABLE [dbo].[Issues] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [ApplicationId] INT           NOT NULL,
    [Description]   VARCHAR (100) NOT NULL,
    [UserName]      VARCHAR (50)  NOT NULL,
    [DateSubmitted] DATETIME      NOT NULL,
    [DateClosed]    DATETIME      NULL,
    [Priority]      SMALLINT      NOT NULL,
    PRIMARY KEY ([Id])
);