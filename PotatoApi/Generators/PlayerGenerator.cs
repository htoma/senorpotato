using System;
using System.Collections.Generic;
using System.Linq;
using Azure;
using PotatoApi.Models;

namespace PotatoApi.Generators
{
    public static class PlayerGenerator
    {
        public static IEnumerable<Player> Generate(int count, Random rand, IList<PlayerEntity> players)
        {
            return Enumerable.Range(1, count).Select(x =>
            {
                var player = players.Count > 0
                    ? players[rand.Next(0, players.Count - 1)]
                    : new PlayerEntity("Senor Potato", string.Empty, "Singapore");

                var skills = GenerateSkills(rand);
                return new Player
                {
                    Name = player.Name,
                    Description = player.Description,
                    Country = player.Country,
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