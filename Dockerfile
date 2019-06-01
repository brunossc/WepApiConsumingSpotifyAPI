FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["WebApiSpotify.csproj", "./"]
RUN dotnet restore "./WebApiSpotify.csproj"
COPY . .
WORKDIR /src/.
RUN dotnet build "WebApiSpotify.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "WebApiSpotify.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WebApiSpotify.dll"]
