

dotnet --version
dotnet new console --name ConsoleApp

```
docker build --rm -f "ConsoleApp/Dockerfile" -t "netcore-preview:netcore3" .
```

```
docker run --rm netcore-preview:netcore3
```

## Links

- https://github.com/dotnet/core/blob/master/release-notes/5.0/preview/5.0.0-preview.2.md