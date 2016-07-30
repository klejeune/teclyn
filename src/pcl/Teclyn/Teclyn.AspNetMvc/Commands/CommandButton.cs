using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using Teclyn.Core.Commands;

namespace Teclyn.AspNetMvc.Commands
{
    public class CommandButton : IDisposable
    {
        private TextWriter writer;

        public CommandButton(TextWriter writer, string serializedCommand, bool reload = false, string @class = "", object htmlAttributes = null)
        {
            this.writer = writer;
            
            var classes = string.IsNullOrWhiteSpace(@class) ? "command-button" : "command-button " + @class;

            writer.Write($"<button class='{classes}' data-teclyn-command='{serializedCommand}'>");
        }


        public void Dispose()
        {
            writer.Write("</button>");
        }
    }
}