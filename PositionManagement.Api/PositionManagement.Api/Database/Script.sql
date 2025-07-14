-- CREATE DATABASE PositionManagement;
use PositionManagement;
go

drop table if exists [dbo].[Positions];
CREATE TABLE [dbo].[Positions] (
	[TansactionId] INT IDENTITY(1,1) NOT NULL,
	[TradeId] INT NOT NULL,
	[Version] INT NOT NULL,
	[SecurityCode] NVARCHAR(20) NOT NULL,
	[Quantity] INT NOT NULL,
	[Call] NVARCHAR(4) NOT NULL,
	CONSTRAINT [PK_Positions] PRIMARY KEY CLUSTERED ([TansactionId])
);

drop table if exists [dbo].[TempPositions];
CREATE TABLE [dbo].[TempPositions] (
	[TansactionId] INT IDENTITY(1,1) NOT NULL,
	[TradeId] INT NOT NULL,
	[Version] INT NOT NULL,
	[SecurityCode] NVARCHAR(20) NOT NULL,
	[Quantity] INT NOT NULL,
	[Call] NVARCHAR(4) NOT NULL,
	CONSTRAINT [PK_TempPositions] PRIMARY KEY CLUSTERED ([TansactionId])
);

