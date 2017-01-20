namespace Adacola.RussianDinner

[<AutoOpen>]
module Model =
    open FSharp.Data

    type ConsumerKeyProvider = JsonProvider<"consumerKeySample.json">
    type ConsumerKey = ConsumerKeyProvider.Root
    type AccessTokenProvider = JsonProvider<"accessTokenSample.json">
    type AccessToken = AccessTokenProvider.Root
    type TweetID = TweetID of int64
    
    type Meal = {
        Name : string
    }