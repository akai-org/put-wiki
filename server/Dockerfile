FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS base
WORKDIR /app

COPY ["PutWiki.slnx", "./"]
COPY ["Directory.Build.props", "./"]
COPY ["Directory.Packages.props", "./"]

COPY ["Presentation/*.csproj", "Presentation/packages.lock.json*", "Presentation/"]
COPY ["Application/*.csproj", "Application/packages.lock.json*", "Application/"]
COPY ["Infrastructure/*.csproj", "Infrastructure/packages.lock.json*", "Infrastructure/"]
COPY ["Domain/*.csproj", "Domain/packages.lock.json*", "Domain/"]
 
RUN dotnet restore "Presentation/Presentation.csproj" --locked-mode

FROM base AS publish
COPY . .
RUN dotnet publish "Presentation/Presentation.csproj" -c Release -o /app/out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS prod
WORKDIR /app
RUN apk add --no-cache krb5-libs curl
COPY --from=publish /app/out .

ENTRYPOINT ["dotnet", "Presentation.dll"]