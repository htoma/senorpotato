module CardPlay

open System
open Player
open Team

type CardPlay = { Timespan: TimeSpan
                  DefendingLimit: int //max # defenders
                  AttackingLimit: int  //max # attackers
                  ScoringTrigger: Team*Team->bool //goal scored by attacker: Attacking players->Defending players
                  CounterattackTrigger: Team*Team->bool //goal scored on the counterattack: same order of players 
                  AttackingEffect: Team*Team->Team //how attacking team is affected
                  DefendingEffect: Team*Team->Team //how defending team is affected
                }
