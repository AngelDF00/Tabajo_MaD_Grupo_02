/* 
 * SQL Server Script
 * 
 * In a local environment (for example, with the SQLServerExpress instance 
 * included in the VStudio installation) it will be necessary to create the 
 * database and the user required by the connection string. So, the following
 * steps are needed:
 *
 *     Configure the @Default_DB_Path variable with the path where 
 *     database and log files will be created  
 *
 * This script can be executed from MS Sql Server Management Studio Express,
 * but also it is possible to use a command Line syntax:
 *
 *    > sqlcmd.exe -U [user] -P [password] -I -i SqlServerCreateTables.sql
 *
 */


 /******************************************************************************/
 /*** PATH to store the db files. This path must exists in the local system. ***/
 /******************************************************************************/
 DECLARE @Default_DB_Path as VARCHAR(64)  
 SET @Default_DB_Path = N'C:\SourceCode.SVN\DataBase\'
 
USE [master]


/***** Drop database if already exists  ******/
IF  EXISTS (SELECT name FROM sys.databases WHERE name = 'PracticaMAD_test')
DROP DATABASE [PracticaMAD_test]


USE [master]


/* DataBase Creation */

	                              
DECLARE @sql nvarchar(500)

SET @sql = 
  N'CREATE DATABASE [PracticaMAD_test] 
    ON PRIMARY ( NAME = PracticaMAD_test, FILENAME = "' + @Default_DB_Path + N'PracticaMAD_test.mdf")
    LOG ON ( NAME = PracticaMAD_test_log, FILENAME = "' + @Default_DB_Path + N'PracticaMAD_test_log.ldf")'

EXEC(@sql)
PRINT N'Database [PracticaMAD_test] created.'
GO


USE [PracticaMAD_test]


/* ********** Drop Table UserProfile if already exists *********** */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('[UserProfile]') AND type in ('U'))
DROP TABLE [UserProfile]
GO

/*
 * Create tables.
 * UserProfile table is created. Indexes required for the 
 * most common operations are also defined.
 */

/*  UserProfile */

CREATE TABLE UserProfile (
	usrId bigint IDENTITY(1,1) NOT NULL,
	loginName varchar(30) NOT NULL,
	enPassword varchar(50) NOT NULL,
	firstName varchar(30) NOT NULL,
	lastName varchar(40) NOT NULL,
	email varchar(60) NOT NULL,
	language varchar(2) NOT NULL,
	country varchar(2) NOT NULL,

	CONSTRAINT [PK_UserProfile] PRIMARY KEY (usrId),
	CONSTRAINT [UniqueKey_Login] UNIQUE (loginName)
)


CREATE NONCLUSTERED INDEX [IX_UserProfileIndexByLoginName]
ON [UserProfile] ([loginName] ASC)

PRINT N'Table UserProfile created.'
GO

GO

