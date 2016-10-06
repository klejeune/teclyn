using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Teclyn.AspNetMvc.Commands.Renderers;

namespace Teclyn.AspNetMvc.Commands
{
    public class CommandPropertyRendererFactory
    {
        private class RendererInformation
        {
            public ICommandPropertyRenderer Renderer { get; }
            public Type RenderableType { get; }

            public bool CanRender(PropertyInfo property)
            {
                return this.RenderableType.IsAssignableFrom(property.PropertyType) && this.Renderer.CanRender(property);
            }

            public RendererInformation(Type renderableType, ICommandPropertyRenderer renderer)
            {
                this.RenderableType = renderableType;
                this.Renderer = renderer;
            }

            public override string ToString()
            {
                return this.RenderableType.Name + " - " + this.Renderer.GetType().Name;
            }
        }

        private readonly LinkedList<RendererInformation> renderers = new LinkedList<RendererInformation>();

        public void RegisterRenderer<TProperty>(ICommandPropertyRenderer<TProperty> renderer)
        {
            var info = new RendererInformation(typeof(TProperty), renderer);

            this.renderers.AddFirst(info);
        }

        public ICommandPropertyRenderer<TProperty> GetRenderer<TProperty>(PropertyInfo property)
        {
            var renderer =
                this.renderers.FirstOrDefault(
                    item =>
                        item.RenderableType.IsAssignableFrom(typeof(TProperty)) && item.CanRender(property));

            if (renderer == null)
            {
                throw new MvcCommandRenderingException($"No renderer could be found for property {property.DeclaringType.Name}.{property.Name} of type {property.PropertyType.Name}");
            }

            return (ICommandPropertyRenderer<TProperty>) renderer.Renderer;
        }
    }
}