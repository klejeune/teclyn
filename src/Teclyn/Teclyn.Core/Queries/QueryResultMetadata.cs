using System;

namespace Teclyn.Core.Queries
{
    public class QueryResultMetadata<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        public long? Count { get; set; }
        public TQuery Next { get; set; }
        public TQuery Previous { get; set; }
        public TQuery Last { get; set; }
        public TQuery First { get; set; }

        public void SetPagination(long count, int currentPage, int perPage, Func<int, TQuery> queryFromPage)
        {
            if (perPage <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(perPage), $"The parameter {nameof(perPage)} should be at least 1.");
            }

            var lastPage = (int) Math.Ceiling(count / (double) perPage) + 1;

            this.Count = count;
            this.First = queryFromPage(1);
            this.Last = queryFromPage(lastPage);
            this.Previous = queryFromPage(Math.Max(1, currentPage - 1));
            this.Next = queryFromPage(Math.Min(lastPage, currentPage + 1));
        }
    }
}