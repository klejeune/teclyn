using Teclyn.Core.Commands;

namespace Teclyn.Core.Queries
{
    public interface IUserFriendlyQueryResult
    {
        bool Success { get; }

        QueryResultError[] Errors { get; }
    }
}