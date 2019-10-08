using System;
using Bogus;
using gidu.Domain.Core.Members;

namespace gidu.DomainTest.Builders
{
    public class MemberAdmissionBuilder
    {
        protected DateTime? Date;
        protected int? Minutes;
        protected string Reception;
        protected int OrderNumber;
        protected string Pastor;

        public static MemberAdmissionBuilder New()
        {
            var faker = new Faker("pt_BR");

            return new MemberAdmissionBuilder()
            {
                Date = faker.Date.Past(),
                Minutes = faker.Random.Int(1, 999),
                Reception = MemberAdmissionReception.BAPTISM_PROFESSION_FAITH,
                OrderNumber = faker.Random.Int(1, 999),
                Pastor = faker.Person.FullName
            };
        }

        public MemberAdmissionBuilder WithDate(DateTime? date)
        {
            Date = date;
            return this;
        }

        public MemberAdmissionBuilder WithMinutes(int? minutes)
        {
            Minutes = minutes;
            return this;
        }

        public MemberAdmissionBuilder WithReception(string reception)
        {
            Reception = reception;
            return this;
        }

        public MemberAdmission Build()
        {
            return new MemberAdmission(Date, Minutes, Reception, OrderNumber, Pastor);
        }
    }
}