FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /pottercharacter
COPY ./out .
ENV ASPNETCORE_URLS http://*:80
ENTRYPOINT ["dotnet", "Potter.Characters.Api.dll"]