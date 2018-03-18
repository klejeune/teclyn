﻿using System.Threading.Tasks;
using Teclyn.Core.Events;
using Teclyn.Core.Services;
using Teclyn.Core.Tools;

namespace Teclyn.Core.Commands
{
    public static class CommandExtensions
    {
        public static IEventService GetEventService(this ICommandExecutionContext context)
        {
            return context.DependencyResolver.Get<IEventService>();
        }

        public static ITimeService GetTimeService(this ICommandExecutionContext context)
        {
            return context.DependencyResolver.Get<ITimeService>();
        }

        public static IdGenerator GetIdGenerator(this ICommandExecutionContext context)
        {
            return context.DependencyResolver.Get<IdGenerator>();
        }
        
        public static async Task<ICommandResult> Execute(this ICommand command, CommandService commandService)
        {
            return await commandService.Execute(command);
        }
    }
}