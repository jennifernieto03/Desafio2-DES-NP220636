IF DB_ID('DES104_Desafio02') IS NULL
BEGIN
    CREATE DATABASE DES104_Desafio02;
END
GO

USE DES104_Desafio02;
GO

IF OBJECT_ID('dbo.Clientes', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Clientes
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(80) NOT NULL,
        Apellido NVARCHAR(80) NOT NULL,
        Email NVARCHAR(150) NOT NULL UNIQUE,
        Telefono NVARCHAR(20) NOT NULL,
        Direccion NVARCHAR(200) NOT NULL,
        FechaRegistro DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
        Activo BIT NOT NULL DEFAULT 1
    );
END
GO

IF OBJECT_ID('dbo.Pedidos', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Pedidos
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        ClienteId INT NOT NULL,
        NumeroPedido NVARCHAR(30) NOT NULL UNIQUE,
        FechaPedido DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
        Estado NVARCHAR(30) NOT NULL,
        Total DECIMAL(10,2) NOT NULL,
        Descripcion NVARCHAR(250) NULL,

        CONSTRAINT CK_Pedidos_Total
            CHECK (Total > 0)
    );
END
GO