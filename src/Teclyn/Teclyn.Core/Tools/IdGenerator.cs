using System;

namespace Teclyn.Core.Tools
{
    public class IdGenerator
    {
        public string GenerateId(int length = 32)
        {
            return Guid.NewGuid().ToString("N").Substring(0, length);
        }
    }
}