
# C# Sandbox

[Twitter: @stvansolano](https://twitter.com/stvansolano)

Repo: https://github.com/stvansolano/csharp-sandbox/

## Do you like it? Give a Star! :star:

If you like or are using this project to learn or start your own solution, please give it a star. I appreciate it!

## Getting started:

    git clone https://github.com/stvansolano/csharp-sandbox.git
    cd csharp-sandbox

## Creating a test console 

```
dotnet --version
dotnet new console --name ConsoleApp
```

No .NET installed? Got Docker? Run it as a remote container!

1) Install [VS Code](https://code.visualstudio.com/) or [VS Code Insiders] and the [Remote Containers extension](https://marketplace.visualstudio.com/items?itemName=ms-vscode-remote.remote-containers).
2) Install and configure Docker for your operating system.
3) Open the cloned project with VSCode and switch to the remote container!

Alternatively, if you want to run .NET in Docker

    cd `templates/ConsoleApp`
    docker build --rm -f "ConsoleApp/Dockerfile" -t "netcore-preview:netcore3" .

and

    docker run --rm netcore-preview:netcore3

## What's new in C# 8?

English: [samples/CSharp8-Examples/What-is-new-in-CSharp8.en.md](https://docs.microsoft.com/en-US/dotnet/csharp/whats-new/csharp-8)

Spanish: [samples/CSharp8-Examples/What-is-new-in-CSharp8.es.md](./samples/CSharp8-Examples/What-is-new-in-CSharp8.es.md)

## Recommended (before coding)

    export PATH="$PATH:/root/.dotnet/tools"

## Scaffolding a project:

    dotnet new mvc --name AspnetCore`
    dotnet run

## Get some extra templates/tooling:

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

## Useful development links:

- https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-3.1&tabs=netcore-cli
- https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-add-reference

## Other Links

- https://try.dot.net/
- https://github.com/dotnet/core/blob/master/release-notes/5.0/preview/5.0.0-preview.2.md
