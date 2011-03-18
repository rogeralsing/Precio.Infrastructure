
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 02/03/2011 18:54:31
-- Generated from EDMX file: c:\users\roggan\documents\visual studio 2010\Projects\CodeBaseBlog\CodeBaseBlog.DomainModel\BlogModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [MyBlog];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_PostCategoryLinks_Posts]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PostCategoryLinks] DROP CONSTRAINT [FK_PostCategoryLinks_Posts];
GO
IF OBJECT_ID(N'[dbo].[FK_PostCategoryLinks_Categories]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PostCategoryLinks] DROP CONSTRAINT [FK_PostCategoryLinks_Categories];
GO
IF OBJECT_ID(N'[dbo].[FK_PostComment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Comments] DROP CONSTRAINT [FK_PostComment];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Categories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Categories];
GO
IF OBJECT_ID(N'[dbo].[Comments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Comments];
GO
IF OBJECT_ID(N'[dbo].[Posts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Posts];
GO
IF OBJECT_ID(N'[dbo].[PostCategoryLinks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PostCategoryLinks];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Categories'
CREATE TABLE [dbo].[Categories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(500)  NOT NULL
);
GO

-- Creating table 'Comments'
CREATE TABLE [dbo].[Comments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Body] nvarchar(max)  NOT NULL,
    [Approved] bit  NOT NULL,
    [CreationDate] datetime  NOT NULL,
    [User_Name] nvarchar(500)  NOT NULL,
    [User_Email] nvarchar(500)  NULL,
    [User_WebSite] nvarchar(500)  NULL,
    [User_UserId] nvarchar(max)  NOT NULL,
    [Post_Id] int  NOT NULL
);
GO

-- Creating table 'Posts'
CREATE TABLE [dbo].[Posts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CreationDate] datetime  NOT NULL,
    [LastModifiedDate] datetime  NOT NULL,
    [PublishDate] datetime  NULL,
    [Subject] nvarchar(500)  NOT NULL,
    [Body] nvarchar(max)  NOT NULL,
    [CommentsEnabled] bit  NOT NULL
);
GO

-- Creating table 'PostCategoryLinks'
CREATE TABLE [dbo].[PostCategoryLinks] (
    [Posts_Id] int  NOT NULL,
    [Categories_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Categories'
ALTER TABLE [dbo].[Categories]
ADD CONSTRAINT [PK_Categories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Comments'
ALTER TABLE [dbo].[Comments]
ADD CONSTRAINT [PK_Comments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Posts'
ALTER TABLE [dbo].[Posts]
ADD CONSTRAINT [PK_Posts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Posts_Id], [Categories_Id] in table 'PostCategoryLinks'
ALTER TABLE [dbo].[PostCategoryLinks]
ADD CONSTRAINT [PK_PostCategoryLinks]
    PRIMARY KEY NONCLUSTERED ([Posts_Id], [Categories_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Posts_Id] in table 'PostCategoryLinks'
ALTER TABLE [dbo].[PostCategoryLinks]
ADD CONSTRAINT [FK_PostCategoryLinks_Posts]
    FOREIGN KEY ([Posts_Id])
    REFERENCES [dbo].[Posts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Categories_Id] in table 'PostCategoryLinks'
ALTER TABLE [dbo].[PostCategoryLinks]
ADD CONSTRAINT [FK_PostCategoryLinks_Categories]
    FOREIGN KEY ([Categories_Id])
    REFERENCES [dbo].[Categories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PostCategoryLinks_Categories'
CREATE INDEX [IX_FK_PostCategoryLinks_Categories]
ON [dbo].[PostCategoryLinks]
    ([Categories_Id]);
GO

-- Creating foreign key on [Post_Id] in table 'Comments'
ALTER TABLE [dbo].[Comments]
ADD CONSTRAINT [FK_PostComment]
    FOREIGN KEY ([Post_Id])
    REFERENCES [dbo].[Posts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PostComment'
CREATE INDEX [IX_FK_PostComment]
ON [dbo].[Comments]
    ([Post_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------