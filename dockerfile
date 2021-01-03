FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.2-buster-slim-arm32v7 AS base
WORKDIR /app
EXPOSE 5001
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY PictureLibrary-API.csproj ./
RUN dotnet restore -r linux-arm

COPY . .
WORKDIR /src/.
RUN dotnet build -r linux-arm

FROM build AS publish
WORKDIR /app/PictureLibrary-API
RUN dotnet publish "PictureLibrary-API" -c Release -o /app/publish -r linux-arm

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish ./
ENTRYPOINT ["dotnet", "PictureLibrary-API.dll"]
