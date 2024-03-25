FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/DotnetApiTemplate.Core/DotnetApiTemplate.Core.csproj", "DotnetApiTemplate.Core/"]
COPY ["src/DotnetApiTemplate.Domain/DotnetApiTemplate.Domain.csproj", "DotnetApiTemplate.Domain/"]
COPY ["src/DotnetApiTemplate.Infrastructure/DotnetApiTemplate.Infrastructure.csproj", "DotnetApiTemplate.Infrastructure/"]
COPY ["src/DotnetApiTemplate.Persistence.Postgres/DotnetApiTemplate.Persistence.Postgres.csproj", "DotnetApiTemplate.Persistence.Postgres/"]
COPY ["src/DotnetApiTemplate.Persistence.SqlServer/DotnetApiTemplate.Persistence.SqlServer.csproj", "DotnetApiTemplate.Persistence.SqlServer/"]
COPY ["src/DotnetApiTemplate.WebApi/DotnetApiTemplate.WebApi.csproj", "DotnetApiTemplate.WebApi/"]
COPY ["src/Shared/DotnetApiTemplate.Shared.Abstractions/DotnetApiTemplate.Shared.Abstractions.csproj", "Shared/DotnetApiTemplate.Shared.Abstractions/"]
COPY ["src/Shared/DotnetApiTemplate.Shared.Infrastructure/DotnetApiTemplate.Shared.Infrastructure.csproj", "Shared/DotnetApiTemplate.Shared.Infrastructure/"]

RUN dotnet restore "DotnetApiTemplate.WebApi/DotnetApiTemplate.WebApi.csproj"
COPY . .
WORKDIR /src
RUN dotnet build "src/DotnetApiTemplate.WebApi/DotnetApiTemplate.WebApi.csproj" -c Release -o /app/build

FROM build AS publish

RUN dotnet publish "src/DotnetApiTemplate.WebApi/DotnetApiTemplate.WebApi.csproj" -c Release -o /app/publish
RUN dotnet dev-certs https -ep /app/publish/radyalabs.pfx -p pa55w0rd!


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=pa55w0rd!
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=radyalabs.pfx

ENTRYPOINT ["dotnet", "DotnetApiTemplate.WebApi.dll"]