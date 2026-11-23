-- Datos ficticios para una demostracion. Ejecutar sobre MAD, no sobre la base de pruebas.
-- Los hashes corresponden a la clave publica de demostracion Demo1234!; nunca usarla en produccion.
-- Formato propuesto: PBKDF2-SHA256$iteraciones$salBase64$hashBase64.
SET XACT_ABORT ON;
BEGIN TRANSACTION;

IF NOT EXISTS (SELECT 1 FROM dbo.Usuario WHERE Login = N'demo_angel')
    INSERT dbo.Usuario (Login, HashContrasena, Nombre, Apellidos, Correo, Idioma, Pais)
    VALUES (N'demo_angel', N'PBKDF2-SHA256$100000$o3HmQvk6IlRejV3P6U4/mw==$0MTvUKqJlNDN89tMnuqfmFTx/lhx2RzfUu4bj0DsjMw=', N'Ángel', N'Ficticio', N'angel@example.invalid', N'es', N'España');
IF NOT EXISTS (SELECT 1 FROM dbo.Usuario WHERE Login = N'demo_diego')
    INSERT dbo.Usuario (Login, HashContrasena, Nombre, Apellidos, Correo, Idioma, Pais)
    VALUES (N'demo_diego', N'PBKDF2-SHA256$100000$LCQaibhwvVaDTxoT1CVxlw==$XBrB4EgodGNYblfJ79/Wd6QbcoEsZuv/5JFOoB0sUdc=', N'Diego', N'Ficticio', N'diego@example.invalid', N'es', N'España');
IF NOT EXISTS (SELECT 1 FROM dbo.Usuario WHERE Login = N'demo_marcos')
    INSERT dbo.Usuario (Login, HashContrasena, Nombre, Apellidos, Correo, Idioma, Pais)
    VALUES (N'demo_marcos', N'PBKDF2-SHA256$100000$vXUYAuTfZRgDill2tnrThQ==$rGawLXxoRCDCkzXrINxCSAQ/ZSrCu76ekc/qlYvB1LU=', N'Marcos', N'Ficticio', N'marcos@example.invalid', N'es', N'España');

IF NOT EXISTS (SELECT 1 FROM dbo.Categoria WHERE Nombre = N'Electrónica')
    INSERT dbo.Categoria (Nombre) VALUES (N'Electrónica');
IF NOT EXISTS (SELECT 1 FROM dbo.Categoria WHERE Nombre = N'Deporte')
    INSERT dbo.Categoria (Nombre) VALUES (N'Deporte');

DECLARE @Angel BIGINT = (SELECT UsuarioId FROM dbo.Usuario WHERE Login = N'demo_angel');
DECLARE @Diego BIGINT = (SELECT UsuarioId FROM dbo.Usuario WHERE Login = N'demo_diego');
DECLARE @Marcos BIGINT = (SELECT UsuarioId FROM dbo.Usuario WHERE Login = N'demo_marcos');
DECLARE @Electronica BIGINT = (SELECT TOP (1) CategoriaId FROM dbo.Categoria WHERE Nombre = N'Electrónica' ORDER BY CategoriaId);
DECLARE @Deporte BIGINT = (SELECT TOP (1) CategoriaId FROM dbo.Categoria WHERE Nombre = N'Deporte' ORDER BY CategoriaId);

IF NOT EXISTS (SELECT 1 FROM dbo.Producto WHERE VendedorId = @Angel AND Nombre = N'Bicicleta urbana (demo)')
    INSERT dbo.Producto (Nombre, VendedorId, CategoriaId) VALUES (N'Bicicleta urbana (demo)', @Angel, @Deporte);
IF NOT EXISTS (SELECT 1 FROM dbo.Producto WHERE VendedorId = @Angel AND Nombre = N'Casco de ciclismo (demo)')
    INSERT dbo.Producto (Nombre, VendedorId, CategoriaId) VALUES (N'Casco de ciclismo (demo)', @Angel, @Deporte);
IF NOT EXISTS (SELECT 1 FROM dbo.Producto WHERE VendedorId = @Diego AND Nombre = N'Teclado mecánico (demo)')
    INSERT dbo.Producto (Nombre, VendedorId, CategoriaId) VALUES (N'Teclado mecánico (demo)', @Diego, @Electronica);

DECLARE @Bicicleta BIGINT = (SELECT TOP (1) ProductoId FROM dbo.Producto WHERE VendedorId = @Angel AND Nombre = N'Bicicleta urbana (demo)' ORDER BY ProductoId);
DECLARE @Teclado BIGINT = (SELECT TOP (1) ProductoId FROM dbo.Producto WHERE VendedorId = @Diego AND Nombre = N'Teclado mecánico (demo)' ORDER BY ProductoId);

IF NOT EXISTS (SELECT 1 FROM dbo.Comentario WHERE ProductoId = @Bicicleta AND AutorId = @Diego AND Texto = N'Muy cómoda para trayectos cortos.')
    INSERT dbo.Comentario (ProductoId, AutorId, Texto) VALUES (@Bicicleta, @Diego, N'Muy cómoda para trayectos cortos.');
IF NOT EXISTS (SELECT 1 FROM dbo.Comentario WHERE ProductoId = @Teclado AND AutorId = @Marcos AND Texto = N'El tacto de las teclas es agradable.')
    INSERT dbo.Comentario (ProductoId, AutorId, Texto) VALUES (@Teclado, @Marcos, N'El tacto de las teclas es agradable.');

DECLARE @ComentarioBicicleta BIGINT = (SELECT TOP (1) ComentarioId FROM dbo.Comentario WHERE ProductoId = @Bicicleta AND AutorId = @Diego AND Texto = N'Muy cómoda para trayectos cortos.' ORDER BY ComentarioId);
IF NOT EXISTS (SELECT 1 FROM dbo.Reporte WHERE ComentarioId = @ComentarioBicicleta AND AutorId = @Marcos)
    INSERT dbo.Reporte (ComentarioId, AutorId, Motivo) VALUES (@ComentarioBicicleta, @Marcos, N'Dato posiblemente incorrecto (demo).');

IF NOT EXISTS (SELECT 1 FROM dbo.Valoracion WHERE ProductoId = @Bicicleta AND AutorId = @Diego AND Comentario = N'Entrega puntual (demo).')
    INSERT dbo.Valoracion (ProductoId, AutorId, Voto, Comentario) VALUES (@Bicicleta, @Diego, 5, N'Entrega puntual (demo).');
IF NOT EXISTS (SELECT 1 FROM dbo.Valoracion WHERE ProductoId = @Bicicleta AND AutorId = @Marcos AND Comentario = N'Buena comunicación (demo).')
    INSERT dbo.Valoracion (ProductoId, AutorId, Voto, Comentario) VALUES (@Bicicleta, @Marcos, 4, N'Buena comunicación (demo).');

COMMIT TRANSACTION;
