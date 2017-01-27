using System;
using System.Threading.Tasks;
using Teclyn.Core.Commands;
using Teclyn.Core.Security;
using Teclyn.Core.Security.Context;
using Xunit;

namespace Teclyn.Core.Tests.Commands
{
    public class CommandServiceTests
    {
        private CommandService commandService;
        private readonly TeclynApi teclyn;

        private class DummyCommand : ICommand<string>
        {
            public static string FirstErrorMessage => "The first parameter must not be empty";

            public string FirstParameter { get; set; }
            public int SecondParameter { get; set; }

            public bool CheckParameters(IParameterChecker _)
            {
                return _.Check(() => this.FirstParameter, p => !string.IsNullOrWhiteSpace(p), FirstErrorMessage)
                       && _.Check(() => this.SecondParameter, p => p >= 0, FirstErrorMessage);
            }

            public bool CheckContext(ITeclynContext context, ICommandContextChecker _)
            {
                return true;
            }

            public Task Execute(ICommandExecutionContext context)
            {
                this.Result = this.FirstParameter + this.SecondParameter;

                return Task.FromResult(Type.Missing);
            }

            public string Result { get; private set; }
        }

        public CommandServiceTests()
        {
            this.teclyn = new TeclynApi(new TeclynTestConfiguration());
            this.commandService = teclyn.Get<CommandService>();
        }

        [Fact]
        public async Task ObjectIsCreated()
        {
            var first = "test";
            var second = 5;

            var command = this.commandService.Create<DummyCommand>(c =>
            {
                c.FirstParameter = first;
                c.SecondParameter = second;
            });

            var result = await command.Execute(this.commandService);

            Assert.Equal(first + second, result.Result);
        }
    }
}