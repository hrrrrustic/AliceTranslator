namespace AlicaTranslator

module HttpHandlers =

    open Microsoft.AspNetCore.Http
    open FSharp.Control.Tasks
    open Giraffe
    open AlicaTranslator.Models.DeviceTypes
    open Newtonsoft.Json
    open DeviceDiscovery.Models
    open Nanoleaf.Client
    open Nanoleaf.Client.Discovery
    open System.Linq

    let fakeTokenHandler next ctx =
        task {
            let response = {
                TokenType = "random string"; 
                ExpiresIn = System.Int32.MaxValue; 
                RefreshToken = Some "random string"; 
                AccessToken = "random string"
            }
            return! json response next ctx
        }

    let fakeAuthHandler next (ctx : HttpContext) =
        task {
            let query = ctx.Request.Query
            let redirect = query.["redirect_url"].[0]
            let state = query.["state"].[0]
            let redirectUrl = $"{redirect}?code=randomCode&state={state}"
            return! redirectTo true redirectUrl next ctx
        }

    let getDevices() =
        async {
            let! file = readFileAsStringAsync "Devices.json" |> Async.AwaitTask
            return JsonConvert.DeserializeObject<List<Device>> file 
        }

    let getDevicesHandler next ctx =
        task {
            let devices = getDevices()
            return! json devices next ctx
        }

    let nanoLeafClient = new NanoleafClient("192.168.1.108", "fgzg8imTMDJiHJKDcv3BHYQ42IJaXequ")

    let queryDevicesHandler next ctx = 
        task {
            let! powerStatus = nanoLeafClient.GetPowerStatusAsync() |> Async.AwaitTask
            return! json "ok" next ctx
        }