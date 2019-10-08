using ExpectedObjects;
using Xunit;
using gidu.Domain.Core.Members;
using gidu.Domain.Helpers;
using gidu.DomainTest.Builders;
using gidu.DomainTest.Utils;
using System.Collections.Generic;
using System;

namespace gidu.DomainTest.Tests.Members
{
    public class MemberPersonalDataTest
    {
        private readonly MemberPersonalData _memberPersonalData;

        public static IEnumerable<object[]> _invalidDatesOfBirth =>
            new List<object[]>
            {
                new object[] { (DateTime?)null },
                new object[] { DateTime.MinValue },
                new object[] { DateTime.Now.AddDays(1).Date },
            };

        public MemberPersonalDataTest()
        {
            _memberPersonalData = MemberPersonalDataBuilder.New().Build();
        }

        [Fact]
        public void MustValidObject()
        {
            // Given
            var memberPersonalDataExpected = new
            {
                _memberPersonalData.Name,
                _memberPersonalData.DateOfBirth,
                _memberPersonalData.Naturalness,
                _memberPersonalData.MaritalStatus,
                _memberPersonalData.Schooling,
                _memberPersonalData.Profession
            };

            // When
            MemberPersonalData memberPersonalData = new MemberPersonalData(memberPersonalDataExpected.Name,
                memberPersonalDataExpected.DateOfBirth, memberPersonalDataExpected.Naturalness, memberPersonalDataExpected.MaritalStatus,
                memberPersonalDataExpected.Schooling, memberPersonalDataExpected.Profession);

            // Then
            memberPersonalDataExpected.ToExpectedObject().ShouldMatch(memberPersonalData);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("Lu")]
        [InlineData("Nome tão tão tão grande que ultrapassa o limite de 60 caracteres")]
        public void MustNotHaveInvalidName(string invalidName)
        {
            // Then
            Assert.Throws<DomainException>(() =>
                MemberPersonalDataBuilder.New().WithName(invalidName).Build())
            .WithMessage(MemberPersonalDataValidation.NameIsEmpty, MemberPersonalDataValidation.NameInvalidNumberOfCharacters);
        }

        [Theory]
        [MemberData(nameof(_invalidDatesOfBirth))]
        public void MustNotHaveInvalidDateOfBirth(DateTime dateOfBirth)
        {
            // Then
            Assert.Throws<DomainException>(() =>
                MemberPersonalDataBuilder.New().WithDateOfBirth(dateOfBirth).Build())
            .WithMessage(MemberPersonalDataValidation.DateOfBirthIsInvalid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("Ficando")]
        public void MustNotHaveInvalidMaritalStatus(string invalidMaritalStatus)
        {
            // Then
            Assert.Throws<DomainException>(() =>
                MemberPersonalDataBuilder.New().WithMaritalStatus(invalidMaritalStatus).Build())
            .WithMessage(MemberPersonalDataValidation.MaritalStatusIsEmpty, MemberPersonalDataValidation.MaritalStatusIsInvalid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("Prezinho")]
        public void MustNotHaveInvalidSchooling(string invalidSchooling)
        {
            // Then
            Assert.Throws<DomainException>(() =>
                MemberPersonalDataBuilder.New().WithSchooling(invalidSchooling).Build())
            .WithMessage(MemberPersonalDataValidation.SchoolingIsEmpty, MemberPersonalDataValidation.SchoolingIsInvalid);
        }
    }
}