using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Teclyn.Core.Domains;
using Teclyn.Core.Ioc;
using Teclyn.Core.Metadata;
using Teclyn.Core.Security.Context;

namespace Teclyn.Core.Commands
{
    public class CommandService
    {
        private readonly ITeclynContext context;
        private readonly TeclynApi teclyn;
        private readonly IIocContainer iocContainer;
        private readonly MetadataRepository metadataRepository;

        public CommandService(ITeclynContext context, TeclynApi teclyn, IIocContainer iocContainer, MetadataRepository metadataRepository)
        {
            this.context = context;
            this.teclyn = teclyn;
            this.iocContainer = iocContainer;
            this.metadataRepository = metadataRepository;
        }

        public async Task<ICommandResult> Execute(ICommand command)
        {
            return await this.ExecuteInternal(command, command1 => string.Empty);
        }

        public async Task<ICommandResult<TResult>> Execute<TResult>(ICommand<TResult> command)
        {
            return await this.ExecuteInternal(command, command1 => command1.Result);
        }

        public async Task<ICommandResult> Execute<TCommand>(Action<TCommand> builder) where TCommand : ICommand
        {
            var command = this.Create(builder);
            return await this.Execute(command);
        }

        public async Task<ICommandResult<TResult>> Execute<TCommand, TResult>(Action<TCommand> builder) where TCommand : ICommand<TResult>
        {
            var command = this.Create(builder);
            return await this.Execute(command);
        }

        public async Task<IUserFriendlyCommandResult> ExecuteGeneric(IBaseCommand command)
        {
            var result = await this.ExecuteInternal(command, command1 => string.Empty);
            return result.ToUserFriendly();
        }

        public TCommand Create<TCommand>() where TCommand : IBaseCommand
        {
            return this.iocContainer.Get<TCommand>();
        }

        public TCommand Create<TCommand>(Action<TCommand> builder) where TCommand : IBaseCommand
        {
            var command = this.Create<TCommand>();

            if (builder != null)
            {
                builder(command);
            }

            return command;
        }

        public async Task<ICommandResult<TResult>> ExecuteInternal<TCommand, TResult>(TCommand command,
            Func<TCommand, TResult> resultAccessor) where TCommand : IBaseCommand
        {
            var result = new CommandExecutionResult<TResult>(teclyn);

            this.CheckContextInternal(command, result);
            this.CheckParametersInternal(command, result);

            if (!result.Errors.Any())
            {
                // execute
                await command.Execute(result);

                // get result
                result.Result = resultAccessor(command);

                result.SetSuccess();
            }

            return result;
        }

        public CommandExecutionResult CheckContext(IBaseCommand command)
        {
            var result = new CommandExecutionResult(this.teclyn);

            this.CheckContextInternal(command, result);

            return result;
        }

        public CommandExecutionResult CheckContextAndParameters(IBaseCommand command)
        {
            var result = new CommandExecutionResult(this.teclyn);

            this.CheckContextInternal(command, result);
            this.CheckParametersInternal(command, result);

            return result;
        }

        private void CheckParametersInternal(IBaseCommand command, CommandExecutionResult result)
        {
            result.ParametersAreValid = command.CheckParameters(result);
        }

        private void CheckContextInternal(IBaseCommand command, CommandExecutionResult result)
        {
            result.ContextIsValid = command.CheckContext(context, result);
        }
        
        public IDictionary<string, object> Serialize(IBaseCommand command)
        {
            var properties = command.GetType().GetRuntimeProperties().Where(p => p.CanRead && p.CanWrite);

            return properties.ToDictionary(p => p.Name, p => p.GetValue(command));
        }
        
        public void RegisterCommand(Type commandType)
        {
            if (!typeof(IBaseCommand).GetTypeInfo().IsAssignableFrom(commandType.GetTypeInfo()))
            {
                throw new TeclynException($"Unable to register type {commandType.Name}: it is not a command type. (It doesn't implement ICommand.)");
            }

            this.metadataRepository.RegisterCommand(new CommandInfo(commandType.Name.ToLowerInvariant(), commandType.Name, commandType));
        }
    }
}