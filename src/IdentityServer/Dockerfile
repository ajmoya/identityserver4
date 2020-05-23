FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./

# Tenemos que instalar en la máquina linux donde correrá IdentityServer4 el certificado self-signed de Unatec
# para poder hacer un dotnet restore, ya que tenemos packages nuget del repo SSL de Unatec
COPY cert_unatec.cer /usr/local/share/ca-certificates/cert_unatec.crt
RUN update-ca-certificates

RUN dotnet nuget add source https://repositoriopaquetes.intranet.unatec.es:83/nuget/unatec_interno/ -n Unatec && dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out


# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "IdentityServer.dll", "--environment=dev"]