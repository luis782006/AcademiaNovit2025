# Imagen base con el SDK para compilar el proyecto
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia los archivos del proyecto
COPY ["AcademiaNovit/AcademiaNovit.csproj", "AcademiaNovit/"]
COPY ["AcademiaNovit.Tests/AcademiaNovit.Tests.csproj", "AcademiaNovit.Tests/"]
COPY AcademiaNovit.sln .

# Restaurar paquetes NuGet
RUN dotnet restore AcademiaNovit.sln

# Copiar todo el código
COPY . .

# Publicar la aplicación (compilar)
WORKDIR "/src/AcademiaNovit"
RUN dotnet publish AcademiaNovit.csproj -c Release -o /app/publish /p:UseAppHost=false

# Imagen final con runtime (más liviana)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Copiar la aplicación compilada
COPY --from=build /app/publish .

# Exponer puerto
EXPOSE 8080

# Ejecutar la API al iniciar el contenedor
ENTRYPOINT ["dotnet", "AcademiaNovit.dll"]