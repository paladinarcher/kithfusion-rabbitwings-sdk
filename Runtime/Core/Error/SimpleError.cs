using System;
using System.Text;

namespace RabbitWings.Core
{
    [Serializable]
    public class SimpleError
    {
        public string message;

        public override string ToString()
        {
            var builder = new StringBuilder($"Error: {message}");
            return builder.ToString();
        }
    }
}