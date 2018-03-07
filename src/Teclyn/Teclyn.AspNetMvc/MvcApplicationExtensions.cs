using System.Web;
using Teclyn.Core;
using Teclyn.Core.Api;

public static class MvcApplicationExtensions
{
    private static readonly string teclynApplicationKey = "teclyn";

    public static TeclynApi GetTeclyn(this HttpApplicationState application)
    {
        return (TeclynApi)HttpContext.Current.Application[teclynApplicationKey];
    }

    public static void SetTeclyn(TeclynApi teclyn)
    {
        HttpContext.Current.Application[teclynApplicationKey] = teclyn;
    }
}
