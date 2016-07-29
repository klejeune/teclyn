using System.Xml;

namespace Teclyn.Core.Storage
{
    public interface IDisplayable : IIndexable
    {
        string Name { get; }
    }
}