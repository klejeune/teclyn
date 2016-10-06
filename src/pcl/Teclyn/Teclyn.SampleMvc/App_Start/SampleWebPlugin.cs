using Teclyn.AspNetMvc.Commands;
using Teclyn.Core;
using Teclyn.SampleMvc.Renderers;

namespace Teclyn.SampleMvc
{
    public class SampleWebPlugin : ITeclynPlugin
    {
        public string Name => "Sample MVC Web";
        public void Initialize(TeclynApi teclyn)
        {
            teclyn.Get<CommandPropertyRendererFactory>().RegisterRenderer(new TinyMceRenderer());
        }
    }
}