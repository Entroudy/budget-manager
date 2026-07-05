#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/Backend/BudgetManager/BudgetManager.Presentation/BudgetManager.csproj", "src/Backend/BudgetManager/BudgetManager.Presentation/"]
RUN dotnet restore "src/Backend/BudgetManager/BudgetManager.Presentation/BudgetManager.csproj"
COPY . .
WORKDIR "/src/src/Backend/BudgetManager/BudgetManager.Presentation"
RUN dotnet build "BudgetManager.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BudgetManager.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BudgetManager.dll"]
