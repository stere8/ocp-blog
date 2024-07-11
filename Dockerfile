# Use the official .NET image as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ocp_blog.csproj", "./"]
RUN dotnet restore "./ocp_blog.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "ocp_blog.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ocp_blog.csproj" -c Release -o /app/publish

# Copy the build app to the base image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ocp_blog.dll"]