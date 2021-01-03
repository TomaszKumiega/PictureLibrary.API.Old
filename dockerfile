FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

COPY *.sln .
COPY ./PictureLibrary-API/ ./PictureLibrary-API
RUN dotnet restore

COPY PictureLibrary-API/. ./PictureLibrary-API
WORKDIR /app/PictureLibrary-API
RUN dotnet build

FROM build AS publish
WORKDIR /app/PictureLibrary-API
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=publish /app/PictureLibrary-API/out ./
ENTRYPOINT ["dotnet", "PictureLibrary-API.dll"]
