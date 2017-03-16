#load "Player.fs"
open SenorPotato

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

