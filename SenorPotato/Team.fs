module Team

open Player

type Team = { Name: string
              Players: Player list }
              with member this.Overall = 
                            this.Players |> List.sumBy (fun p -> p.Overall)
