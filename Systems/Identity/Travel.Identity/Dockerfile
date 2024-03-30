#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Systems/Identity/Travel.Identity/Travel.Identity.csproj", "Systems/Identity/Travel.Identity/"]
COPY ["Data/Travel.Context/Travel.Context.csproj", "Data/Travel.Context/"]
COPY ["Services/Travel.Services.Settings/Travel.Services.Settings.csproj", "Services/Travel.Services.Settings/"]
COPY ["Shared/Travel.Common/Travel.Common.csproj", "Shared/Travel.Common/"]
COPY ["Data/Travel.Context.Entities/Travel.Context.Entities.csproj", "Data/Travel.Context.Entities/"]
RUN dotnet restore "./Systems/Identity/Travel.Identity/./Travel.Identity.csproj"
COPY . .
WORKDIR "/src/Systems/Identity/Travel.Identity"
RUN dotnet build "./Travel.Identity.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Travel.Identity.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Travel.Identity.dll"]