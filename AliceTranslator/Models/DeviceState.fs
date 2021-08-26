namespace AliceTranslator.Models

module DeviceState =
    open DeviceShared
    open DeviceDescription
    open System

    type State = {
        Instance : string
        Value : string
    }

    type CapabilityState = {
         CapabilityType : CapabilityType
         State : State
    }

    type Device = {
        Id : string
        Capabilities : CapabilityState list
    }

    type Payload = {
        Devices : Device list
    }

    type Response = {
        RequestId : string
        Payload : Payload
    }

    let createOnOffCapabilityState (value: bool) = 
         { CapabilityType = "devices.capabilities.on_off"; State = {Instance = "on"; Value = value.ToString() } }
    
    let nanoLeafId = (Devices |> Seq.filter (fun x -> x.Name = "NanoLeaf Canvas") |> Seq.head).Id

    let getNanoLeafStateResponse powerStatus : Device = {Id = nanoLeafId; Capabilities = createOnOffCapabilityState powerStatus |> List.singleton }

    let getReposnse powerStatus = {RequestId = Guid.NewGuid().ToString(); Payload = { Devices = getNanoLeafStateResponse powerStatus |> List.singleton }}