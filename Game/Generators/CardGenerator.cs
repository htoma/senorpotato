using System.Collections.Generic;
using Game.Cards;
using Game.Utils;

namespace Game.Generators
{
    public static class CardGenerator
    {
        public static IEnumerable<ActionCard> GenerateActionCards(int count)
        {
            const int attackDuration = 12; //minutes
            const int setPiecesDuration = 5; //minutes

            return new[]
            {
                new ActionCard(ActionCard.EActionCardType.Attack, attackDuration),
                new ActionCard(ActionCard.EActionCardType.Attack, attackDuration),
                new ActionCard(ActionCard.EActionCardType.Attack, attackDuration),
                new ActionCard(
                    (ActionCard.EActionCardType)
                    Seeder.Random()
                        .Next((int) ActionCard.EActionCardType.Penalty, (int) ActionCard.EActionCardType.Freekick + 1),
                    setPiecesDuration)
            };
        }
    }
}