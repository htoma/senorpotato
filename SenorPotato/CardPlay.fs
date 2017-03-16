module CardPlay

open System
open Player

type CardPlay = { Timespan: TimeSpan
                  DefendingLimit: int //max # defenders
                  AttackingLimit: int  //max # attackers
                  ScoringTrigger: Player list->Player list->bool //goal scored by attacker: Attacking players->Defending players
                  CounterattackTrigger: Player list->Player list->bool //goal scored on the counterattack: same order of players 
                  AttackingEffect: Player list->Player list->Player list //returns new list of attackers
                  DefendingEffect: Player list->Player list->Player list //returns new list of defenders
                }
