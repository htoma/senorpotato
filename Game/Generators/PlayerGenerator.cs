using System;
using System.Collections.Generic;
using System.Linq;
using Azure;

namespace Game.Generators
{
    public static class PlayerGenerator
    {
        public static IEnumerable<Player> Generate(Random rand, IList<PlayerEntity> players, IList<CaptainEntity> captains)
        {
            return players.Select(x =>
            {
                var skills = GenerateSkills(rand);
                return new Player
                {
                    Name = x.Name,
                    Description = x.Description,
                    Country = x.Country,
                    Skills = skills,
                    Overall = skills.Attack + skills.Defence + skills.Penalty,
                    Captain = GenerateCaptain(rand, captains)
                };
            }).ToList();
        }

        public static Captain FromEntity(CaptainEntity entity)
        {
            return new Captain
            {
                Affected = (Captain.EAffected)Enum.Parse(typeof(Captain.EAffected), entity.Affected),
                Skill = (ESkill)Enum.Parse(typeof(ESkill), entity.Skill),
                Value = entity.Value
            };
        }

        private static Skills GenerateSkills(Random rand)
        {
            return new Skills
            {
                Attack = GenerateSkill(rand),
                Defence = GenerateSkill(rand),
                Penalty = GenerateSkill(rand),
                Stamina = GenerateSkill(rand)
            };
        }

        private static int GenerateSkill(Random rand)
        {
            return rand.Next(1, 5);
        }

        private static Captain GenerateCaptain(Random rand, IList<CaptainEntity> captains)
        {
            if (captains.Count == 0)
            {
                throw new ArgumentException("Need at least one captain");
            }
            return FromEntity(captains[rand.Next(0, captains.Count - 1)]);
        }
    }
}