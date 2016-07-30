using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Teclyn.Core.Commands
{
    public class CommandExecutionResult : ICommandContextChecker, IParameterChecker, ICommandExecutionContext
    {
        private List<CommandResultError> errors = new List<CommandResultError>();

        public IEnumerable<CommandResultError> Errors
        {
            get { return this.errors; }
        }

        public bool ContextIsValid { get; set; }
        public bool ParametersAreValid { get; set; }
        public bool Success { get; set; }

        public TeclynApi Teclyn { get; }

        public CommandExecutionResult(TeclynApi teclyn)
        {
            this.Teclyn = teclyn;
        }

        public bool Check(bool expression, string message)
        {
            return this.Check(string.Empty, expression, message);
        }

        public bool Check<TProperty>(Expression<Func<TProperty>> property, Predicate<TProperty> expression, string message)
        {
            return this.Check(this.GetFieldName(property), expression(property.Compile()()), message);
        }

        public bool Check(string property, bool expression, string message)
        {
            if (!expression)
            {
                this.errors.Add(new CommandResultError(message, property));
            }

            return expression;
        }

        private string GetFieldName<TProperty>(Expression<Func<TProperty>> property)
        {
            return ((MemberExpression) ((LambdaExpression) property).Body).Member.Name;
        }

        public void SetSuccess()
        {
            if (this.errors.Any())
            {
                throw new TeclynException("The command cannot be tagged as successful: errors occured.");
            }

            this.Success = true;
        }
    }

    public class CommandExecutionResult<TResult> : CommandExecutionResult, ICommandResult<TResult>
    {
        public TResult Result { get; set; }

        public CommandExecutionResult(TeclynApi teclyn) : base(teclyn)
        {
        }

        public IUserFriendlyCommandResult ToUserFriendly()
        {
            return new UserFriendlyCommandResult<TResult>(this);
        }
    }
}