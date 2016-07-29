using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Teclyn.Core.Tests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var dictionary = new Dictionary<int, string>
            {
                {56, "test1" },
                {98, "test2" },
                {296, "test3" },
            };

            var result = EnumerableExtensions.GetValueOrDefault(dictionary, 0);

            Console.WriteLine(result == null);
        }
    }
}
