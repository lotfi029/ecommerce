# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["eCommerce.CatalogService/eCommerce.CatalogService.csproj", "eCommerce.CatalogService/"]
RUN dotnet restore "./eCommerce.CatalogService/eCommerce.CatalogService.csproj"
COPY . .
WORKDIR "/src/eCommerce.CatalogService"
RUN dotnet build "./eCommerce.CatalogService.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./eCommerce.CatalogService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV Product_Service_URI=http://localhost:7006
ENTRYPOINT ["dotnet", "eCommerce.CatalogService.dll"]