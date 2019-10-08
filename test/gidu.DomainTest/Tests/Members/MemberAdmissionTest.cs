using ExpectedObjects;
using Xunit;
using gidu.Domain.Core.Members;
using gidu.Domain.Helpers;
using gidu.DomainTest.Builders;
using gidu.DomainTest.Utils;
using System;

namespace gidu.DomainTest.Tests.Members
{
    public class MemberAdmissionTest
    {
        private readonly MemberAdmission _memberAdmission;

        public MemberAdmissionTest()
        {
            _memberAdmission = MemberAdmissionBuilder.New().Build();
        }

        [Fact]
        public void MustValidObject()
        {
            // Given
            var memberAdmissionExpected = new
            {
                _memberAdmission.Date,
                _memberAdmission.Minutes,
                _memberAdmission.Reception,
                _memberAdmission.OrderNumber,
                _memberAdmission.Pastor
            };

            // When
            MemberAdmission memberAdmission = new MemberAdmission(memberAdmissionExpected.Date, memberAdmissionExpected.Minutes,
                _memberAdmission.Reception, _memberAdmission.OrderNumber, _memberAdmission.Pastor);

            // Then
            memberAdmissionExpected.ToExpectedObject().ShouldMatch(memberAdmission);
        }

        [Fact]
        public void MustNotHaveInvalidDate()
        {
            // Given
            var dateInvalid = DateTime.Now.AddDays(1).Date;

            // Then
            Assert.Throws<DomainException>(() =>
                MemberAdmissionBuilder.New().WithDate(dateInvalid).Build())
            .WithMessage(MemberAdmissionValidation.DateIsInvalid);
        }

        [Fact]
        public void MustNotHaveInvalidReception()
        {
            // Given
            var invalidReception = "Indicação";

            // Then
            Assert.Throws<DomainException>(() =>
                MemberAdmissionBuilder.New().WithReception(invalidReception).Build())
            .WithMessage(MemberAdmissionValidation.ReceptionIsInvalid);
        }
    }
}