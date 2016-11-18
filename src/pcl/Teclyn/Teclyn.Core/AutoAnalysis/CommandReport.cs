using System.Reflection;

namespace Teclyn.Core.AutoAnalysis
{
    public class CommandReport
    {
        public string Type { get; }

        public CommandReport(TypeInfo type)
        {
            this.Type = type.Name;
        }
    }
}