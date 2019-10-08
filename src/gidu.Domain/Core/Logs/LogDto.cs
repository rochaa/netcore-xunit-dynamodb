using System;

namespace gidu.Domain.Core.Logs
{
    public class LogDto
    {
        public string Operation { get; set; }
        public string User { get; set; }
        public DateTime Date { get; set; }
    }
}