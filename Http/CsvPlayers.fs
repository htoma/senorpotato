module CsvPlayers

open FSharp.Data

type PlayerData = CsvProvider<"players.csv">

let getPlayers () = 
    PlayerData.GetSample().Rows
    |> Seq.map (fun r -> r.Name,r.Nickname,r.Country)