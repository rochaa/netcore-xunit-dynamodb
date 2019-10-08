using System;
using System.Collections.Generic;
using Bogus;
using gidu.Domain.Core.Members;

namespace gidu.DomainTest.Builders
{
    public class MemberFamilyBuilder
    {
        protected DateTime? DateMarried;
        protected Family Spouse;
        protected Family Father;
        protected Family Mother;
        protected List<Family> Children;

        public static MemberFamilyBuilder New()
        {
            var faker = new Faker("pt_BR");

            return new MemberFamilyBuilder()
            {
                DateMarried = faker.Date.Past(),
                Spouse = FamilyBuilder.New().Build(),
                Father = FamilyBuilder.New().Build(),
                Mother = FamilyBuilder.New().Build(),
                Children = new List<Family> {
                    FamilyBuilder.New().Build()
                },
            };
        }

        public MemberFamilyBuilder WithDateMarried(DateTime? dateMarried)
        {
            DateMarried = dateMarried;
            return this;
        }

        public MemberFamilyBuilder WithSpouse(Family spouse)
        {
            Spouse = spouse;
            return this;
        }

        public MemberFamilyBuilder WithFather(Family father)
        {
            Father = father;
            return this;
        }

        public MemberFamilyBuilder WithMother(Family mother)
        {
            Mother = mother;
            return this;
        }

        public MemberFamilyBuilder WithChildren(List<Family> children)
        {
            Children = children;
            return this;
        }

        public MemberFamily Build()
        {
            return new MemberFamily(DateMarried, Spouse, Father, Mother, Children);
        }
    }
}