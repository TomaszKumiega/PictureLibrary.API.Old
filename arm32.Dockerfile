FROM mcr.microsoft.com/dotnet/sdk:7.0.202-bullseye-slim-arm32v7 AS base
WORKDIR /app
EXPOSE 5001
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

COPY *.sln .
COPY ./PictureLibrar.API/ ./PictureLibrary.API
RUN dotnet restore -r linux-arm

COPY PictureLibrary.API/. ./PictureLibrary.API
WORKDIR /app/PictureLibrary.API 
RUN dotnet build -r linux-arm

FROM build AS publish
WORKDIR /app/PictureLibrary.API
RUN dotnet publish -c Release -o out -r linux-arm

FROM base AS final
WORKDIR /app
COPY --from=publish /app/PictureLibrary.API/out ./
ENTRYPOINT ["dotnet", "PictureLibrary.API.dll"]
