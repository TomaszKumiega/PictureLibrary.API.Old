FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim-arm32v7 AS build
WORKDIR /app
EXPOSE 5001
EXPOSE 5000

COPY *.sln .
COPY ./PictureLibrary-API/ ./PictureLibrary-API
RUN dotnet restore

COPY PictureLibrary-API/. ./PictureLibrary-API
WORKDIR /app/PictureLibrary-API
RUN dotnet build -r linux-arm

FROM build AS publish
WORKDIR /app/PictureLibrary-API
RUN dotnet publish -c Release -o out -r linux-arm

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim-arm32v7 AS final
WORKDIR /app
COPY --from=publish /app/PictureLibrary-API/out ./
ENTRYPOINT ["dotnet", "PictureLibrary-API.dll"]
