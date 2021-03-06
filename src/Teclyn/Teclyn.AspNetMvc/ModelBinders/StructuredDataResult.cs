﻿using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Teclyn.AspNetMvc.ModelBinders
{
    public class StructuredDataResult<T> : ActionResult
    {
        public T Data { get; }
        private bool indent;

        public StructuredDataResult(T data, bool indent)
        {
            this.Data = data;
            this.indent = indent;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            new ContentResult
            {
                ContentType = "application/json",
                Content = JsonConvert.SerializeObject(this.Data, indent ? Formatting.Indented : Formatting.None),
                ContentEncoding = Encoding.Default,
            }.ExecuteResult(context);
        }
    }
}