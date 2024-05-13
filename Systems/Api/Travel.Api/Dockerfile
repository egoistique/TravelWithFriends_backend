#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Systems/Api/Travel.Api/Travel.Api.csproj", "Systems/Api/Travel.Api/"]
COPY ["Data/Travel.Context.Migrations/Travel.Context.Migrations.PgSql/Travel.Context.Migrations.PgSql.csproj", "Data/Travel.Context.Migrations/Travel.Context.Migrations.PgSql/"]
COPY ["Data/Travel.Context/Travel.Context.csproj", "Data/Travel.Context/"]
COPY ["Services/Travel.Services.Settings/Travel.Services.Settings.csproj", "Services/Travel.Services.Settings/"]
COPY ["Shared/Travel.Common/Travel.Common.csproj", "Shared/Travel.Common/"]
COPY ["Data/Travel.Context.Entities/Travel.Context.Entities.csproj", "Data/Travel.Context.Entities/"]
COPY ["Data/Travel.Context.Seeder/Travel.Context.Seeder.csproj", "Data/Travel.Context.Seeder/"]
COPY ["Services/Travel.Services.UserAccount/Travel.Services.UserAccount.csproj", "Services/Travel.Services.UserAccount/"]
COPY ["Services/Travel.Services.Activities/Travel.Services.Activities.csproj", "Services/Travel.Services.Activities/"]
COPY ["Services/Travel.Services.Categories/Travel.Services.Categories.csproj", "Services/Travel.Services.Categories/"]
COPY ["Services/Travel.Services.Logger/Travel.Services.Logger.csproj", "Services/Travel.Services.Logger/"]
COPY ["Services/Travel.Services.Stat/Travel.Services.Stat.csproj", "Services/Travel.Services.Stat/"]
COPY ["Services/Travel.Services.Trips/Travel.Services.Trips.csproj", "Services/Travel.Services.Trips/"]
COPY ["Systems/Api/Travel.Api.Settings/Travel.Api.Settings.csproj", "Systems/Api/Travel.Api.Settings/"]
RUN dotnet restore "./Systems/Api/Travel.Api/./Travel.Api.csproj"
COPY . .
WORKDIR "/src/Systems/Api/Travel.Api"
RUN dotnet build "./Travel.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Travel.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Travel.Api.dll"]