namespace Teclyn.Core.Security.Context
{
    public interface ITeclynUser
    {
        string Id { get; }
        string Name { get; }

        bool IsAdmin { get; }
        bool IsGuest { get; }
    }
}