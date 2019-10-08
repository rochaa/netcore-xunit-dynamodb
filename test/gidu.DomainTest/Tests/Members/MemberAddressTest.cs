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
    public class MemberAddressTest
    {
        private readonly MemberAddress _memberAddress;

        public MemberAddressTest()
        {
            _memberAddress = MemberAddressBuilder.New().Build();
        }

        [Fact]
        public void MustValidObject()
        {
            // Given
            var memberAddressExpected = new
            {
                _memberAddress.Address,
                _memberAddress.Neighborhood,
                _memberAddress.Zip
            };

            // When
            MemberAddress memberAddress = new MemberAddress(memberAddressExpected.Address,
                memberAddressExpected.Neighborhood, memberAddressExpected.Zip);

            // Then
            memberAddressExpected.ToExpectedObject().ShouldMatch(memberAddress);
        }

        [Theory]
        [InlineData("12345670")]
        [InlineData("ABCDE-EFG")]
        public void MustNotHaveInvalidZip(string invalidZip)
        {
            // Then
            Assert.Throws<DomainException>(() =>
                MemberAddressBuilder.New().WithZip(invalidZip).Build())
            .WithMessage(MemberAddressValidation.ZipIsInvalid);
        }
    }
}