# ChatApp
A sample chat application using SignalR

# How to run?

1. [Download](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) and install the .NET 6 SDK. 
2. Clone this repository:
```
$ git clone https://github.com/suxrobGM/ChatApp.git
$ cd ChatApp/src
```
3. Run the server:
```
$ dotnet run --project Chat.Server
```
4. Run the Blazor web client:
```
$ dotnet run --project Chat.Client.BlazorApp
```

By default, the server runs on localhost https://127.0.0.1:7281 and the client app runs on https://127.0.0.1:7212