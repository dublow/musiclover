namespace musiclover

module providers = 
    open FSharp.Data

    type DjamProvider = JsonProvider<"documents\jsons\djam.json">

module domain = 
    open System

    type DeezerId = DeezerId of int
    type SpotifyId = SpotifyId of string

    type AllPlatform = {
        DeezerId:DeezerId
        SpotifyId:SpotifyId
    }

    type Platform = 
    | Empty
    | Deezer of DeezerId
    | Spotify of SpotifyId
    | All of AllPlatform

    type Radio = {
        Name:string
        Uri:Uri
    }

    type Artist = {
        Name:string
    }

    type Track = {
        Title:string
        Duration: int
        Artist:Artist
        Platform:Platform
    }

module ``interface`` = 
    open System.Net.Http
    open System.Threading.Tasks

    type PostRequest = {
        Uri:string
        HttpContent:HttpContent
    }

    type IMusicHttpClient = 
        abstract member PostAsync: PostRequest -> (HttpResponseMessage -> 'a) -> Task<'a>