using System;

namespace PotatoApi.Generators
{
    public static class Seeder
    {
        private static readonly Random Rand = new Random((int) DateTime.UtcNow.Ticks);

        public static Random Random()
        {
            return Rand;
        }
    }
}