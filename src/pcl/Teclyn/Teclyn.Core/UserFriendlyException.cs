using System;

namespace Teclyn.Core
{
    public class UserFriendlyException : Exception
    {
        public UserFriendlyException(string message) : base(message)
        {
            
        }
    }
}