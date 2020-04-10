FROM mcr.microsoft.com/dotnet/core/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:5.0 AS build
WORKDIR /src
COPY ["samples/webapi/AdventureWorks.Api", "./webapi/AdventureWorks.Api"]
COPY ["samples/efcore5/AdventureWorks.Data", "./efcore5/AdventureWorks.Data"] 

RUN dotnet restore "webapi/AdventureWorks.Api/AdventureWorks.Api.csproj"
COPY . .
WORKDIR "/src/webapi/AdventureWorks.Api"

RUN dotnet build "AdventureWorks.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AdventureWorks.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AdventureWorks.Api.dll"]

# docker build --rm -f "samples/webapi/AdventureWorks.Api/Dockerfile" -t adventure-works-api:latest "samples"