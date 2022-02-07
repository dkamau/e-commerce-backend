#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/BiziBuddyBackend.Web/BiziBuddyBackend.Web.csproj", "src/BiziBuddyBackend.Web/"]
COPY ["src/BiziBuddyBackend.Infrastructure/BiziBuddyBackend.Infrastructure.csproj", "src/BiziBuddyBackend.Infrastructure/"]
COPY ["src/BiziBuddyBackend.SharedKernel/BiziBuddyBackend.SharedKernel.csproj", "src/BiziBuddyBackend.SharedKernel/"]
COPY ["src/BiziBuddyBackend.Core/BiziBuddyBackend.Core.csproj", "src/BiziBuddyBackend.Core/"]
RUN dotnet restore "src/BiziBuddyBackend.Web/BiziBuddyBackend.Web.csproj"
COPY . .
WORKDIR "/src/src/BiziBuddyBackend.Web"
RUN dotnet build "BiziBuddyBackend.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BiziBuddyBackend.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "BiziBuddyBackend.Web.dll"]
CMD ASPNETCORE_URLS=http://*:$PORT dotnet BiziBuddyBackend.Web.dll