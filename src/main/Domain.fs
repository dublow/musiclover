namespace musiclover

module providers = 
    open FSharp.Data

    type DjamProvider = JsonProvider<"Documents\Jsons\djam.json">

module domain = 
    open System

    type Radio = {
        Name:string
        Uri:Uri
    }

    type Artist = {
        Name:string
    }

    type Music = {
        Title:string
        Artist:Artist
    }