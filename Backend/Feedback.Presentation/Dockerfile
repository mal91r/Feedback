FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Feedback.Presentation/Feedback.Presentation.csproj", "Feedback.Presentation/"]
ENV NUGET_PACKAGES=/root/.nuget/packages
RUN mkdir -p /root/.nuget/packages
RUN dotnet restore "Feedback.Presentation/Feedback.Presentation.csproj"
COPY . .
WORKDIR "/src/Feedback.Presentation"
RUN dotnet build "Feedback.Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Feedback.Presentation.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Feedback.Presentation.dll"]
