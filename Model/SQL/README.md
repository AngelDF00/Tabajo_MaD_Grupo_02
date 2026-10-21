# Base de datos de la primera iteración

Este directorio contiene el esquema común confirmado para los tres bloques del modelo y datos ficticios para una demostración.

## Ejecutar en SQL Server LocalDB

Desde la raíz del repositorio, en PowerShell:

```powershell
sqllocaldb create MSSQLLocalDB
sqllocaldb start MSSQLLocalDB
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -b -i Model\SQL\SqlServerCreateDatabase.sql
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -b -d MAD -i Model\SQL\SqlServerCreateTables.sql
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -b -d MAD -i Model\SQL\SqlServerInsertDemoData.sql
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -b -i Test\Scripts\SqlServerCreateTables.Test.sql
```

Si la instancia `MSSQLLocalDB` ya existe, se omite `sqllocaldb create`. Los scripts no borran datos existentes y pueden ejecutarse de nuevo. El script de pruebas usa `:r`, una directiva de `sqlcmd`; en SSMS hay que activar el modo SQLCMD y resolver la ruta desde la raíz del repositorio. La base de pruebas se llama `PracticaMAD_Grupo02_Test` y queda vacía para que cada prueba cree y limpie sus propios datos. `Test/App.config` apunta solo a esa base mediante autenticación integrada de Windows; no se guardan contraseñas de SQL Server.

## Tablas y responsables de revisión

| Tabla | Claves y relaciones | Revisar con |
| --- | --- | --- |
| `Usuario` | `UsuarioId` PK; `Login` único; `HashContrasena`, nombre, apellidos, correo, idioma y país | Ángel |
| `Categoria` | `CategoriaId` PK; nombre | Diego |
| `Producto` | `ProductoId` PK; FK a vendedor `Usuario` y a `Categoria`; nombre y fecha de alta | Diego y Marcos |
| `Comentario` | `ComentarioId` PK; FK a producto y autor; texto, fecha, `EnRevision` | Diego |
| `Reporte` | `ReporteId` PK; FK a comentario y autor; motivo, fecha; único `(ComentarioId, AutorId)` | Diego y Marcos |
| `Valoracion` | `ValoracionId` PK; FK a producto y autor; voto 1–5, comentario opcional y fecha | Marcos |

Todos los identificadores son `BIGINT IDENTITY`. Las fechas se guardan en UTC con `DATETIME2(3)`. Las claves externas no tienen borrado en cascada. El vendedor valorado se obtiene de `Valoracion.ProductoId -> Producto.VendedorId`: no hay un segundo `VendedorId` en `Valoracion`. La media y el total de votos se calculan al consultar; tampoco hay una restricción de un solo voto por usuario.

## Detalles de implementación por concretar

- El SQL de demostración usa PBKDF2-SHA256 con 100 000 iteraciones, sal aleatoria y contraseña pública `Demo1234!` para tres cuentas ficticias (`demo_ana`, `demo_bruno`, `demo_celia`). El futuro servicio de autenticación debe usar el mismo formato o regenerar estos hashes con el formato elegido. Las direcciones `example.invalid` no pertenecen a usuarios reales.
- El servicio de reportes deberá cambiar `Comentario.EnRevision` en la misma transacción en la que crea el reporte y bloquear la edición de un comentario en revisión. El valor numérico del umbral no figura todavía en el repositorio; hay que registrarlo antes de implementar ese servicio. También hay que concretar si el autor puede eliminarlo cuando está en revisión.
- El esquema permite categorías con el mismo nombre porque el enunciado no exige unicidad. El script de demostración evita añadir otra categoría con el mismo nombre.
- Al implementar los servicios, hay que fijar la semántica de la búsqueda y la paginación, y comprobar que las clases de Entity Framework 6 mapean estas tablas y columnas sin que el inicializador de EF intente recrearlas.

Los datos de demostración son solo ejemplos de catálogo, comentarios, un reporte y dos valoraciones. La base de pruebas se deja sin esos datos para que las pruebas de integración sean independientes.

## Relación con las notas de `Practica.txt`

- El comentario tiene dos estados: `EnRevision = 0` corresponde a Activo y `EnRevision = 1` a En revisión. La aplicación debe mostrar el estado En revisión e impedir la modificación cuando se alcance el umbral acordado. No se añade un estado Oculto porque el enunciado indica que ocultar comentarios reportados queda fuera del alcance obligatorio.
- Los `login` que se muestran se obtienen uniendo las claves externas con `Usuario`. No se copian en `Comentario`, `Reporte` o `Valoracion`, porque podrían quedar desactualizados si cambian los datos del usuario.
- `Reporte` relaciona al autor del reporte con el comentario. El vendedor se obtiene mediante `Reporte -> Comentario -> Producto -> Usuario`. No se necesita un `VendedorId` adicional.
- La media de valoraciones es un dato calculado a partir de los votos de los productos del vendedor. No se guarda en una columna ni hace falta como entidad del diagrama E-R. El servicio debe obtenerla junto con el número total de votos.
- La búsqueda por palabra clave sin distinguir mayúsculas se implementará en el servicio de productos. La redirección al formulario de acceso, el cierre de sesión y la opción de recordar la contraseña corresponden a la Web de la segunda iteración; no requieren columnas nuevas en este esquema.
