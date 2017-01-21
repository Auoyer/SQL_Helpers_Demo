
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 11/16/2016 10:36:45
-- Generated from EDMX file: D:\Documents\Visual Studio 2013\Projects\T_ORM\EF_MSSQL_ModelFirst\People_MF_DataModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [People_EF_ModelFirst];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'TableASet'
CREATE TABLE [dbo].[TableASet] (
    [A_Id] int IDENTITY(1,1) NOT NULL,
    [A_Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'TableBSet'
CREATE TABLE [dbo].[TableBSet] (
    [B_Id] int IDENTITY(1,1) NOT NULL,
    [B_Like] nvarchar(max)  NOT NULL,
    [A_Id] nvarchar(max)  NOT NULL,
    [TableAA_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [A_Id] in table 'TableASet'
ALTER TABLE [dbo].[TableASet]
ADD CONSTRAINT [PK_TableASet]
    PRIMARY KEY CLUSTERED ([A_Id] ASC);
GO

-- Creating primary key on [B_Id] in table 'TableBSet'
ALTER TABLE [dbo].[TableBSet]
ADD CONSTRAINT [PK_TableBSet]
    PRIMARY KEY CLUSTERED ([B_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [TableAA_Id] in table 'TableBSet'
ALTER TABLE [dbo].[TableBSet]
ADD CONSTRAINT [FK_TableATableB]
    FOREIGN KEY ([TableAA_Id])
    REFERENCES [dbo].[TableASet]
        ([A_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TableATableB'
CREATE INDEX [IX_FK_TableATableB]
ON [dbo].[TableBSet]
    ([TableAA_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------