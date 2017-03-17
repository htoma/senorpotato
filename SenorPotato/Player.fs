module Player

type Skills = { Defence: int
                Attack: int
                Penalty: int
                Stamina: int }

type Player = { Name: string
                Description: string
                Overall: int
                Skills: Skills
                CaptainBonus: Skills->Skills
                IsCaptain: bool }
                with member this.CompleteSkills = if this.IsCaptain then this.CaptainBonus this.Skills else this.Skills
