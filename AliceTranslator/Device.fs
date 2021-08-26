namespace AlicaTranslator.Models

module DeviceTypes =
    type FakeAuthToken = {
        AccessToken : string
        TokenType : string
        ExpiresIn : int
        RefreshToken : string option
        }

    type CapabilityType = string

    type State = {
        Instance : string
        Value : string
    }

    type CapabilityDescription = {
         Retrievable : bool
         Reportable : bool
         CapabilityType : CapabilityType
    }

    type CapabilityState = {
         CapabilityType : CapabilityType
         State : State
        }

    type Capabality = 
        | CapabilityState of CapabilityState
        | CapabilityDescription of CapabilityDescription

    type Device = {
        Id : string
        Name : string
        Description : string
        Room : string
        Type : string
        }

     type Payload = Device list

     type Response = {
        RequestId : string
        Payload : Payload
     }
     let createOnOffCapabilityState (value: bool) = 
        CapabilityState { CapabilityType = "devices.capabilities.on_off"; State = {Instance = "on"; Value = value.ToString()}}