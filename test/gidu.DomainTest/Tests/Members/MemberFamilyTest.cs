using ExpectedObjects;
using Xunit;
using gidu.Domain.Core.Members;
using gidu.Domain.Helpers;
using gidu.DomainTest.Builders;
using gidu.DomainTest.Utils;
using System;

namespace gidu.DomainTest.Tests.Members
{
    public class MemberFamilyTest
    {
        private readonly MemberFamily _memberFamily;

        public MemberFamilyTest()
        {
            _memberFamily = MemberFamilyBuilder.New().Build();
        }

        [Fact]
        public void MustValidObject()
        {
            // Given
            var memberFamilyExpected = new
            {
                _memberFamily.DateMarried,
                _memberFamily.Spouse,
                _memberFamily.Father,
                _memberFamily.Mother,
                _memberFamily.Children
            };

            // When
            MemberFamily memberFamily = new MemberFamily(memberFamilyExpected.DateMarried, memberFamilyExpected.Spouse,
                _memberFamily.Father, _memberFamily.Mother, _memberFamily.Children);

            // Then
            memberFamilyExpected.ToExpectedObject().ShouldMatch(memberFamily);
        }
    }
}