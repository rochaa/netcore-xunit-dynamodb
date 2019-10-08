using System.Linq;
using gidu.Domain.Helpers;
using Xunit;

namespace gidu.DomainTest.Utils
{
    public static class AssertExtension
    {
        public static void WithMessage(this DomainException exception, params string[] errors)
        {
            if (exception.Errors.Any(e => errors.Contains(e.ErrorMessage)))
                Assert.True(true);
            else
                Assert.False(true, $"Esperava a mensagem '{string.Join(",", errors)}'");
        }
    }
}