FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

MAINTAINER fpeterek@seznam.cz

COPY *.cs fleet.xml pl_projekt.csproj /fpeterek/dotnot/project/
WORKDIR /fpeterek/dotnot/project/
RUN dotnet build

ENTRYPOINT ["./bin/Debug/netcoreapp3.1/pl_projekt"]
