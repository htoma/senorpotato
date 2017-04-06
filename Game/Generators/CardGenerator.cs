using System.Collections.Generic;
using Game.Cards;
using Game.Utils;

namespace Game.Generators
{
    public static class CardGenerator
    {
        public static IEnumerable<ActionCard> GenerateActionCards(int count)
        {
            const int equalDuration = 12;

            return new[]
            {
                new ActionCard(ActionCard.EActionCardType.Attack, equalDuration),
                new ActionCard(ActionCard.EActionCardType.Attack, equalDuration),
                new ActionCard(ActionCard.EActionCardType.Attack, equalDuration),
                new ActionCard(
                    (ActionCard.EActionCardType)
                    Seeder.Random()
                        .Next((int) ActionCard.EActionCardType.Freekick, (int) ActionCard.EActionCardType.Penalty + 1),
                    equalDuration)
            };
        }
    }
}