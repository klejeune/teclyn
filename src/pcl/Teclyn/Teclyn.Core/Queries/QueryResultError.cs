namespace Teclyn.Core.Queries
{
    public class QueryResultError
    {
        public string Message { get; }
        public string Field { get; }

        public QueryResultError(string message)
        {
            this.Message = message;
        }

        public QueryResultError(string message, string field)
        {
            this.Message = message;
            this.Field = field;
        }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(this.Field))
            {
                return $"{this.Field} - {this.Message}";
            }

            return this.Message;
        }
    }
}