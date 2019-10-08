using Bogus;
using gidu.Domain.Core.Members;

namespace gidu.DomainTest.Builders
{
    public class FamilyBuilder
    {
        protected string Name;
        protected string MemberId;


        public static FamilyBuilder New()
        {
            var faker = new Faker("pt_BR");

            return new FamilyBuilder()
            {
                Name = faker.Person.FullName,
                MemberId = faker.Random.AlphaNumeric(20),
            };
        }

        public FamilyBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        public FamilyBuilder WithMemberId(string memberId)
        {
            MemberId = memberId;
            return this;
        }

        public Family Build()
        {
            return new Family(Name, MemberId);
        }
    }
}