using System;
using System.Collections.Generic;
using System.Linq;
using Azure;
using PotatoApi.Models;

namespace PotatoApi.Generators
{
    public class CaptainGenerator
    {
        public static IEnumerable<Captain> Generate(int count, Random rand)
        {
            return Enumerable.Range(1, count).Select(x =>
            {
                return new Captain();
            }).ToList();
        }
    }
}