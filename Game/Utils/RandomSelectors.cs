using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Utils
{
    public static class RandomSelectors
    {
        public static IEnumerable<T> Shuffle<T>(Random rand, IEnumerable<T> data)
        {
            var result = data.ToList();
            for (int i = 0; i < result.Count; i++)
            {
                var pos = rand.Next(i, result.Count);
                var temp = result[pos];
                result[pos] = result[i];
                result[i] = temp;
            }
            return result;
        }
    }
}
