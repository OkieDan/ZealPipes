using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZealPipes.Common.Models
{
    public class ZealSettings
    {
        public ZealSettings(IConfiguration configuration)
        {
            EqProcessName = configuration["ApplicationSettings:Zeal:EqProcessName"];
            PipePrefix = configuration["ApplicationSettings:Zeal:PipePrefix"];
            int.TryParse(configuration["ApplicationSettings:Zeal:BufferSize"], out var bufferSize);
            BufferSize = bufferSize;
        }
        public string PipePrefix { get; set; }
        public int BufferSize { get; set; }
        public string EqProcessName { get; set; }
    }
}
