module AliceTranslator.App

open System
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Giraffe
open AliceTranslator.HttpHandlers
open Newtonsoft.Json
open Newtonsoft.Json.Serialization
// ---------------------------------
// Web app
// ---------------------------------

let webApp =
    choose [
        subRoute "/auth" (
            choose [
                GET >=> fakeAuthHandler
                subRoute "/token" (
                    choose [
                        POST >=> fakeTokenHandler
                    ]
                )
            ]
        )
        subRoute "/v1.0" (
            choose [
                subRoute "/user" (
                    choose [
                        subRoute "/devices" (
                            choose [
                                GET >=> getDevicesHandler
                                subRoute "/query" (
                                    choose [
                                        POST >=> queryDevicesHandler
                                    ]
                                )
                                subRoute "/action" (
                                    choose [
                                        POST >=> actionDeviceHandler
                                    ]
                                )
                            ]
                        )
                        subRoute "/unlink" (
                            choose [
                                POST >=> setStatusCode 200
                            ]
                        )
                    ]
                )
                HEAD >=> setStatusCode 200
            ]
        )
 
        setStatusCode 404 >=> text "Not Found" ]

// ---------------------------------
// Error handler
// ---------------------------------

let errorHandler (ex : Exception) (logger : ILogger) =
    logger.LogError(ex, "An unhandled exception has occurred while executing the request.")
    clearResponse >=> setStatusCode 500 >=> text ex.Message

// ---------------------------------
// Config and Main
// ---------------------------------

let configureCors (builder : CorsPolicyBuilder) =
    builder
        .WithOrigins(
            "http://localhost:5000",
            "https://localhost:5001")
       .AllowAnyMethod()
       .AllowAnyHeader()
       |> ignore

let configureApp (app : IApplicationBuilder) =
    let env = app.ApplicationServices.GetService<IWebHostEnvironment>()
    (match env.IsDevelopment() with
    | true  ->
        app.UseDeveloperExceptionPage()
    | false ->
        app .UseGiraffeErrorHandler(errorHandler)
            .UseHttpsRedirection())
        .UseCors(configureCors)
        .UseGiraffe(webApp)

let configureServices (services : IServiceCollection) =
    services.AddCors()    |> ignore
    services.AddGiraffe() |> ignore
    let jsonSettings = new JsonSerializerSettings(ContractResolver = new DefaultContractResolver(NamingStrategy = SnakeCaseNamingStrategy()))
    services.AddSingleton<Json.ISerializer>(NewtonsoftJson.Serializer(jsonSettings)) |> ignore

let configureLogging (builder : ILoggingBuilder) =
    builder.AddConsole()
           .AddDebug() |> ignore

[<EntryPoint>]
let main args =
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(
            fun webHostBuilder ->
                webHostBuilder
                    .Configure(Action<IApplicationBuilder> configureApp)
                    .ConfigureServices(configureServices)
                    .ConfigureLogging(configureLogging)
                    |> ignore)
        .Build()
        .Run()
    0