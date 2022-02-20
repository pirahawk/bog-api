# FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as builder

# WORKDIR /app
# COPY . /app

# RUN dotnet restore
# RUN dotnet publish -o ./bogapi


# FROM mcr.microsoft.com/dotnet/core/aspnet:latest as run


# WORKDIR /bogapi
# COPY --from=builder /app/bogapi ./
# ENTRYPOINT ["dotnet", "Bog.Api.Web.dll"]


FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src
COPY . .
RUN dotnet restore "Bog.Api.Web/Bog.Api.Web.csproj"
RUN dotnet build "Bog.Api.Web/Bog.Api.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bog.Api.Web/Bog.Api.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bog.Api.Web.dll"]