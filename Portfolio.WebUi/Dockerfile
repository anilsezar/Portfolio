# https://devblogs.microsoft.com/dotnet/improving-multiplatform-container-support/
ARG DOTNET_VERSION=8.0

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/aspnet:$DOTNET_VERSION AS base
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

EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:$DOTNET_VERSION AS build
ARG BUILD_CONFIGURATION=Release
ARG TARGETARCH

WORKDIR /src
COPY . .
RUN dotnet restore "Portfolio.WebUi/Portfolio.WebUi.csproj" --arch $TARGETARCH
RUN dotnet build "Portfolio.WebUi/Portfolio.WebUi.csproj" --arch $TARGETARCH --configuration $BUILD_CONFIGURATION --output /app/build

FROM build AS publish
ARG TARGETARCH

RUN dotnet publish --arch $TARGETARCH "Portfolio.WebUi/Portfolio.WebUi.csproj" --configuration $BUILD_CONFIGURATION --output /app/publish /p:UseAppHost=false

FROM base AS final
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Portfolio.WebUi.dll"]