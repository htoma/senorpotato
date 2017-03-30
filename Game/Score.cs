using System;

namespace Game
{
    public class Score
    {
        public Score()
        {
            First = 0;
            Second = 0;
        }

        public void GoalScored(ETurn turn)
        {
            switch (turn)
            {
                case ETurn.First:
                    First += 1;
                    break;
                case ETurn.Second:
                    Second += 1;
                    break;
                default:
                    throw new ArgumentException("Invalid player turn");
            }
        }

        public int First { get; set; }
        public int Second { get; set; }
    }
}