using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Teclyn.Core.Domains;
using Teclyn.Core.Ioc;
using Teclyn.Core.Security.Context;

namespace Teclyn.Core.Commands
{
    public class CommandService
    {
        private ITeclynContext context;
        private TeclynApi teclyn;
        private IIocContainer iocContainer;

        public CommandService(ITeclynContext context, TeclynApi teclyn, IIocContainer iocContainer)
        {
            this.context = context;
            this.teclyn = teclyn;
            this.iocContainer = iocContainer;
        }

        public ICommandResult Execute<TCommand>(TCommand command) where TCommand : ICommand
        {
            return this.ExecuteInternal(command, command1 => string.Empty);
        }

        public ICommandResult<TResult> Execute<TCommand, TResult>(TCommand command)
            where TCommand : ICommand<TResult>
        {
            return this.ExecuteInternal(command, command1 => command1.Result);
        }

        public ICommandResult<TResult> ExecuteInternal<TCommand, TResult>(TCommand command,
            Func<TCommand, TResult> resultAccessor) where TCommand : ICommand
        {
            this.iocContainer.Inject(command);

            var result = new CommandExecutionResult<TResult>(teclyn);

            this.CheckContextInternal(command, result);
            this.CheckParametersInternal(command, result);

            if (!result.Errors.Any())
            {
                // execute
                command.Execute(result);

                // get result
                result.Result = resultAccessor(command);

                result.SetSuccess();
            }

            return result;
        }

        public CommandExecutionResult CheckContext(ICommand command)
        {
            var result = new CommandExecutionResult(this.teclyn);

            this.CheckContextInternal(command, result);

            return result;
        }

        public CommandExecutionResult CheckContextAndParameters(ICommand command)
        {
            var result = new CommandExecutionResult(this.teclyn);

            this.CheckContextInternal(command, result);
            this.CheckParametersInternal(command, result);

            return result;
        }

        private void CheckParametersInternal(ICommand command, CommandExecutionResult result)
        {
            result.ParametersAreValid = command.CheckParameters(result);
        }

        private void CheckContextInternal(ICommand command, CommandExecutionResult result)
        {
            result.ContextIsValid = command.CheckContext(context, result);
        }

        public bool IsRemote<TCommand>()
        {
            return IsRemote(typeof(TCommand));
        }

        public bool IsRemote(Type commandType)
        {
            return commandType.GetTypeInfo().GetCustomAttribute<RemoteAttribute>() != null;
        }

        public IDictionary<string, object> Serialize(ICommand command)
        {
            var properties = command.GetType().GetRuntimeProperties().Where(p => p.CanRead && p.CanWrite);

            return properties.ToDictionary(p => p.Name, p => p.GetValue(command));
        }
    }
}