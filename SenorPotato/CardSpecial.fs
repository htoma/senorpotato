module CardSpecial

open Player
open Team

type CardSpecialType =
    | InflictFatigue of (Team*Team->Team*Team) //opposition loses stamina based on condition on players
    | ForceSubstition of (Team*Team->Team*Team) //force substitution move on opponent based on condition on players
    | Swap of (Team*Team->Team*Team) //swap card with opponents
