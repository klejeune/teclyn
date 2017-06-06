using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Teclyn.Core.Commands;

namespace Teclyn.Core.Queries
{
    public class QueryExecutionResult: IQueryContextChecker, IParameterChecker, IQueryExecutionContext
    {
        private readonly List<QueryResultError> errors = new List<QueryResultError>();

        public IEnumerable<QueryResultError> Errors => this.errors;

        public bool ContextIsValid { get; set; }
        public bool ParametersAreValid { get; set; }
        public bool Success { get; set; }

        public TeclynApi Teclyn { get; }

        public QueryExecutionResult(TeclynApi teclyn)
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
                this.errors.Add(new QueryResultError(message, property));
            }

            return expression;
        }

        private string GetFieldName<TProperty>(Expression<Func<TProperty>> property)
        {
            return ((MemberExpression)((LambdaExpression)property).Body).Member.Name;
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

    public class QueryExecutionResult<TResult> : QueryExecutionResult, IQueryResult<TResult>
    {
        public TResult Result { get; set; }

        public QueryExecutionResult(TeclynApi teclyn) : base(teclyn)
        {
        }

        public IUserFriendlyQueryResult ToUserFriendly()
        {
            return new UserFriendlyQueryResult<TResult>(this);
        }
    }
}