FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Feedback.Presentation/Feedback.Presentation.csproj", "Feedback.Presentation/"]
RUN dotnet restore "Feedback.Presentation/Feedback.Presentation.csproj"
COPY . .
WORKDIR "/src/Feedback.Presentation"
RUN dotnet build "Feedback.Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Feedback.Presentation.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Feedback.Presentation.dll"]
