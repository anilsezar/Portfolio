# https://devblogs.microsoft.com/dotnet/improving-multiplatform-container-support/
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
LABEL  maintainer="Anil Sezer - msnanil@hotmail.com"

ARG TARGETPLATFORM
ARG TARGETOS
ARG TARGETARCH
ARG TARGETVARIANT
ARG BUILDPLATFORM
ARG BUILDOS
ARG BUILDARCH
ARG BUILDVARIANT

RUN echo "Building on $BUILDPLATFORM, targeting $TARGETPLATFORM"
RUN echo "Building on ${BUILDOS} and ${BUILDARCH} with optional variant ${BUILDVARIANT}"
RUN echo "Targeting ${TARGETOS} and ${TARGETARCH} with optional variant ${TARGETVARIANT}"

EXPOSE 80
EXPOSE 443

WORKDIR /src

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG TARGETARCH

COPY . .
# todo: replace -a with --arch 
# todo: replace -c with --configuration
# todo: replace -o with --output

RUN dotnet restore "Portfolio.WebUi/Portfolio.WebUi.csproj" -a $TARGETARCH
RUN dotnet build "Portfolio.WebUi/Portfolio.WebUi.csproj" -c Release -o /app/build -a $TARGETARCH


FROM build AS publish
# RUN dotnet publish "Portfolio.WebUi.csproj" -c Release -o /app/publish /p:UseAppHost=false
ARG TARGETARCH

RUN dotnet publish "Portfolio.WebUi/Portfolio.WebUi.csproj" -o /app/publish --no-restore -a $TARGETARCH

FROM base AS final
# WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Portfolio.WebUi.dll"]