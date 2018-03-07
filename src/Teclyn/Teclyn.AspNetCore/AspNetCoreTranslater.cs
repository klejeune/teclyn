using System.Text.RegularExpressions;
using Teclyn.Core.Api;

namespace Teclyn.AspNetCore
{
    public class AspNetCoreTranslater
    {
        public string ExportCommandId(CommandInfo commandInfo)
        {
            return this.Hyphenate(commandInfo.Id);
        }

        public string ExportDomainId(DomainInfo domainInfo)
        {
            return this.Hyphenate(domainInfo.Id);
        }

        public string ExportQueryId(QueryInfo queryInfo)
        {
            return this.Hyphenate(queryInfo.Id);
        }

        public string ImportCommandId(string commandId)
        {
            return this.DeHyphenate(commandId);
        }

        public string ImportDomainId(string domainId)
        {
            return this.DeHyphenate(domainId);
        }

        public string ImportQueryId(string queryId)
        {
            return this.DeHyphenate(queryId);
        }

        private string Hyphenate(string camlCaseString)
        {
            return Regex.Replace(camlCaseString, @"([a-z])([A-Z])", "$1-$2").ToLower();
        }
        
        private string DeHyphenate(string value)
        {
            if (value.Length >= 1)
            {
                value = char.ToUpper(value[0]) + value.Substring(1);
            }

            return Regex.Replace(value, @"(.)-(.)", m => $"{m.Groups[1]}{m.Groups[2].Value.ToUpperInvariant()}");
        }
    }
}