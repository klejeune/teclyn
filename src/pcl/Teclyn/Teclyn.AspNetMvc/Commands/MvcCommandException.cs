using System;

namespace Teclyn.AspNetMvc.Commands
{
    public class MvcCommandException : Exception
    {
        public MvcCommandException(string message) : base(message)
        {
            
        }
    }
}