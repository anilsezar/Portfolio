FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY . .

# Build the application
RUN dotnet publish "Portfolio.WebUi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage: Use the runtime image to create a self-contained deployment
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
EXPOSE 443

# Set the entry point
ENTRYPOINT ["dotnet", "Portfolio.WebUi.dll"]
