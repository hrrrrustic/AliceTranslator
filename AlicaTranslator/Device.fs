namespace AlicaTranslator.Models

type FakeAuthToken = {
    AccessToken : string
    TokenType : string
    ExpiresIn : int
    RefreshToken : string option
    }

type State = {
    Instance : string
    Value : string
}

type Capabality = {
    Type : string
    Retrievable : bool
    Reportable : bool
    State : State
}

type Device = {
    Id : string
    Name : string
    Description : string
    Room : string
    Type : string
    }

 type Payload = {
    UserId : string
    Devices : Device list
 }