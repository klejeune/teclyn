namespace Teclyn.Core.Security.Context
{
    public class GuestTeclynUser : ITeclynUser
    {
        public string Id => "@@guest@@";
        public string Name => "Guest";
        public bool IsAdmin => false;
        public bool IsGuest => true;
    }
}