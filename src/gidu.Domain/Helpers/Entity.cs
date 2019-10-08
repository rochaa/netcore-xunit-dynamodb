using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;
using gidu.Domain.Core.Logs;

namespace gidu.Domain.Helpers
{
    public class Entity
    {
        [DynamoDBHashKey]
        public string Id { get; set; }
        public List<Log> Logs { get; set; }

        public void AddLog(string logOperation, string user)
        {
            if (Logs == null)
                Logs = new List<Log>();

            Logs.Add(new Log(logOperation, user));
        }
    }
}