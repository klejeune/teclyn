using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using Teclyn.Core.Commands;

namespace Teclyn.AspNetMvc.Commands
{
    public sealed class CommandButton : IDisposable
    {
        private readonly TextWriter _writer;

        public CommandButton(TextWriter writer, string serializedCommand, bool reload = false, string @class = "", object htmlAttributes = null)
        {
            this._writer = writer;
            
            var classes = string.IsNullOrWhiteSpace(@class) ? "command-button" : "command-button " + @class;

            writer.Write($"<button class='{classes}' data-teclyn-command='{serializedCommand}'>");
        }


        public void Dispose()
        {
            _writer.Write("</button>");
        }
    }
}