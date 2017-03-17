#load "Player.fs"
#load "Team.fs"
#load "CardPlay.fs"
#load "CardSpecial.fs"

open System

open Player
open Team
open CardPlay
open CardSpecial

//player and team
let captainAttack (skills: Skills) =
    {skills with Attack=skills.Attack+2}

let captainDefending (skills: Skills) =
    {skills with Defence=skills.Defence+2}

let attackingPlayer = { Name="Senor Potato"
                        Description="amazing player"
                        Overall=32
                        Skills={ Defence=3
                                 Attack=6
                                 Penalty=5
                                 Stamina=6 }
                        CaptainBonus=captainAttack
                        IsCaptain=false }

let defensivePlayer = { Name="Big defender"
                        Description="Stays strong"
                        Overall=27
                        Skills={ Defence=7
                                 Attack=2
                                 Penalty=2
                                 Stamina=6 }
                        CaptainBonus=captainDefending
                        IsCaptain=false }

let myTeam = { Name="All Stars"
               Players=[{attackingPlayer with IsCaptain=true}]
               Substitutions=[defensivePlayer] }

let yourTeam = { Name="Awesome"
                 Players=[{defensivePlayer with IsCaptain=true}]
                 Substitutions=[attackingPlayer] }

//play cards
let scoreGoal (mine:Team,yours:Team) = 
    mine.Attack-yours.Defence>3

let counterattack (mine:Team,yours:Team) =
    yours.Defence-mine.Attack>5

let attackingEffect (mine:Team,yours:Team) = 
    if yours.Defence-mine.Attack>5 then {mine with Players=mine.Players|>List.tail}
    else mine

let defendingEffect (mine:Team,yours:Team) = 
    yours

let cardPlay = { Timespan=TimeSpan.FromMinutes(3.)
                 DefendingLimit=4
                 AttackingLimit=3
                 ScoringTrigger=scoreGoal
                 CounterattackTrigger=counterattack
                 AttackingEffect=attackingEffect
                 DefendingEffect=defendingEffect }

if cardPlay.ScoringTrigger (myTeam,yourTeam) then printfn "I scored" else printfn "I missed"
if cardPlay.CounterattackTrigger (myTeam,yourTeam) then printfn "Goal on the counter" else printfn "Can't counter me"

//special cards
let fatigueOpponent (mine:Team,yours:Team) =
    let strongDef = mine.Players 
                    |> List.tryFind (fun p -> p.Skills.Defence>5)
    match strongDef with
    | Some p -> 
                let yourPlayers = yours.Players 
                                    |> List.map (fun x -> 
                                                        {x with Skills={x.Skills with Stamina=x.Skills.Stamina-1}})
                mine,{yours with Players=yourPlayers}
    | _ -> mine,yours

let cardFatigue = InflictFatigue fatigueOpponent

