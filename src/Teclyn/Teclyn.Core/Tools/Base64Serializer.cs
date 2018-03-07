using System;
using System.Text;

namespace Teclyn.Core.Tools
{
    public class Base64Serializer
    {
        public string Serialize(string data)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
        }

        public string Deserialize(string base64)
        {
            var bytes = Convert.FromBase64String(base64);

            return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }
    }
}