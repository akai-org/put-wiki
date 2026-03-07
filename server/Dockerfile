FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS base
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

FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS prod
WORKDIR /app
COPY --from=publish /app/out .
ENTRYPOINT ["dotnet", "Presentation.dll"]