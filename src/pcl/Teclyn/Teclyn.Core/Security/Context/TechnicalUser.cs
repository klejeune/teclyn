namespace Teclyn.Core.Security.Context
{
    public class TechnicalUser : ITeclynUser
    {
        public string Id => "@@technical@@";
        public string Name => "Technical";
        public bool IsAdmin => true;
        public bool IsGuest => false;
    }
}