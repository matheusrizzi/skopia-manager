# Usar imagem oficial do .NET SDK para build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copia csproj e restaura dependências
COPY *.sln .
COPY src ./src
COPY tests ./tests
RUN dotnet restore

# Build e publish
RUN dotnet publish src/SkopiaManager.API/SkopiaManager.API.csproj -c Release -o /app/publish

# Imagem final de runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:80
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SkopiaManager.API.dll"]
