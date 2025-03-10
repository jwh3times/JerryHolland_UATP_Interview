# Use the official .NET image as a build environment
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.sln .
COPY RapidPay/*.csproj ./RapidPay/
COPY Tests/*.csproj ./Tests/
RUN dotnet restore

# Copy everything else and build
COPY RapidPay/. ./RapidPay/
WORKDIR /app/RapidPay
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 5000
EXPOSE 5001
COPY --from=build /app/RapidPay/out ./
COPY RapidPay/RapidPay.pfx /app/RapidPay.pfx
ENTRYPOINT ["dotnet", "RapidPay.dll"]