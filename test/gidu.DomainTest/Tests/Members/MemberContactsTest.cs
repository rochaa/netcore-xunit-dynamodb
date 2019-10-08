using ExpectedObjects;
using Xunit;
using gidu.Domain.Core.Members;
using gidu.Domain.Helpers;
using gidu.DomainTest.Builders;
using gidu.DomainTest.Utils;
using System.Collections.Generic;

namespace gidu.DomainTest.Tests.Members
{
    public class MemberContactsTest
    {
        private readonly MemberContacts _memberContacts;

        public MemberContactsTest()
        {
            _memberContacts = MemberContactsBuilder.New().Build();
        }

        [Fact]
        public void MustValidObject()
        {
            // Given
            var memberContactsExpected = new
            {
                _memberContacts.Phones,
                _memberContacts.Email,
            };

            // When
            MemberContacts memberContacts = new MemberContacts(memberContactsExpected.Phones, memberContactsExpected.Email);

            // Then
            memberContactsExpected.ToExpectedObject().ShouldMatch(memberContacts);
        }

        [Theory]
        [InlineData("(11) 1234-5678")]
        [InlineData("12345678")]
        [InlineData("111234-5678")]
        public void MustNotHaveInvalidPhone(string phone)
        {
            // Given
            var phoneInvalid = new List<string>{
                phone
            };

            // Then
            Assert.Throws<DomainException>(() =>
                MemberContactsBuilder.New().WithPhones(phoneInvalid).Build())
            .WithMessage(MemberContactsValidation.PhoneIsInvalid);
        }

        [Fact]
        public void MustNotHaveDuplicatedPhone()
        {
            // Given
            var phoneInvalid = new List<string>{
                "1112345678",
                "1112345678"
            };

            // Then
            Assert.Throws<DomainException>(() =>
                MemberContactsBuilder.New().WithPhones(phoneInvalid).Build())
            .WithMessage(MemberContactsValidation.PhoneIsDuplicated);
        }

        [Theory]
        [InlineData("email@")]
        [InlineData("123.com.br")]
        [InlineData("@gmail.com")]
        public void MustNotHaveInvalidEmail(string invalidEmail)
        {
            // Then
            Assert.Throws<DomainException>(() =>
                MemberContactsBuilder.New().WithEmail(invalidEmail).Build())
            .WithMessage(MemberContactsValidation.EmailIsInvalid);
        }
    }
}