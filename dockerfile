FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

COPY *.sln .
COPY ./PictureLibrary.API/ ./PictureLibrary.API
COPY ./PictureLibrary.APIClient/ ./PictureLibrary.APIClient
COPY ./PictureLibrary.DataAccess/ ./PictureLibrary.DataAccess
COPY ./PictureLibrary.Model/ ./PictureLibrary.Model
COPY ./PictureLibrary.Tools/ ./PictureLibrary.Tools
COPY ./PictureLibrary.DataAccess.Tests/ ./PictureLibrary.DataAccess.Tests
RUN dotnet restore

COPY ./PictureLibrary.API/ ./PictureLibrary.API
COPY ./PictureLibrary.APIClient/ ./PictureLibrary.APIClient
COPY ./PictureLibrary.DataAccess/ ./PictureLibrary.DataAccess
COPY ./PictureLibrary.Model/ ./PictureLibrary.Model
COPY ./PictureLibrary.Tools/ ./PictureLibrary.Tools
COPY ./PictureLibrary.DataAccess.Tests/ ./PictureLibrary.DataAccess.Tests
WORKDIR /app/PictureLibrary.API
RUN dotnet build

FROM build AS publish
WORKDIR /app/PictureLibrary.API
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/sdk:7.0
WORKDIR /app
COPY --from=publish /app/PictureLibrary.API/out ./
ENTRYPOINT ["dotnet", "PictureLibrary.API.dll"]
