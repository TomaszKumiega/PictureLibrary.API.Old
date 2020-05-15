FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

COPY *.sln .
COPY ./PictureLibrary-RaspberryAPI/ ./PictureLibrary-RaspberryAPI
RUN dotnet restore

COPY PictureLibrary-RaspberryAPI/. ./PictureLibrary-RaspberryAPI
WORKDIR /app/PictureLibrary-RaspberryAPI
RUN dotnet build

FROM build AS publish
WORKDIR /app/PictureLibrary-RaspberryAPI
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=publish /app/PictureLibrary-RaspberryAPI/out ./
ENTRYPOINT ["dotnet", "/PictureLibrary-RaspberryAPI.dll"]
