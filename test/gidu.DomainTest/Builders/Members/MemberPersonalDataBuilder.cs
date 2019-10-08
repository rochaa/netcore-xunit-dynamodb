using System;
using Bogus;
using gidu.Domain.Core.Members;

namespace gidu.DomainTest.Builders
{
    public class MemberPersonalDataBuilder
    {
        protected string Name;
        protected DateTime DateOfBirth;
        protected string Naturalness;
        protected string MaritalStatus;
        protected string Schooling;
        protected string Profession;

        public static MemberPersonalDataBuilder New()
        {
            var faker = new Faker("pt_BR");

            return new MemberPersonalDataBuilder()
            {
                Name = faker.Person.FullName,
                DateOfBirth = faker.Person.DateOfBirth,
                Naturalness = faker.Address.City(),
                MaritalStatus = MemberMaritalStatus.MARRIED,
                Schooling = MemberSchooling.HIGHER_EDUCATION,
                Profession = faker.Random.Utf16String(1, 20)
            };
        }

        public MemberPersonalDataBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        public MemberPersonalDataBuilder WithDateOfBirth(DateTime dateOfBirth)
        {
            DateOfBirth = dateOfBirth;
            return this;
        }

        public MemberPersonalDataBuilder WithNaturalness(string naturalness)
        {
            Naturalness = naturalness;
            return this;
        }

        public MemberPersonalDataBuilder WithMaritalStatus(string maritalStatus)
        {
            MaritalStatus = maritalStatus;
            return this;
        }

        public MemberPersonalDataBuilder WithSchooling(string schooling)
        {
            Schooling = schooling;
            return this;
        }

        public MemberPersonalDataBuilder WithProfession(string profession)
        {
            Profession = profession;
            return this;
        }

        public MemberPersonalData Build()
        {
            return new MemberPersonalData(Name, DateOfBirth, Naturalness, MaritalStatus, Schooling, Profession);
        }
    }
}