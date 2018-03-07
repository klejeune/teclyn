using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Teclyn.Core.Api;
using Teclyn.Core.Ioc;

namespace Teclyn.Core.Commands
{
    public class CommandExecutionResult : ICommandContextChecker, IParameterChecker, ICommandExecutionContext, ICommandResult
    {
        private readonly List<CommandResultError> _errors = new List<CommandResultError>();

        public IEnumerable<CommandResultError> Errors => this._errors;

        public bool ContextIsValid { get; set; }
        public bool ParametersAreValid { get; set; }
        public bool Success { get; set; }

        public IDependencyResolver DependencyResolver { get; }

        public CommandExecutionResult(IDependencyResolver dependencyResolver)
        {
            this.DependencyResolver = dependencyResolver;
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
                this._errors.Add(new CommandResultError(message, property));
            }

            return expression;
        }

        private string GetFieldName<TProperty>(Expression<Func<TProperty>> property)
        {
            return ((MemberExpression) ((LambdaExpression) property).Body).Member.Name;
        }

        public void SetSuccess()
        {
            if (this._errors.Any())
            {
                throw new TeclynException("The command cannot be tagged as successful: errors occured.");
            }

            this.Success = true;
        }

        public void SetFailure(string message)
        {
            this._errors.Add(new CommandResultError(message));
        }
    }
}