FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/BackBookRentals.Api/BackBookRentals.Api.csproj", "BackBookRentals.Api/"]
COPY ["src/BackBookRentals.Dto/BackBookRentals.Dto.csproj", "BackBookRentals.Dto/"]
COPY ["src/BackBookRentals.Entities/BackBookRentals.Entities.csproj", "BackBookRentals.Entities/"]
COPY ["src/BackBookRentals.Persistence/BackBookRentals.Persistence.csproj", "BackBookRentals.Persistence/"]
COPY ["src/BackBookRentals.Repositories/BackBookRentals.Repositories.csproj", "BackBookRentals.Repositories/"]
COPY ["src/BackBookRentals.Services/BackBookRentals.Services.csproj", "BackBookRentals.Services/"]
RUN dotnet restore "BackBookRentals.Api/BackBookRentals.Api.csproj"
COPY ./src .
WORKDIR "/src/BackBookRentals.Api"
RUN dotnet build "BackBookRentals.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BackBookRentals.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=publish /app/publish/appsettings.json .
COPY --from=publish /app/publish/appsettings.Development.json .
COPY --from=publish /app/publish/appsettings.Docker.json .
ENTRYPOINT ["dotnet", "BackBookRentals.Api.dll"]