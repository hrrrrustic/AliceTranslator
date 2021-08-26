namespace AliceTranslator

module HttpHandlers =

    open Microsoft.AspNetCore.Http
    open FSharp.Control.Tasks
    open Giraffe
    open AliceTranslator.Models.DeviceDescription
    open AliceTranslator.NanoLeaf.NanoLeafService
    open AliceTranslator.Models.DeviceAction

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

    let getDevicesHandler next ctx =
        task {
            let response = getDevicesResponse()
            return! json response next ctx
        }

    let queryDevicesHandler next ctx =
        task {
            let! powerStatus = getNanoLeafWithOnlyPower nanoLeafClient
            return! json powerStatus next ctx
        }

    let actionDeviceHandler next (ctx: HttpContext) =
        task {
            let! req = ctx.BindJsonAsync<Request.Request>()
            let onOff = ((req.Payload.Devices |> Seq.head).Capabilities |> Seq.head).State.Value
            setNanoLeafPower nanoLeafClient onOff |> Async.AwaitTask |> ignore
            let response = Response.getSuccessTurnOnResult()
            return! json response next ctx
        }
