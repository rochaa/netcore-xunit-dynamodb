using System.Collections.Generic;

namespace gidu.Domain.Core.Members
{
    public class MemberContacts
    {
        public List<string> Phones { get; private set; }
        public string Email { get; private set; }

        public MemberContacts(List<string> phones, string email)
        {
            Phones = phones != null ? phones : new List<string>();
            Email = email;

            (new MemberContactsValidation()).ValidateRules(this);
        }

        protected MemberContacts() { }
    }
}