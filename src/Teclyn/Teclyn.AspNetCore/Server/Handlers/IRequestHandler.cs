using Microsoft.AspNetCore.Http;

namespace Teclyn.AspNetCore.Server.Handlers
{
    public interface IRequestHandler
    {
        string GetTemplate();
        RequestDelegate GetRequestDelegate();
        string GetVerb();
    }
}