#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["livil-mq-microservice/livil-mq-microservice.csproj", "livil-mq-microservice/"]
RUN dotnet restore "livil-mq-microservice/livil-mq-microservice.csproj"
COPY . .
WORKDIR "/src/livil-mq-microservice"
RUN dotnet build "livil-mq-microservice.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "livil-mq-microservice.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "livil-mq-microservice.dll"]