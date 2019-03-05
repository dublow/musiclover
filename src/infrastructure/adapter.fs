namespace musiclover.adapter

    module http = 
        open System.Net.Http
        open FSharp.Control.Tasks
        open musiclover.core.``interface``
        open System.Text

        type MusicHttpClient =
            member private this.ToMusicHttpResponseAsync(httpResponseMessage:HttpResponseMessage) = 
                task {
                    let! content = httpResponseMessage.Content.ReadAsStringAsync()
                    return { Code = httpResponseMessage.StatusCode; Content = content }
                }

            member private this.ToHttpContent contentType body = 
                let toMediaType contentType = 
                    match contentType with
                    | ApplicationJson -> "application/json"
                
                new StringContent(body, Encoding.UTF8, toMediaType contentType)

            interface IMusicHttpClient with
                member this.PostAsync request map = 
                    task {
                        use httpClient = new HttpClient()

                        use body = this.ToHttpContent request.ContentType request.Body
                        let! httpResponse = httpClient.PostAsync(request.Uri, body)
                        let! response = this.ToMusicHttpResponseAsync(httpResponse)

                        return map(response)
                    }
                member this.GetAsync uri map = 
                    task{
                        use httpClient = new HttpClient()

                        let! httpResponse = httpClient.GetAsync(uri)
                        let! response = this.ToMusicHttpResponseAsync(httpResponse)

                        return map(response)
                    }
