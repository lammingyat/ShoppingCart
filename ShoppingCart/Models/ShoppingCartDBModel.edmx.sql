
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 08/20/2023 17:07:00
-- Generated from EDMX file: C:\Users\user\source\repos\ShoppingCart\ShoppingCart\Models\ShoppingCartDBModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ShoppingCartDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Categories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Categories];
GO
IF OBJECT_ID(N'[dbo].[OrderDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OrderDetails];
GO
IF OBJECT_ID(N'[dbo].[Orders]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Orders];
GO
IF OBJECT_ID(N'[dbo].[Products]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Products];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Categories'
CREATE TABLE [dbo].[Categories] (
    [CategoryId] int IDENTITY(1,1) NOT NULL,
    [CategoryName] nvarchar(max)  NOT NULL,
    [Valid] bit  NOT NULL,
    [CreateUserId] int  NOT NULL,
    [CreateDate] datetime  NOT NULL,
    [LastModifyUserId] int  NOT NULL,
    [LastModifyDate] datetime  NOT NULL
);
GO

-- Creating table 'OrderDetails'
CREATE TABLE [dbo].[OrderDetails] (
    [OrderDetailId] int IDENTITY(1,1) NOT NULL,
    [OrderId] int  NOT NULL,
    [ProductId] nvarchar(max)  NOT NULL,
    [Quantity] int  NOT NULL,
    [UnitPrice] decimal(18,2)  NOT NULL,
    [Total] decimal(18,2)  NOT NULL,
    [Valid] bit  NOT NULL,
    [InvalidDate] datetime  NULL
);
GO

-- Creating table 'Orders'
CREATE TABLE [dbo].[Orders] (
    [OrderId] int IDENTITY(1,1) NOT NULL,
    [OrderDate] datetime  NOT NULL,
    [OrderNumber] nvarchar(max)  NOT NULL,
    [UserId] int  NOT NULL,
    [Total] decimal(18,2)  NOT NULL,
    [Paid] bit  NOT NULL,
    [PaidDate] datetime  NULL,
    [Valid] bit  NOT NULL,
    [InvalidDate] datetime  NULL,
    [Address] nvarchar(max)  NULL,
    [City] nvarchar(max)  NULL,
    [Province] nvarchar(max)  NULL,
    [PostalCode] nvarchar(max)  NULL
);
GO

-- Creating table 'Products'
CREATE TABLE [dbo].[Products] (
    [ProductId] uniqueidentifier  NOT NULL,
    [CategoryId] int  NOT NULL,
    [ProductCode] nvarchar(max)  NOT NULL,
    [ProductName] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [ImagePath] nvarchar(max)  NULL,
    [ProductPrice] decimal(18,2)  NOT NULL,
    [Valid] bit  NOT NULL,
    [CreateUserId] int  NOT NULL,
    [CreateDate] datetime  NOT NULL,
    [LastModifyUserId] int  NOT NULL,
    [LastModifyDate] datetime  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [UserId] int IDENTITY(1,1) NOT NULL,
    [Login] nvarchar(max)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL,
    [Admin] bit  NOT NULL,
    [Valid] bit  NOT NULL,
    [CreateUserId] int  NULL,
    [CreateDate] datetime  NULL,
    [LastModifyUserId] int  NULL,
    [LastModifyDate] datetime  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [CategoryId] in table 'Categories'
ALTER TABLE [dbo].[Categories]
ADD CONSTRAINT [PK_Categories]
    PRIMARY KEY CLUSTERED ([CategoryId] ASC);
GO

-- Creating primary key on [OrderDetailId] in table 'OrderDetails'
ALTER TABLE [dbo].[OrderDetails]
ADD CONSTRAINT [PK_OrderDetails]
    PRIMARY KEY CLUSTERED ([OrderDetailId] ASC);
GO

-- Creating primary key on [OrderId] in table 'Orders'
ALTER TABLE [dbo].[Orders]
ADD CONSTRAINT [PK_Orders]
    PRIMARY KEY CLUSTERED ([OrderId] ASC);
GO

-- Creating primary key on [ProductId] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [PK_Products]
    PRIMARY KEY CLUSTERED ([ProductId] ASC);
GO

-- Creating primary key on [UserId] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([UserId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------