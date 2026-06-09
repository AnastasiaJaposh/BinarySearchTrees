using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSTTest
{
    public static class TestInput
    {
        static Random rand = new(Seed: 0);
        public static IEnumerable<object[]> IntInput => new List<object[]>
        {
            new object[] { Enumerable.Range(0, 10000).Select(x => rand.Next()).ToArray() }, // random
            new object[] { Enumerable.Range(0, 10000).Select(x => x).ToArray() }, // sorted
            new object[] { Enumerable.Range(0, 10000).Select(x => rand.Next()).OrderBy(x => x / 10).ToArray() } // semi-sorted
        };
    }
}
