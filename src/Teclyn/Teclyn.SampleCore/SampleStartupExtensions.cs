using Microsoft.Extensions.DependencyInjection;
using Teclyn.Core.Api;

namespace Teclyn.SampleCore
{
    public static class SampleStartupExtensions
    {
        public static void AddSample(this IServiceCollection services, ITeclynApiConfiguration configuration)
        {
            services.AddTeclyn(new TeclynApi(configuration));
        }

    }
}