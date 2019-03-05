namespace musiclover.core

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
        open System.Threading.Tasks
        open System
        open System.Net

        type ContentType = 
        | ApplicationJson

        type MusicHttpResponse = {
            Code:HttpStatusCode
            Content:string
        }

        type MusicHttpPostRequest = {
            Uri:Uri
            ContentType:ContentType
            Body:string
        }

        type IMusicHttpClient = 
            abstract member PostAsync: MusicHttpPostRequest -> (MusicHttpResponse -> 'a) -> Task<'a>
            abstract member GetAsync: Uri -> (MusicHttpResponse -> 'a) -> Task<'a>

    module dto =
        open providers
        open domain

        let toTracks text = 
            let toPlatform (deezer:option<DjamProvider.Deezer>) (spotify:option<DjamProvider.Spotify>) = 
                match deezer, spotify with
                | Some d, Some s -> All {DeezerId = DeezerId d.Id; SpotifyId = SpotifyId s.Id}
                | Some d, None -> Deezer (DeezerId d.Id)
                | None, Some s -> Spotify (SpotifyId s.Id)
                | None, None -> Empty

            let response = DjamProvider.Parse text
                        
            [for track in response.Tracks -> 
                { Title = track.Title;
                  Duration = track.Duration;
                  Artist = {Name = track.Artist}
                  Platform = (toPlatform track.Deezer track.Spotify)
                  }
            ]