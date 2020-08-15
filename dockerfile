FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as builder

WORKDIR /app
COPY . /app

RUN dotnet restore
RUN dotnet publish -o ./bogapi


FROM mcr.microsoft.com/dotnet/core/aspnet:latest as run


WORKDIR /bogapi
COPY --from=builder /app/bogapi ./
ENTRYPOINT ["dotnet", "Bog.Api.Web.dll"]