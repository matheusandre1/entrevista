FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src

COPY ["Projeto/Projeto.csproj", "Projeto/"]
COPY ["Projeto.Core/Projeto.Core.shproj", "Projeto.Core/"]
COPY ["Projeto.Tests/Projeto.Tests.csproj", "Projeto.Tests/"]

WORKDIR /src/Projeto
RUN dotnet restore "Projeto.csproj" -r linux-x86

WORKDIR /src
COPY . .

WORKDIR /src/Projeto
RUN dotnet build "Projeto.csproj" -c Release -o /app/build -r linux-x86 --no-restore

FROM build AS publish
RUN dotnet publish "Projeto.csproj" -c Release -o /app/publish -r linux-x86 --no-restore --no-build

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS final

RUN apt-get update && \
    apt-get install -y --no-install-recommends \
    libsqlite3-0 \
    && rm -rf /var/lib/apt/lists/*

RUN groupadd -r appuser && useradd -r -g appuser appuser

WORKDIR /app

COPY --from=publish /app/publish .

COPY --from=build /src/Projeto/banco.db ./banco.db 2>/dev/null || true

RUN chown -R appuser:appuser /app

USER appuser

EXPOSE 80

HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD curl -f http://localhost:80/ || exit 1


ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "Projeto.dll"]
