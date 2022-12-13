FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim AS base
RUN apt-get update && apt-get install -y libgdiplus
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim AS build
WORKDIR /src
COPY ["src/Parking.Management.Application/Parking.Management.Application.csproj", "Parking.Management.Application/"]
COPY ["src/Parking.Management.Data/Parking.Management.Data.csproj", "Parking.Management.Data/"]
COPY ["src/Parking.Management.Service/Parking.Management.Service.csproj", "Parking.Management.Service/"]
COPY ["src/Parking.Management.ViewModel/Parking.Management.ViewModel.csproj", "Parking.Management.ViewModel/"]
RUN dotnet restore "Parking.Management.Application/Parking.Management.Application.csproj"
COPY ./src .

WORKDIR "/src/Parking.Management.Application"
RUN dotnet build "Parking.Management.Application.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Parking.Management.Application.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Parking.Management.Application.dll"]

