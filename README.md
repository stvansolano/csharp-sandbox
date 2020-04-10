

```
dotnet --version
dotnet new console --name ConsoleApp
```
Then 
```
docker build --rm -f "ConsoleApp/Dockerfile" -t "netcore-preview:netcore3" .
```
and
```
docker run --rm netcore-preview:netcore3
```

## Recommended (before start)

    export PATH="$PATH:/root/.dotnet/tools"

## Scaffolding:

`dotnet new mvc --name AspnetCore`
`dotnet watch run`

## Extra templates:

### Blazor Web Assembly!
[Install Blazor Web Assembly Template](https://www.nuget.org/packages/Microsoft.AspNetCore.Components.WebAssembly.Templates/)

or 

    dotnet new --install Microsoft.AspNetCore.Components.WebAssembly.Templates::3.2.0-preview3.20168.3

### EF Core 5.0 preview

    dotnet tool uninstall --global dotnet-ef

    dotnet tool install --global dotnet-ef --version 5.0.0-preview.2.20159.4

Then

    dotnet-ef --help


Ready! 🎉🦄
```
                 _/\__       
           ---==/    \\      
     ___  ___   |.    \|\    
    | __|| __|  |  )   \\\   
    | _| | _|   \_/ |  //|\\ 
    |___||_|       /   \\\/\\
```

Add EF Core 5.x to a `classlib`

    dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 5.0.0-preview.2.20159.4

```
dotnet ef dbcontext scaffold "<CONNECTION_STRING_HERE>" Microsoft.EntityFrameworkCore.SqlServer -o Models
```

## Other Links

- https://try.dot.net/
- https://github.com/dotnet/core/blob/master/release-notes/5.0/preview/5.0.0-preview.2.md
