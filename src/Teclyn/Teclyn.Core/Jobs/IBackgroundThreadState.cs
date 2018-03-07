using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teclyn.Core.Jobs
{
    public interface IBackgroundThreadState
    {
        bool MustStop { get; }
    }
}
