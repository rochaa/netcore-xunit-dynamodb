using System;
using System.Collections.Generic;
using gidu.Domain.Core.Logs;

namespace gidu.Domain.Core.Users
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Permission { get; set; }
        public string KeyPhoto { get; set; }
        public string Status { get; set; }
        public List<LogDto> Logs { get; set; }
        public DateTime JoinedDate { get; set; }
    }
}