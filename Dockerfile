FROM mcr.microsoft.com/dotnet/aspnet:7.0.0 AS base
WORKDIR /app
EXPOSE 8000

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /Source/AllSopFoodService
COPY ["Source/AllSopFoodService/AllSopFoodService.csproj", "Source/AllSopFoodService/"]
RUN dotnet restore "Source/AllSopFoodService/AllSopFoodService.csproj"
COPY . .
WORKDIR "/Source/AllSopFoodService"
RUN dotnet build "Source/AllSopFoodService/AllSopFoodService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Source/AllSopFoodService/AllSopFoodService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet AllSopFoodService.dll
