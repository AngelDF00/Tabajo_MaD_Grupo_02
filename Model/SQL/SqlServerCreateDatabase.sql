-- Ejecutar en master. No borra una base existente.
USE [master];
GO
IF DB_ID(N'MAD') IS NULL
BEGIN
    CREATE DATABASE [MAD];
    PRINT N'Base MAD creada.';
END
ELSE
    PRINT N'La base MAD ya existe.';
GO
