﻿using System.Threading.Tasks;
using Teclyn.Core.Commands;
using Teclyn.Core.Events;
using Teclyn.Core.Security.Context;
using Teclyn.Sample.Blog.Core.Posts.Events;

namespace Teclyn.Sample.Blog.Core.Posts.Commands
{
    public class UpdatePostText : ICommand
    {
        public string PostId { get; set; }
        public string Text { get; set; }

        public bool CheckParameters(IParameterChecker _)
        {
            return true;
        }

        public bool CheckContext(ITeclynContext context, ICommandContextChecker _)
        {
            return true;
        }

        public async Task Execute(ICommandExecutionContext context)
        {
            await context.GetEventService().Raise(new TextUpdated
            {
                AggregateId = this.PostId,
                Text = this.Text,
            });
        }
    }
}