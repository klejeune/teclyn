using System.Collections.Generic;
using Xunit;

namespace Teclyn.Core.Tests.Tools
{
    public class EnumerableExtensionsTests
    {
        [Fact]
        public void GetValueOrDefaultTest()
        {
            var dictionary = new Dictionary<int, string>
            {
                {56, "test1" },
                {98, "test2" },
                {296, "test3" },
            };

            var defaultValue = "default value";

            var result = EnumerableExtensions.GetValueOrDefault(dictionary, 0, defaultValue);

            Assert.Equal(defaultValue, result);
        }
    }
}