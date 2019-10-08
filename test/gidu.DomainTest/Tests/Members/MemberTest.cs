using ExpectedObjects;
using Xunit;
using gidu.Domain.Core.Members;
using gidu.Domain.Helpers;
using gidu.DomainTest.Builders;
using gidu.DomainTest.Utils;

namespace gidu.DomainTest.Tests.Members
{
    public class MemberTest
    {
        private readonly Member _member;

        public MemberTest()
        {
            _member = MemberBuilder.New().Build();
        }

        [Fact]
        public void MustValidObject()
        {
            // Given
            var memberExpected = new
            {
                _member.Occupation,
                _member.Church,
                _member.KeyPhoto,
                _member.Search,
                _member.Status,
                _member.PersonalData,
                _member.Address,
                _member.Admission,
                _member.Family,
                _member.Contacts
            };
            var userLog = "Admin";

            // When
            Member member = new Member(memberExpected.Occupation, memberExpected.Church, memberExpected.PersonalData, memberExpected.Address,
                memberExpected.Admission, memberExpected.Family, memberExpected.Contacts, userLog);

            // Then
            memberExpected.ToExpectedObject().ShouldMatch(member);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("BISPO")]
        public void MustNotHaveInvalidOccupation(string invalidOccupation)
        {
            // Then
            Assert.Throws<DomainException>(() =>
                MemberBuilder.New().WithOccupation(invalidOccupation).Build())
            .WithMessage(MemberValidation.OccupationIsEmpty, MemberValidation.OccupationIsInvalid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("JARDINS")]
        public void MustNotHaveInvalidChurch(string invalidChurch)
        {
            // Then
            Assert.Throws<DomainException>(() =>
                MemberBuilder.New().WithChurch(invalidChurch).Build())
            .WithMessage(MemberValidation.ChurchIsEmpty, MemberValidation.ChurchIsInvalid);
        }

        [Fact]
        public void MustNotHavePersonalDataIsNull()
        {
            // Then
            Assert.Throws<DomainException>(() =>
                MemberBuilder.New().WithPersonalData(null).Build())
            .WithMessage(MemberValidation.PersonalDataIsEmpty);
        }
    }
}