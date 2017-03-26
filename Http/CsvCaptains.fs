module CsvCaptains

open FSharp.Data

type CaptainData = CsvProvider<"captains.csv">

let getCaptains () = 
    CaptainData.GetSample().Rows
    |> Seq.map (fun r -> r.Affected,r.Skill,r.Value)
