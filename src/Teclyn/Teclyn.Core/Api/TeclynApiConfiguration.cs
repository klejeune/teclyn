namespace Teclyn.Core.Api
{
    public class TeclynApiConfiguration : ITeclynApiConfiguration
    {
        public string CommandEndpointPrefix { get; set; }

        public TeclynApiConfiguration()
        {
            this.CommandEndpointPrefix = "teclyn";
        }
    }
}