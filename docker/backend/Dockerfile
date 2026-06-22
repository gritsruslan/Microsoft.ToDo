FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY . ./

RUN ls -la

RUN dotnet restore Microsoft.ToDo.API/Microsoft.ToDo.API.csproj
RUN dotnet publish Microsoft.ToDo.API/Microsoft.ToDo.API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:7777
EXPOSE 7777

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Microsoft.ToDo.API.dll"] 