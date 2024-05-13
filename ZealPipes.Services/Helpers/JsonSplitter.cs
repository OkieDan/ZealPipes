using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

namespace ZealPipes.Services.Helpers
{
    internal class JsonSplitter
    {
        private StringBuilder _buffer;

        public JsonSplitter()
        {
            _buffer = new StringBuilder();
        }

        public IEnumerable<string> SplitJson(string data)
        {
            _buffer.Append(data);

            // Split the accumulated data by '}' followed by one or more whitespace characters and '{'
            string[] jsons = Regex.Split(_buffer.ToString(), @"(?<=\})\s*(?=\{)");

            // If the last element of jsons ends with '{', it's an incomplete JSON object
            // Append it to the buffer to wait for the completion in the next read
            if (jsons[jsons.Length - 1].EndsWith("{"))
            {
                _buffer.Clear();
                _buffer.Append(jsons[jsons.Length - 1]);
            }
            else
            {
                _buffer.Clear();
            }

            // Deserialize each JSON object and yield return it
            foreach (string json in jsons)
            {
                yield return json;
            }
        }
    }


}
