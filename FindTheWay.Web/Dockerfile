FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["FindTheWay.Web/FindTheWay.Web.csproj", "FindTheWay.Web/"]
COPY ["FindTheWay.Common/FindTheWay.Common.csproj", "FindTheWay.Common/"]
RUN dotnet restore "FindTheWay.Web/FindTheWay.Web.csproj"
COPY . .
WORKDIR "/src/FindTheWay.Web"
RUN dotnet build "FindTheWay.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FindTheWay.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FindTheWay.Web.dll"]