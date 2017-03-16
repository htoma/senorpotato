namespace SenorPotato

type Skills = { Defence: int
                Attack: int
                Penalty: int
                Stamina: int }

type Player = { Name: string
                Description: string
                Overall: int
                Skills: Skills
                Captain: Skills->Skills }
