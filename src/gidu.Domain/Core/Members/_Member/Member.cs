using System;
using gidu.Domain.Helpers;
using gidu.Domain.Core.Logs;

namespace gidu.Domain.Core.Members
{
    public class Member : Entity
    {
        public string Occupation { get; private set; }
        public string Church { get; private set; }
        public string KeyPhoto { get; private set; }
        public string Search { get; private set; }
        public string Status { get; private set; }
        public MemberPersonalData PersonalData { get; private set; }
        public MemberAddress Address { get; private set; }
        public MemberAdmission Admission { get; private set; }
        public MemberContacts Contacts { get; private set; }
        public MemberFamily Family { get; private set; }

        public Member(string occupation, string church, MemberPersonalData personalData, MemberAddress address,
            MemberAdmission admission, MemberFamily family, MemberContacts contacts, string userOperation)
        {
            FillMember(occupation, church, personalData, address, admission, family, contacts, LogOperation.INSERT, userOperation);

            (new MemberValidation()).ValidateRules(this);
        }

        protected Member() { }

        private void FillMember(string occupation, string church, MemberPersonalData personalData, MemberAddress address,
            MemberAdmission admission, MemberFamily family, MemberContacts contacts, string logOperation, string userOperation)
        {
            Occupation = occupation;
            Church = church;
            Search = personalData != null ? $"{personalData.Name.ToLower().RemoveDiacritics()} {personalData.Profession.ToLower().RemoveDiacritics()}" : null;
            Status = MemberStatus.ACTIVE;
            PersonalData = personalData;
            Address = address;
            Admission = admission;
            Family = family;
            Contacts = contacts;

            AddLog(logOperation, userOperation);
        }

        public void Update(string occupation, string church, MemberPersonalData personalData, MemberAddress address,
            MemberAdmission admission, MemberFamily family, MemberContacts contacts, string userOperation)
        {
            FillMember(occupation, church, personalData, address, admission, family, contacts, LogOperation.UPDATE, userOperation);

            (new MemberValidation()).ValidateRules(this);
        }

        public void Delete(string user)
        {
            Status = MemberStatus.EXCLUDED;
            AddLog(LogOperation.DELETE, user);
        }

        public void AddKeyPhoto(string key)
        {
            KeyPhoto = key;
        }
    }
}