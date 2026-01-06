# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/CoursePlatform.API/CoursePlatform.API.csproj", "src/CoursePlatform.API/"]
COPY ["src/CoursePlatform.Core/CoursePlatform.Core.csproj", "src/CoursePlatform.Core/"]
COPY ["src/CoursePlatform.Infrastructure/CoursePlatform.Infrastructure.csproj", "src/CoursePlatform.Infrastructure/"]
RUN dotnet restore "src/CoursePlatform.API/CoursePlatform.API.csproj"
COPY . .
WORKDIR "/src/src/CoursePlatform.API"
RUN dotnet build "CoursePlatform.API.csproj" -c Release -o /app/build

# Publish Stage
FROM build AS publish
RUN dotnet publish "CoursePlatform.API.csproj" -c Release -o /app/publish

# Final Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Expose port (8080 is default for ASP.NET Core 8 images)
EXPOSE 8080
ENTRYPOINT ["dotnet", "CoursePlatform.API.dll"]
