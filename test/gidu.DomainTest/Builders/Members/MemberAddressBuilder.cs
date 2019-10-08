using Bogus;
using gidu.Domain.Core.Members;

namespace gidu.DomainTest.Builders
{
    public class MemberAddressBuilder
    {
        protected string Address;
        protected string Neighborhood;
        protected string Zip;

        public static MemberAddressBuilder New()
        {
            var faker = new Faker("pt_BR");

            return new MemberAddressBuilder()
            {
                Address = faker.Person.Address.Street,
                Neighborhood = faker.Person.Address.City,
                Zip = faker.Address.ZipCode("#####-###")
            };
        }

        public MemberAddressBuilder WithAddress(string address)
        {
            Address = address;
            return this;
        }

        public MemberAddressBuilder WithNeighborhood(string neighborhood)
        {
            Neighborhood = neighborhood;
            return this;
        }

        public MemberAddressBuilder WithZip(string zip)
        {
            Zip = zip;
            return this;
        }

        public MemberAddress Build()
        {
            return new MemberAddress(Address, Neighborhood, Zip);
        }
    }
}