FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY ContosoCrafts.Website/*.csproj ./aspnetapp/
RUN dotnet restore ./aspnetapp/ContosoCrafts.Website.csproj

# copy everything else and build app
COPY ContosoCrafts.Website/. ./aspnetapp/
WORKDIR /app/aspnetapp
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=build /app/aspnetapp/out ./
ENTRYPOINT ["dotnet", "ContosoCrafts.Website.dll"]
