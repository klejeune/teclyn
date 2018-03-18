﻿using System.Text;
using Newtonsoft.Json;

namespace Teclyn.AspNetMvc.Commands
{
    public class JsonSerializer
    {
        public string Serialize(object @object)
        {
            return JsonConvert.SerializeObject(@object);
        }

        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public object Deserialize(string json)
        {
            return JsonConvert.DeserializeObject(json);
        }
        
        private const string INDENT_STRING = "    ";
        public static string FormatJson(string str)
        {
            var indent = 0;
            var quoted = false;
            var sb = new StringBuilder();
            for (var i = 0; i < str.Length; i++)
            {
                var ch = str[i];
                switch (ch)
                {
                    case '{':
                    case '[':
                        sb.Append(ch);
                        if (!quoted)
                        {
                            sb.AppendLine();
                            indent++;

                            for (int indentIndex = 0; indentIndex < indent; indentIndex++)
                            {
                                sb.Append(INDENT_STRING);
                            }
                        }
                        break;
                    case '}':
                    case ']':
                        if (!quoted)
                        {
                            sb.AppendLine();
                            indent--;

                            for (int indentIndex = 0; indentIndex < indent; indentIndex++)
                            {
                                sb.Append(INDENT_STRING);
                            }
                        }
                        sb.Append(ch);
                        break;
                    case '"':
                        sb.Append(ch);
                        bool escaped = false;
                        var index = i;
                        while (index > 0 && str[--index] == '\\')
                            escaped = !escaped;
                        if (!escaped)
                            quoted = !quoted;
                        break;
                    case ',':
                        sb.Append(ch);
                        if (!quoted)
                        {
                            sb.AppendLine();

                            for (int indentIndex = 0; indentIndex < indent; indentIndex++)
                            {
                                sb.Append(INDENT_STRING);
                            }
                        }
                        break;
                    case ':':
                        sb.Append(ch);
                        if (!quoted)
                            sb.Append(" ");
                        break;
                    default:
                        sb.Append(ch);
                        break;
                }
            }
            return sb.ToString();
        }
    }
}