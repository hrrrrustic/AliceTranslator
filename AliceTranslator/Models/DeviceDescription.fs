namespace AliceTranslator.Models

module DeviceDescription =
    open System.IO
    open Newtonsoft.Json
    open System
    open DeviceShared

    type FakeAuthToken = {
        AccessToken : string
        TokenType : string
        ExpiresIn : int
        RefreshToken : string option
        }

    type Capability = {
        Type : CapabilityType
        Retrievable : bool
        Reportable : bool
        }

    type Device = {
        Id : string
        Name : string
        Description : string
        Room : string
        Type : string
        Capabilities : Capability list
        }

    type Payload = {
        UserId: string
        Devices: Device list
        }

    type Response = {
        RequestId : string
        Payload : Payload
        }

    let Devices =
        let file = File.ReadAllText "Devices.json"
        JsonConvert.DeserializeObject<List<Device>> file
        
    let getDevicesResponse() = {
        RequestId = Guid.NewGuid().ToString()
        Payload = {
            UserId = "bdc2f6b3-3c30-4e5b-b601-a817a35b9ac1"
            Devices = Devices
            }
        }
     