FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim AS base
RUN apt-get update && apt-get install -y libgdiplus
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim AS build
WORKDIR /src
COPY ["src/User.Management.Application/User.Management.Application.csproj", "User.Management.Application/"]
COPY ["src/User.Management.Data/User.Management.Data.csproj", "User.Management.Data/"]
COPY ["src/User.Management.Service/User.Management.Service.csproj", "User.Management.Service/"]
COPY ["src/User.Management.ViewModel/User.Management.ViewModel.csproj", "User.Management.ViewModel/"]
RUN dotnet restore "User.Management.Application/User.Management.Application.csproj"
COPY ./src .

WORKDIR "/src/User.Management.Application"
RUN dotnet build "User.Management.Application.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "User.Management.Application.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=build /src/User.Management.Application/wwwroot ./wwwroot
ENTRYPOINT ["dotnet", "User.Management.Application.dll"]

