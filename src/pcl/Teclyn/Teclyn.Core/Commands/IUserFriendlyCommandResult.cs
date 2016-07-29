namespace Teclyn.Core.Commands
{
    public interface IUserFriendlyCommandResult
    {
        bool Success { get; }

        CommandResultError[] Errors { get; }
    }
}