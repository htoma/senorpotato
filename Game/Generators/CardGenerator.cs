using System.Collections.Generic;
using Game.Cards;
using Game.Utils;

namespace Game.Generators
{
    public static class CardGenerator
    {
        public static IEnumerable<ActionCard> GenerateActionCards(int count)
        {
            return new[]
            {
                new ActionCard(ActionCard.EActionCardType.Attack), new ActionCard(ActionCard.EActionCardType.Attack),
                new ActionCard(ActionCard.EActionCardType.Attack),
                new ActionCard(
                    (ActionCard.EActionCardType)
                    Seeder.Random()
                        .Next((int) ActionCard.EActionCardType.Freekick, (int) ActionCard.EActionCardType.Penalty + 1))
            };
        }
    }
}