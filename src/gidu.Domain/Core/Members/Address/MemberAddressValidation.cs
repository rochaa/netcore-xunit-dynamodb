using System;
using FluentValidation;
using FluentValidation.Results;
using gidu.Domain.Helpers;

namespace gidu.Domain.Core.Members
{
    public class MemberAddressValidation : AbstractValidator<MemberAddress>
    {
        public static string ZipIsInvalid = "Cep do membro está inválido";

        public void ValidateRules(MemberAddress MemberAddress)
        {
            InsertRules();
            ValidationResult results = this.Validate(MemberAddress);

            if (!results.IsValid)
                throw new DomainException(results.Errors);
        }

        private void InsertRules()
        {
            RuleFor(e => e.Address);

            RuleFor(e => e.Neighborhood);

            RuleFor(e => e.Zip)
                .Must(c => string.IsNullOrEmpty(c) || c.IsValidZip()).WithMessage(ZipIsInvalid);
        }
    }
}