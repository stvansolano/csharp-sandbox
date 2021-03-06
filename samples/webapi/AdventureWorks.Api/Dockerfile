FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["samples/webapi/AdventureWorks.Api", "./webapi/AdventureWorks.Api"]
COPY ["samples/efcore3x/AdventureWorks.Data", "./efcore3x/AdventureWorks.Data"] 

RUN dotnet restore "webapi/AdventureWorks.Api/AdventureWorks.Api.csproj"
COPY . .
WORKDIR "/src/webapi/AdventureWorks.Api"

RUN dotnet build "AdventureWorks.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AdventureWorks.Api.csproj" -c Release -o /app/publish --framework netcoreapp3.1

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AdventureWorks.Api.dll"]

# docker build --pull --rm -f "samples/webapi/AdventureWorks.Api/Dockerfile" -t adventure-works-api:latest .
# az acr build --image samples/mywebapi-netcore3x:latest --registry myDemos --file samples/webapi/AdventureWorks.Api/Dockerfile "."
# ./ngrok http 5000 --host-header=localhost:5000