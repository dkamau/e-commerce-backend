#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/ECommerceBackend.Web/ECommerceBackend.Web.csproj", "src/ECommerceBackend.Web/"]
COPY ["src/ECommerceBackend.Infrastructure/ECommerceBackend.Infrastructure.csproj", "src/ECommerceBackend.Infrastructure/"]
COPY ["src/ECommerceBackend.SharedKernel/ECommerceBackend.SharedKernel.csproj", "src/ECommerceBackend.SharedKernel/"]
COPY ["src/ECommerceBackend.Core/ECommerceBackend.Core.csproj", "src/ECommerceBackend.Core/"]
RUN dotnet restore "src/ECommerceBackend.Web/ECommerceBackend.Web.csproj"
COPY . .
WORKDIR "/src/src/ECommerceBackend.Web"
RUN dotnet build "ECommerceBackend.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ECommerceBackend.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "ECommerceBackend.Web.dll"]
CMD ASPNETCORE_URLS=http://*:$PORT dotnet ECommerceBackend.Web.dll