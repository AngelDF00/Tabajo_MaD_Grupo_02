
# Plantilla 

Esta es la solución base y plantilla inicial para la realización de la práctica de la asignatura. El repositorio contiene una estructura preconfigurada con los proyectos necesarios y las referencias extenas configuradas para que puedas centrarte en la implementación de la lógica de negocio y las pruebas.

Se trata de una plantilla de uso OPCIONAL para facilitar los primeros pasos, pero es perfectamente válido (incluso recomendable) comenzar el desarrollo desde cero.

---

## 📂 Estructura de la Solución

La solución de Visual Studio (`.sln`) está compuesta por tres proyectos principales:

1. **`Model` (Capa Modelo):**

* Contiene las entidades del dominio, la lógica de negocio y los servicios necesarios.
* Configurada como proyecto de librería para que genere una .dll que pueda ser llamada desde el proyecto Web. En los pasos inciales puede configurarse como proyecto de consola para que genere un .exe con el que sea más sencillo visualizar por pantalla el estado de ejecución (si no se usa el proyecto de test)
* **Estado actual:** No funcional. Debes implementar las clases, interfaces y lógica requeridas según el enunciado de la práctica.


2. **`Test` (Proyecto de pruebas unitarias):**
* Destinado a albergar las pruebas unitarias para verificar el correcto funcionamiento del proyecto `Model`.
* **Estado actual:**  No funcional. Incluye la estructura básica y las referencias al proyecto `Model`. Deberás añadir tus propios casos de prueba.


3. **`Web` (Aplicación Web / API):**
* Interfaz de usuario o punto de entrada de la aplicación que consume los servicios del proyecto `Model`.
* **Estado actual:**  No funcional. Contiene la configuración inicial pero carece de la funcionalidad completa.



---

## ⚙️ Requisitos Previos

* **Visual Studio** versiones 2019, 2022 o 20126 recomendadas.
* **.NET SDK** correspondiente a la versión utilizada en la solución (lo actualizará automáticamente VStudio al iniciarse).

---


## ⚙️ Integración GIT

* Puedes gestionar todo el control de versiones (crear ramas, realizar commits, pushes o fusiones) desde consola de comandos o utilizando las herramientas integradas de Git en Visual Studio

* Ventana de Git Changes: Menú View > Git Changes para ver los archivos modificados, gestionar commits y sincronizar cambios con repositorio remoto.

* Gestión de ramas: Menú Git > Manage Branches 

* Extensiones compatibles: también se pueden instalar extensiones para la gestión de git (e.g. GitHub Extension for Visual Studio)

---


## 📝 Instrucciones para la Entrega

* Implementa los requisitos detallados en el enunciado de la práctica dentro de los proyectos correspondientes.
* Asegúrate de que todas las pruebas unitarias pasen satisfactoriamente antes de realizar la entrega final.
* Realiza commits frecuentes y descriptivos en tu repositorio.
* Para la primera corrección se creará un tag iteracion-1 que incluirá el código a revisar en la defensa.
* Para la segunda corrección se creará un tag iteracion-2 que incluirá el código a revisar en la defensa. Incluir en el repositorio la memoria de la práctica.