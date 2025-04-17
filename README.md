# Academia Novit 2025

## Requisitos

Antes de ejecutar el proyecto, asegúrate de cumplir con los siguientes requisitos:

1. **.NET SDK**: Instala la versión 8.0 o superior del SDK de .NET. Puedes descargarlo desde [dotnet.microsoft.com](https://dotnet.microsoft.com/).
2. **SQLite**: El proyecto utiliza SQLite como base de datos. No se requiere configuración adicional, ya que el archivo `Data.db` se genera automáticamente.
3. **Visual Studio (opcional)**: Si prefieres usar un entorno gráfico, instala Visual Studio 2022 o superior con la carga de trabajo de desarrollo web y de escritorio .NET.

## Configuración

1. Clona este repositorio en tu máquina local:
```bash
git clone https://github.com/tu-usuario/AcademiaNovit2025.git
cd AcademiaNovit2025
```

2. Restaura las dependencias del proyecto:
```bash
dotnet restore
```

3. Aplica las migraciones para configurar la base de datos:

```bash
dotnet ef database update --project AcademiaNovit
```


## Ejecución del Proyecto

Desde la Línea de Comandos
1. Navega al directorio del proyecto principal:

```bash
cd AcademiaNovit
```

2. Ejecuta el proyecto:

```bash
dotnet run
```

3. Accede a la aplicación en tu navegador en http://localhost:5286/scalar

Desde Visual Studio

1. Abre el archivo de solución `AcademiaNovit.sln` en Visual Studio.
2. Selecciona el proyecto `AcademiaNovit` como proyecto de inicio.
3. Presiona `F5` o haz clic en el botón de "Iniciar" para ejecutar la aplicación.
4. Accede a la aplicación en tu navegador en http://localhost:5286/scalar

## Pruebas
Para ejecutar las pruebas unitarias, utiliza el siguiente comando en la raíz del proyecto:

```bash
dotnet test
```

Estructura del Proyecto
- **AcademiaNovit/:** Contiene el código fuente principal de la aplicación.
- **AcademiaNovit.Tests/:** Contiene las pruebas unitarias del proyecto.
- **Migrations/:** Contiene las migraciones de la base de datos generadas por Entity Framework.
- **Util/:** Contiene utilidades como validadores.

## Notas
- Si necesitas cambiar la configuración de la base de datos, edita el archivo `appsettings.json` o `appsettings.Development.json`.
- Asegúrate de que el archivo `Data.db` esté en el directorio raíz del proyecto `AcademiaNovit` para que la base de datos funcione correctamente.

## DevSecOps

1. Docker Desktop - [https://www.docker.com/products/docker-desktop/](https://www.docker.com/products/docker-desktop/)
2. Virtual Box 7.1.4 - [https://download.virtualbox.org/virtualbox/7.1.4/VirtualBox-7.1.4-165100-Win.exe](https://download.virtualbox.org/virtualbox/7.1.4/VirtualBox-7.1.4-165100-Win.exe)
3. Virtual Machine - Ubuntu Server 24.04 (link WeTransfer)