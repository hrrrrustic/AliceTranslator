namespace AliceTranslator.NanoLeaf

module NanoLeafService =
    open Nanoleaf.Client
    open FSharp.Control.Tasks
    open AliceTranslator.Models.DeviceState

    let nanoLeafClient = new NanoleafClient("192.168.1.108", "fgzg8imTMDJiHJKDcv3BHYQ42IJaXequ")

    let getNanoLeafWithOnlyPower (client : NanoleafClient) = 
        async {
            let! power = client.GetPowerStatusAsync() |> Async.AwaitTask
            return getReposnse power
            }

    let setNanoLeafPower (client : NanoleafClient) request =
        task {
            return 
                match request with
                | "true" -> client.TurnOnAsync()
                | _ -> client.TurnOffAsync()
        }