using System.Collections.Generic;
using System.Linq;
using Game.Cards;
using Game.Utils;

namespace Game.Generators
{
    public static class CardGenerator
    {
        public static IEnumerable<ActionCard> GenerateActionCards(int count)
        {
            return Enumerable.Range(1, count).Select(x => new ActionCard(
                (ActionCard.EActionCardType)
                Seeder.Random()
                    .Next((int) ActionCard.EActionCardType.Penalty, (int) ActionCard.EActionCardType.Max)));
        }
    }
}