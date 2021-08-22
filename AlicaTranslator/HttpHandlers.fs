namespace AlicaTranslator

module HttpHandlers =

    open Microsoft.AspNetCore.Http
    open FSharp.Control.Tasks
    open Giraffe
    open AlicaTranslator.Models
    open Newtonsoft.Json

    let fakeTokenHandler next ctx =
        task{
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

    let getDevices =
        task {
            let! file = readFileAsStringAsync "Devices.json"
            return JsonConvert.DeserializeObject<List<Device>> file
        }

    let getDevicesHandler next ctx =
        task {
            let devices = getDevices
            return! json devices next ctx
        }


