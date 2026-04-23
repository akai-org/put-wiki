ARG DOTNET_VERSION=10.0

FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION}-alpine AS base
WORKDIR /app

COPY ["PutWiki.slnx", "./"]
COPY ["Directory.Build.props", "./"]
COPY ["Directory.Packages.props", "./"]

COPY ["Presentation/*.csproj", "Presentation/"]
COPY ["Application/*.csproj", "Application/"]
COPY ["Infrastructure/*.csproj", "Infrastructure/"]
COPY ["Domain/*.csproj", "Domain/"]
 
RUN dotnet restore "Presentation/Presentation.csproj"

FROM base AS publish
COPY . .
RUN dotnet publish "Presentation/Presentation.csproj" -c Release -o /app/out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION}-alpine AS prod
WORKDIR /app
RUN apk add --no-cache curl
COPY --from=publish /app/out .

HEALTHCHECK CMD curl --fail http://localhost:3000/health || exit 1

ENTRYPOINT ["dotnet", "Presentation.dll"]