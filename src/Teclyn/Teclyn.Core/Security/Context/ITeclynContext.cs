using System;

namespace Teclyn.Core.Security.Context
{
    public interface ITeclynContext
    {
        ITeclynUser CurrentUser { get; }

        IDisposable NewContext(ITeclynUser user);

        IDisposable TechnicalContext();
    }
}