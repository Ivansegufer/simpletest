ARG ASPNET_VERSION=10.0

FROM mcr.microsoft.com/dotnet/sdk:${ASPNET_VERSION} AS build

WORKDIR /src

COPY simpletest.Api/simpletest.Api.csproj simpletest.Api/
RUN dotnet restore simpletest.Api/simpletest.Api.csproj

COPY simpletest.Api/ simpletest.Api/
RUN dotnet publish simpletest.Api/simpletest.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:${ASPNET_VERSION} AS runtime

WORKDIR /app

COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "simpletest.Api.dll"]
