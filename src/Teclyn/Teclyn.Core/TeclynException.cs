using System;

namespace Teclyn.Core
{
    public class TeclynException : Exception
    {
        public TeclynException(string message) : base(message)
        {
            
        }

        public TeclynException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}