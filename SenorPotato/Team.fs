module Team

open Player

type Team = { Name: string
              Players: Player list
              Substitutions: Player list }
              with member this.Attack = 
                            this.Players |> List.sumBy (fun p -> p.CompleteSkills.Attack)
                   member this.Defence = 
                            this.Players |> List.sumBy (fun p -> p.CompleteSkills.Defence)
