FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Utal.Icc.Mm.Mvc/Utal.Icc.Mm.Mvc.csproj", "Utal.Icc.Mm.Mvc/"]
RUN dotnet restore "Utal.Icc.Mm.Mvc/Utal.Icc.Mm.Mvc.csproj"
COPY . .
WORKDIR "/src/Utal.Icc.Mm.Mvc"
RUN dotnet build "Utal.Icc.Mm.Mvc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Utal.Icc.Mm.Mvc.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Utal.Icc.Mm.Mvc.dll"]