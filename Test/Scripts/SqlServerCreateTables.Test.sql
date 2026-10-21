-- Ejecutar con sqlcmd desde la raiz del repositorio. :r es sintaxis de sqlcmd.
USE [master];
GO
IF DB_ID(N'PracticaMAD_Grupo02_Test') IS NULL
    CREATE DATABASE [PracticaMAD_Grupo02_Test];
GO
USE [PracticaMAD_Grupo02_Test];
GO
:r Model\SQL\SqlServerCreateTables.sql
