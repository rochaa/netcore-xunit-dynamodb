using System;

namespace gidu.Domain.Core.Logs
{
    public class Log
    {
        public string Operation { get; private set; }
        public string User { get; private set; }
        public DateTime Date { get; private set; }

        public Log(string operation, string user)
        {
            Operation = operation;
            User = user;
            Date = DateTime.Now;
        }

        public Log() { }
    }
}