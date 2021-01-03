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
RUN sudo apt-get -y update
RUN sudo apt-get -y upgrade
RUN sudo apt-get -y install sqlite3 libsqlite3-dev
RUN mkdir /PictureLibraryAPI/db
RUN /usr/bin/sqlite3 /PictureLibraryAPI/db/PictureLibraryAPI.db
ENTRYPOINT ["dotnet", "PictureLibrary-API.dll"]
