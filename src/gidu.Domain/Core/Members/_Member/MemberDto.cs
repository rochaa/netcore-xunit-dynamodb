using System;
using System.Collections.Generic;
using gidu.Domain.Core.Logs;

namespace gidu.Domain.Core.Members
{
    public class MemberDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Naturalness { get; set; }
        public string MaritalStatus { get; set; }
        public string Schooling { get; set; }
        public string Profession { get; set; }
        public string Occupation { get; set; }
        public string Church { get; set; }
        public string KeyPhoto { get; set; }
        public string User { get; set; }
        public string LogOperation { get; set; }
        public string Status { get; set; }
        public List<string> Phones { get; set; }
        public MemberAddressDto Address { get; set; }
        public MemberAdmissionDto Admission { get; set; }
        public List<LogDto> Logs { get; set; }
    }
}