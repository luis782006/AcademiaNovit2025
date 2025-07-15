# 1. Etapa de compilación: Usa la imagen del SDK de .NET 9.0
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia los archivos de proyecto (.csproj y .sln) y restaura las dependencias primero.
# Esto aprovecha el cache de Docker, para no tener que restaurar dependencias en cada cambio de código.
COPY ["AcademiaNovit/AcademiaNovit.csproj", "AcademiaNovit/"]
COPY ["AcademiaNovit.Tests/AcademiaNovit.Tests.csproj", "AcademiaNovit.Tests/"]
COPY ["AcademiaNovit.sln", "."]
RUN dotnet restore "./AcademiaNovit.sln"

# Copia el resto del código fuente
COPY . .

# Publica la aplicación en modo Release
WORKDIR "/src/AcademiaNovit"
RUN dotnet publish "AcademiaNovit.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 2. Etapa final: Usa la imagen de tiempo de ejecución de ASP.NET 9.0 (más liviana)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Copia los artefactos compilados desde la etapa de compilación
COPY --from=build /app/publish .

# Expone los puertos estándar para HTTP y HTTPS
EXPOSE 8080
EXPOSE 8081

# Define el punto de entrada para ejecutar la aplicación
ENTRYPOINT ["dotnet", "AcademiaNovit.dll"]
