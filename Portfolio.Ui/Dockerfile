# From root:
# docker build -f Portfolio.Grpc/Dockerfile -t portfolio-grpc .
# https://devblogs.microsoft.com/dotnet/improving-multiplatform-container-support/
ARG DOTNET_VERSION=9.0
ARG TARGETARCH
ARG BUILD_CONFIGURATION=Release

FROM mcr.microsoft.com/dotnet/aspnet:$DOTNET_VERSION AS base
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

# USER $APP_UID # todo: https://devblogs.microsoft.com/dotnet/securing-containers-with-rootless/
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:$DOTNET_VERSION AS build

WORKDIR /src
COPY ["protos/", "protos/"]
COPY ["Portfolio.Ui/Portfolio.Ui.csproj", "Portfolio.Ui/"]

RUN dotnet restore "Portfolio.Ui/Portfolio.Ui.csproj" --arch $TARGETARCH
# todo: this copy . . should be deleted
COPY . .
WORKDIR "/src/Portfolio.Ui"
RUN dotnet build "Portfolio.Ui.csproj" --arch $TARGETARCH --configuration $BUILD_CONFIGURATION --output /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Portfolio.Ui.csproj" --configuration $BUILD_CONFIGURATION --output /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Portfolio.Ui.dll"]
