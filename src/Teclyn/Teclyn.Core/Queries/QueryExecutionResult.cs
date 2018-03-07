using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Teclyn.Core.Api;
using Teclyn.Core.Commands;
using Teclyn.Core.Ioc;

namespace Teclyn.Core.Queries
{
    public class QueryExecutionResult<TQuery, TResult>: IQueryContextChecker, IParameterChecker, IQueryExecutionContext<TQuery, TResult>, IQueryResult<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        public IDependencyResolver DependencyResolver { get; }
        public QueryResultMetadata<TQuery, TResult> Metadata { get; }

        private readonly List<QueryResultError> _errors = new List<QueryResultError>();

        public IEnumerable<QueryResultError> Errors => this._errors;

        public bool ContextIsValid { get; set; }
        public bool ParametersAreValid { get; set; }

        public object GetResult()
        {
            return this.Result;
        }

        public bool Success { get; set; }

        public TResult Result { get; set; }
        
        public QueryExecutionResult(IDependencyResolver dependencyResolver)
        {
            this.DependencyResolver = dependencyResolver;
            this.Metadata = new QueryResultMetadata<TQuery, TResult>();
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
                this._errors.Add(new QueryResultError(message, property));
            }

            return expression;
        }

        private string GetFieldName<TProperty>(Expression<Func<TProperty>> property)
        {
            return ((MemberExpression)((LambdaExpression)property).Body).Member.Name;
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
            this._errors.Add(new QueryResultError(message));
        }
    }
}