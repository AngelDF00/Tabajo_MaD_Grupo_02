-- Esquema comun. Ejecutar dentro de MAD o PracticaMAD_Grupo02_Test.
-- No contiene USE ni DROP para poder reutilizarlo sin perder datos.

IF OBJECT_ID(N'dbo.Usuario', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Usuario (
        UsuarioId BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Usuario PRIMARY KEY,
        Login NVARCHAR(30) NOT NULL,
        HashContrasena NVARCHAR(200) NOT NULL,
        Nombre NVARCHAR(50) NOT NULL,
        Apellidos NVARCHAR(100) NOT NULL,
        Correo NVARCHAR(254) NOT NULL,
        Idioma NVARCHAR(10) NULL,
        Pais NVARCHAR(100) NULL,
        CONSTRAINT UQ_Usuario_Login UNIQUE (Login),
        CONSTRAINT CK_Usuario_LoginNoVacio CHECK (LEN(LTRIM(RTRIM(Login))) > 0),
        CONSTRAINT CK_Usuario_CorreoNoVacio CHECK (LEN(LTRIM(RTRIM(Correo))) > 0)
    );
END;
GO

IF OBJECT_ID(N'dbo.Categoria', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Categoria (
        CategoriaId BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Categoria PRIMARY KEY,
        Nombre NVARCHAR(100) NOT NULL,
        CONSTRAINT CK_Categoria_NombreNoVacio CHECK (LEN(LTRIM(RTRIM(Nombre))) > 0)
    );
END;
GO

IF OBJECT_ID(N'dbo.Producto', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Producto (
        ProductoId BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Producto PRIMARY KEY,
        Nombre NVARCHAR(200) NOT NULL,
        FechaAlta DATETIME2(3) NOT NULL CONSTRAINT DF_Producto_FechaAlta DEFAULT SYSUTCDATETIME(),
        VendedorId BIGINT NOT NULL,
        CategoriaId BIGINT NOT NULL,
        CONSTRAINT FK_Producto_Vendedor FOREIGN KEY (VendedorId) REFERENCES dbo.Usuario(UsuarioId),
        CONSTRAINT FK_Producto_Categoria FOREIGN KEY (CategoriaId) REFERENCES dbo.Categoria(CategoriaId),
        CONSTRAINT CK_Producto_NombreNoVacio CHECK (LEN(LTRIM(RTRIM(Nombre))) > 0)
    );
END;
GO

IF OBJECT_ID(N'dbo.Comentario', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Comentario (
        ComentarioId BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Comentario PRIMARY KEY,
        ProductoId BIGINT NOT NULL,
        AutorId BIGINT NOT NULL,
        Texto NVARCHAR(MAX) NOT NULL,
        FechaCreacion DATETIME2(3) NOT NULL CONSTRAINT DF_Comentario_Fecha DEFAULT SYSUTCDATETIME(),
        -- 0 = Activo; 1 = En revision. La interfaz decide el texto que muestra.
        EnRevision BIT NOT NULL CONSTRAINT DF_Comentario_EnRevision DEFAULT (0),
        CONSTRAINT FK_Comentario_Producto FOREIGN KEY (ProductoId) REFERENCES dbo.Producto(ProductoId),
        CONSTRAINT FK_Comentario_Autor FOREIGN KEY (AutorId) REFERENCES dbo.Usuario(UsuarioId),
        CONSTRAINT CK_Comentario_TextoNoVacio CHECK (LEN(LTRIM(RTRIM(Texto))) > 0)
    );
END;
GO

-- Tambien se aplican sobre una base creada con una version anterior del script.
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE parent_object_id = OBJECT_ID(N'dbo.Usuario') AND name = N'CK_Usuario_HashNoVacio')
    ALTER TABLE dbo.Usuario WITH CHECK ADD CONSTRAINT CK_Usuario_HashNoVacio CHECK (LEN(LTRIM(RTRIM(HashContrasena))) > 0);
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE parent_object_id = OBJECT_ID(N'dbo.Usuario') AND name = N'CK_Usuario_NombreNoVacio')
    ALTER TABLE dbo.Usuario WITH CHECK ADD CONSTRAINT CK_Usuario_NombreNoVacio CHECK (LEN(LTRIM(RTRIM(Nombre))) > 0);
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE parent_object_id = OBJECT_ID(N'dbo.Usuario') AND name = N'CK_Usuario_ApellidosNoVacios')
    ALTER TABLE dbo.Usuario WITH CHECK ADD CONSTRAINT CK_Usuario_ApellidosNoVacios CHECK (LEN(LTRIM(RTRIM(Apellidos))) > 0);
GO

IF OBJECT_ID(N'dbo.Reporte', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Reporte (
        ReporteId BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Reporte PRIMARY KEY,
        ComentarioId BIGINT NOT NULL,
        AutorId BIGINT NOT NULL,
        Motivo NVARCHAR(1000) NOT NULL,
        FechaCreacion DATETIME2(3) NOT NULL CONSTRAINT DF_Reporte_Fecha DEFAULT SYSUTCDATETIME(),
        CONSTRAINT FK_Reporte_Comentario FOREIGN KEY (ComentarioId) REFERENCES dbo.Comentario(ComentarioId),
        CONSTRAINT FK_Reporte_Autor FOREIGN KEY (AutorId) REFERENCES dbo.Usuario(UsuarioId),
        CONSTRAINT UQ_Reporte_ComentarioAutor UNIQUE (ComentarioId, AutorId),
        CONSTRAINT CK_Reporte_MotivoNoVacio CHECK (LEN(LTRIM(RTRIM(Motivo))) > 0)
    );
END;
GO

IF OBJECT_ID(N'dbo.Valoracion', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Valoracion (
        ValoracionId BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Valoracion PRIMARY KEY,
        ProductoId BIGINT NOT NULL,
        AutorId BIGINT NOT NULL,
        Voto INT NOT NULL,
        Comentario NVARCHAR(1000) NULL,
        FechaCreacion DATETIME2(3) NOT NULL CONSTRAINT DF_Valoracion_Fecha DEFAULT SYSUTCDATETIME(),
        CONSTRAINT FK_Valoracion_Producto FOREIGN KEY (ProductoId) REFERENCES dbo.Producto(ProductoId),
        CONSTRAINT FK_Valoracion_Autor FOREIGN KEY (AutorId) REFERENCES dbo.Usuario(UsuarioId),
        CONSTRAINT CK_Valoracion_Voto CHECK (Voto BETWEEN 1 AND 5)
    );
END;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Producto') AND name = N'IX_Producto_Vendedor')
    CREATE INDEX IX_Producto_Vendedor ON dbo.Producto(VendedorId);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Producto') AND name = N'IX_Producto_Categoria')
    CREATE INDEX IX_Producto_Categoria ON dbo.Producto(CategoriaId);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Comentario') AND name = N'IX_Comentario_ProductoFecha')
    CREATE INDEX IX_Comentario_ProductoFecha ON dbo.Comentario(ProductoId, FechaCreacion DESC, ComentarioId DESC);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Comentario') AND name = N'IX_Comentario_Autor')
    CREATE INDEX IX_Comentario_Autor ON dbo.Comentario(AutorId);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Reporte') AND name = N'IX_Reporte_Autor')
    CREATE INDEX IX_Reporte_Autor ON dbo.Reporte(AutorId);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Valoracion') AND name = N'IX_Valoracion_ProductoFecha')
    CREATE INDEX IX_Valoracion_ProductoFecha ON dbo.Valoracion(ProductoId, FechaCreacion DESC, ValoracionId DESC);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Valoracion') AND name = N'IX_Valoracion_Autor')
    CREATE INDEX IX_Valoracion_Autor ON dbo.Valoracion(AutorId);
GO
