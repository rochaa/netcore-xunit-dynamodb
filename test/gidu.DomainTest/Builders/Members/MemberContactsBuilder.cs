using System.Collections.Generic;
using Bogus;
using gidu.Domain.Core.Members;

namespace gidu.DomainTest.Builders
{
    public class MemberContactsBuilder
    {
        protected List<string> Phones;
        protected string Email;

        public static MemberContactsBuilder New()
        {
            var faker = new Faker("pt_BR");

            return new MemberContactsBuilder()
            {
                Phones = new List<string> {
                    faker.Random.String2(10, "0123456789")
        },
                Email = faker.Person.Email
            };
        }

        public MemberContactsBuilder WithPhones(List<string> phones)
        {
            Phones = phones;
            return this;
        }

        public MemberContactsBuilder WithEmail(string email)
        {
            Email = email;
            return this;
        }

        public MemberContacts Build()
        {
            return new MemberContacts(Phones, Email);
        }
    }
}