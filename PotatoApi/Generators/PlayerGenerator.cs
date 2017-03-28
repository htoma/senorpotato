using System;
using System.Collections.Generic;
using System.Linq;
using Azure;
using PotatoApi.Models;

namespace PotatoApi.Generators
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
            return Captain.FromEntity(captains[rand.Next(0, captains.Count - 1)]);
        }
    }
}