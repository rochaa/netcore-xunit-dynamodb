using System;
using System.Collections.Generic;
using Bogus;
using gidu.Domain.Core.Members;

namespace gidu.DomainTest.Builders
{
    public class MemberBuilder
    {
        protected string Id;
        protected string Occupation;
        protected string Church;
        protected string KeyPhoto;
        protected string Search;
        protected string Status;
        protected MemberPersonalData PersonalData;
        protected MemberAddress Address;
        protected MemberAdmission Admission;
        protected MemberFamily Family;
        protected MemberContacts Contacts;

        public static MemberBuilder New()
        {
            var faker = new Faker("pt_BR");

            return new MemberBuilder()
            {
                Occupation = MemberOccupation.MEMBER,
                Church = MemberChurch.SANTO_AMARO,
                KeyPhoto = null,
                PersonalData = MemberPersonalDataBuilder.New().Build(),
                Address = MemberAddressBuilder.New().Build(),
                Admission = MemberAdmissionBuilder.New().Build(),
                Family = MemberFamilyBuilder.New().Build(),
                Contacts = MemberContactsBuilder.New().Build(),
            };
        }

        public MemberBuilder WithId(string id)
        {
            Id = id;
            return this;
        }

        public MemberBuilder WithOccupation(string occupation)
        {
            Occupation = occupation;
            return this;
        }

        public MemberBuilder WithChurch(string church)
        {
            Church = church;
            return this;
        }

        public MemberBuilder WithPersonalData(MemberPersonalData personalData)
        {
            PersonalData = personalData;
            return this;
        }

        public MemberBuilder WithAddress(MemberAddress address)
        {
            Address = address;
            return this;
        }

        public MemberBuilder WithAdmission(MemberAdmission admission)
        {
            Admission = admission;
            return this;
        }

        public MemberBuilder WithFamily(MemberFamily family)
        {
            Family = family;
            return this;
        }

        public MemberBuilder WithContacts(MemberContacts contacts)
        {
            Contacts = contacts;
            return this;
        }

        public Member Build()
        {
            return new Member(Occupation, Church, PersonalData, Address, Admission, Family, Contacts, "Admin");
        }
    }
}