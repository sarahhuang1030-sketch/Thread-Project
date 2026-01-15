FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# copy csproj files (paths are relative to the build context, which is now the parent folder)
COPY Workshop04/Workshop04.csproj Workshop04/
COPY Workshop04.Data/Workshop04.Data.csproj Workshop04.Data/
RUN dotnet restore Workshop04/Workshop04.csproj

# copy the rest of the source
COPY . .
RUN dotnet publish Workshop04/Workshop04.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "Workshop04.dll"]