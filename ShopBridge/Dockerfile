#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 1433
RUN curl -sL https://deb.nodesource.com/setup_12.x |  bash -
RUN apt-get install -y nodejs

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
RUN curl -sL https://deb.nodesource.com/setup_12.x |  bash -
RUN apt-get install -y nodejs
WORKDIR /src
RUN npm version
COPY ["ShopBridge/ShopBridge.csproj", "ShopBridge/"]
COPY ["ShopBridge.Contracts/ShopBridge.Contracts.csproj", "ShopBridge.Contracts/"]
COPY ["Shopbridge.Database/Shopbridge.Database.csproj", "Shopbridge.Database/"]
COPY ["Shopbridge.Models/Shopbridge.Models.csproj", "Shopbridge.Models/"]
COPY ["ShopBridge.Application/ShopBridge.Application.csproj", "ShopBridge.Application/"]
RUN dotnet restore "ShopBridge/ShopBridge.csproj"
COPY . .
WORKDIR "/src/ShopBridge"
RUN dotnet build "ShopBridge.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "ShopBridge.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShopBridge.dll"]