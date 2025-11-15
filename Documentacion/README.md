# ClinicaWeb - Prueba Técnica

**Autor:** Juan Pablo Domínguez — Ingeniero de Sistemas  
**Año:** 2025  

---

## 1. Descripción del proyecto
ClinicaWeb es un sistema de agendamiento de citas médicas que permite:

- Registrar nuevas citas para pacientes con doctores disponibles.  
- Listar todas las citas con filtros avanzados por paciente, doctor y estado.  
- Cancelar citas respetando la regla de 24 horas de antelación.  
- Ordenar y paginar las citas para facilitar la visualización.  
- Mantener un historial organizado de citas programadas y canceladas.  

Este proyecto fue desarrollado como prueba técnica, utilizando ASP.NET Web Forms, Entity Framework y SQL Server.

---

## 2. Requisitos del sistema
Para ejecutar este proyecto, necesitas:

- **Visual Studio 2022** o superior.  
- **.NET Framework 4.8* .  
- **SQL Server LocalDB** o **SQL Server 2019**.  
- **Conexión a Internet** para cargar los recursos de Bootstrap desde CDN.  

---

## 3. Configuración de la base de datos
1. Crear la base de datos en SQL Server.  
2. Restaurar el archivo `ClinicaDB.bak` incluido en el repositorio.  
3. Configurar la cadena de conexión en el archivo `Web.config`:

------------------------------------------------------------------------------------------

```xml
<connectionStrings>
    <add name="ClinicaDBEntities"
         connectionString="metadata=res://*/Models.ClinicaDB.csdl|res://*/Models.ClinicaDB.ssdl|res://*/Models.ClinicaDB.msl;
                           provider=System.Data.SqlClient;
                           provider connection string='data source=TU_SERVIDOR;
                                                     initial catalog=ClinicaDB;
                                                     integrated security=True;
                                                     MultipleActiveResultSets=True;
                                                     App=EntityFramework'"
         providerName="System.Data.EntityClient" />
</connectionStrings>

------------------------------------------------------------------------------------------

Importante: Reemplaza TU_SERVIDOR con el nombre de tu servidor SQL.

4. Ejecución del proyecto

- Abrir la solución .sln en Visual Studio.
- Restaurar paquetes NuGet si es necesario.
- Ejecutar el proyecto presionando F5.
- Acceder en el navegador a:

http://localhost:xxxx/AgendarCita.aspx para agendar citas.

http://localhost:xxxx/ListadoCitas.aspx para ver el listado de citas.

5. Contenido del repositorio

El repositorio incluye:

- ClinicaWeb/: Carpeta con toda la solución de Visual Studio y código fuente.

- ClinicaDB.bak: Base de datos utilizada en la prueba con datos de ejemplo.

- README.md: Este documento con instrucciones de configuración y ejecución.

6. Estructura de la base de datos

La base de datos contiene las siguientes tablas:

- Paciente: Información de los pacientes (Nombre, Apellido, ID).
- Doctor: Información de los doctores (Nombre, Apellido, ID).
- Cita: Registro de citas médicas con:
- IdPaciente
- IdDoctor
- FechaHora
- Motivo
- Estado (Programada o Cancelada)


Todas las tablas incluyen datos de ejemplo para realizar pruebas de agendamiento, filtrado y cancelación.

7. Funcionalidades principales
7.1 Agendar Cita: Permite seleccionar paciente, doctor, fecha, hora y motivo.
7.2 Listado de Citas:
    - Filtros por paciente, doctor y estado.
    - Ordenamiento por columnas (Fecha, Hora, Paciente, Doctor).
    - Paginación para mejorar la visualización.


7.3 Cancelar Cita:
-Solo se permite cancelar con al menos 24 horas de antelación.
-Cambia el estado a "Cancelada" y muestra un mensaje de confirmación.

8. Autor
Juan Pablo Domínguez — Ingeniero de Sistemas
Este proyecto fue desarrollado como prueba técnica para demostrar competencias en desarrollo web, bases de datos y buenas prácticas de codificación en ASP.NET Web Forms.