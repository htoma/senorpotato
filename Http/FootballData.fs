module FootballData

open FSharp.Data
open System.Configuration

type Request =
    { Url: string 
      Headers: seq<string*string>
      Query: seq<string*string> }

let get (req: Request) =
    let headers = HttpRequestHeaders.Accept HttpContentTypes.Json :: (req.Headers |> List.ofSeq)
    Http.AsyncRequestString(req.Url, List.ofSeq req.Query, headers) |> Async.RunSynchronously

type TeamData = JsonProvider<"footballdata.json">

let url team = sprintf "http://api.football-data.org/v1/teams/%d/players" team

let getTeamPlayers (team: int) = 
    let request = { Url = url team
                    Headers = ["X-Auth-Token", ConfigurationManager.AppSettings.["FootballDataToken"]]
                    Query = [] }
    let response = get request
    let players = TeamData.Parse(response).Players
                  |> Seq.map (fun p -> p.Name.Split([|'\n'|]).[0],p.Nationality)
    players

let getPlayers () =
    [1..20]
    |> Seq.map getTeamPlayers
    |> Seq.concat
