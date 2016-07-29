using System.Reflection;

namespace Teclyn.Core.Events.Injection
{
    public interface IEventInjector
    {
        bool AppliesToProperty(PropertyInfo property);
        void Inject(ITeclynEvent @event, PropertyInfo property);
    }
}