using System;
using System.Collections.Generic;
using System.Linq;
using Azure;
using PotatoApi.Models;

namespace PotatoApi.Generators
{
    public static class PlayerGenerator
    {
        public static IEnumerable<Player> Generate(Random rand, IList<PlayerEntity> players)
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
                    Overall = skills.Attack + skills.Defence + skills.Penalty
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
    }
}