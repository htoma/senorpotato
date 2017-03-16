#load "Player.fs"
#load "Team.fs"
#load "CardPlay.fs"

open System

open Player
open Team
open CardPlay

let skills = { Defence=3
               Attack=6
               Penalty=5
               Stamina=6 }

let captainAttack (skills: Skills) =
    {skills with Attack=skills.Attack+2}

let player = { Name="Senor Potato"
               Description="amazing player"
               Overall=32
               Skills=skills
               Captain=captainAttack }

let team = { Name="All Stars"
             Players=[player] }

team.Overall

let scoreGoal (attackers:Player list) (defenders:Player list) = 
    let attack = attackers
                 |> List.sumBy (fun p -> p.Overall)
    let defence = defenders
                 |> List.sumBy (fun p -> p.Overall)
    attack-defence>3

let counterattack (attackers:Player list) (defenders:Player list) = 
    let attack = attackers
                 |> List.sumBy (fun p -> p.Overall)
    let defence = defenders
                 |> List.sumBy (fun p -> p.Overall)
    defence-attack>5

let attackingEffect (attackers:Player list) (defenders:Player list) = 
    let attack = attackers
                 |> List.sumBy (fun p -> p.Overall)
    let defence = defenders
                 |> List.sumBy (fun p -> p.Overall)
    if defence-attack>5 then attackers |> List.tail else attackers

let defendingEffect (attackers:Player list) (defenders:Player list) =
    defenders

let cardPlay = { Timespan=TimeSpan.FromMinutes(3.)
                 DefendingLimit=4
                 AttackingLimit=3
                 ScoringTrigger=scoreGoal
                 CounterattackTrigger=counterattack
                 AttackingEffect=attackingEffect
                 DefendingEffect=defendingEffect }
