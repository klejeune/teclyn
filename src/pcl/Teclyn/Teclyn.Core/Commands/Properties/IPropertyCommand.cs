using Teclyn.Core.Domains;

namespace Teclyn.Core.Commands.Properties
{
    public interface IPropertyCommand : ICommand
    {
        
    }

    public interface IPropertyCommand<TAggregate, TProperty> : IPropertyCommand where TAggregate : IAggregate
    {

    }
}