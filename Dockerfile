FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
MAINTAINER Robson Pedroso
WORKDIR /app

COPY . .
COPY . src/QuakeLog.API/assets/*.txt

RUN dotnet restore

RUN dotnet publish src/QuakeLog.API/QuakeLog.API.csproj \
    -c Release \
    -o dist \
    --no-restore

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=build /app/dist .
ENTRYPOINT ["dotnet", "QuakeLog.API.dll"]
EXPOSE 80