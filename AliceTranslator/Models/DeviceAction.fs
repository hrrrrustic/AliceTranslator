namespace AliceTranslator.Models

module DeviceAction =
    module Response =
        open System

        type ActionResult = {
            Status : string
        }

        type State = {
            Instance : string
            ActionResult : ActionResult
        }
    
        type Capability = {
            Type : string
            State : State
        }

        type Device = {
            Id : string
            Capabilities : Capability list
        }

        type Payload = {
            Devices : Device list
        }

        type Response = {
            RequestId : string
            Payload : Payload
        }
        
        let getCapabilityResult = {Type = "devices.capabilities.on_off"; State = {Instance = "on"; ActionResult = {Status = "DONE"}}}
        let getDeviceResult() = {Id = "ee3cbaca-28bc-450f-a5d8-deb3d4653a97"; Capabilities = getCapabilityResult |> List.singleton}
        let getSuccessTurnOnResult() : Response = {
            RequestId = Guid.NewGuid().ToString()
            Payload = {
                Devices = getDeviceResult() |> List.singleton
                }
            }

        
    module Request =
        open DeviceState

        type Device = {
            Id : string
            Capabilities : CapabilityState list
        }

        type Payload = {
            Devices : Device list
        }

        type Request = {
            Payload : Payload
        }

