using System.Threading.Tasks;
using Teclyn.Core.Commands;
using Teclyn.Core.Security.Context;

namespace Teclyn.Sample.Blog.Core.Users.Commands
{
    public class RegisterUserCommand : ICommand
    {
        public string Username { get; set; }
        public string EmailAddress { get; set; }
    }
}