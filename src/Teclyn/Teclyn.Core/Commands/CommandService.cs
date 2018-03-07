using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Teclyn.Core.Api;
using Teclyn.Core.Domains;
using Teclyn.Core.Ioc;
using Teclyn.Core.Security.Context;
using Teclyn.Core.Tools;

namespace Teclyn.Core.Commands
{
    public class CommandService
    {
        private readonly ITeclynContext _context;
        private readonly IDependencyResolver _dependencyResolver;

        public CommandService(ITeclynContext context, IDependencyResolver dependencyResolver)
        {
            this._context = context;
            this._dependencyResolver = dependencyResolver;
        }

        public async Task<ICommandResult> Execute(ICommand command)
        {
            var method = ReflectionTools.Instance<CommandService>
#pragma warning disable 4014
                .Method(service => service.Execute<Dummies.DummyPropertyCommand>(null))
#pragma warning restore 4014
                .GetGenericMethodDefinition()
                .MakeGenericMethod(command.GetType());

            return await (Task<ICommandResult>) method.Invoke(this, new object[] {command});
        }

        public async Task<ICommandResult> Execute<TCommand>(TCommand command) where TCommand : ICommand
        {
            var commandHandler = this._dependencyResolver.Get<ICommandHandler<TCommand>>();
            var result = new CommandExecutionResult(this._dependencyResolver);

            var success = await commandHandler.CheckContext(command, this._context, result)
                && await commandHandler.CheckParameters(command, result)
                && await this.ExecuteCommandHandler(command, commandHandler, result);
            
            return result;
        }

        private async Task<bool> ExecuteCommandHandler<TCommand>(TCommand command, ICommandHandler<TCommand> commandHandler, CommandExecutionResult result) where TCommand : ICommand
        {
            try
            {
                await commandHandler.Execute(command, result);

                result.SetSuccess();

                return true;
            }
            catch (Exception exception)
            {
                result.SetFailure(exception.ToString());

                return false;
            }
        }
        
        public IDictionary<string, object> Serialize(ICommand command)
        {
            var properties = command.GetType().GetRuntimeProperties().Where(p => p.CanRead && p.CanWrite);

            return properties.ToDictionary(p => p.Name, p => p.GetValue(command));
        }
    }
}