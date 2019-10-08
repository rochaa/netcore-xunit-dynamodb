using System;

namespace gidu.Domain.Core.Members
{
    public class MemberPersonalData
    {
        public string Name { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public string Naturalness { get; private set; }
        public string MaritalStatus { get; private set; }
        public string Schooling { get; private set; }
        public string Profession { get; private set; }

        public MemberPersonalData(string name, DateTime dateOfBirth, string naturalness, string maritalStatus, string schooling, string profession)
        {
            Name = name;
            DateOfBirth = dateOfBirth;
            Naturalness = naturalness;
            MaritalStatus = maritalStatus;
            Schooling = schooling;
            Profession = profession;

            (new MemberPersonalDataValidation()).ValidateRules(this);
        }

        protected MemberPersonalData() { }
    }
}