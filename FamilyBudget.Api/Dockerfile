# Set the base image as the ASP.NET Core runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Set the SDK for the build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["FamilyBudget.Api.csproj", "./"]
RUN dotnet restore "FamilyBudget.Api.csproj"

# Copy everything else and build
COPY . .
RUN dotnet build "FamilyBudget.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FamilyBudget.Api.csproj" -c Release -o /app/publish

# Build runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FamilyBudget.Api.dll"]
