using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using gidu.Domain.Helpers;

namespace gidu.Domain.Core.Members
{
    public class MemberContactsValidation : AbstractValidator<MemberContacts>
    {
        public static string PhoneIsInvalid = "Telefone do membro é inválido";
        public static string PhoneIsDuplicated = "Telefone do membro já foi informado";
        public static string EmailIsInvalid = "Email do membro é inválido";

        public void ValidateRules(MemberContacts memberContacts)
        {
            InsertRules();
            ValidationResult results = this.Validate(memberContacts);

            if (!results.IsValid)
                throw new DomainException(results.Errors);
        }

        private void InsertRules()
        {
            RuleFor(c => c.Phones)
                .Must(p => !p.Any(e => !e.IsPhoneValid())).WithMessage(PhoneIsInvalid)
                .Must(p => !p.GroupBy(g => g).Where(m => m.Count() > 1).Any()).WithMessage(PhoneIsDuplicated);

            RuleFor(c => c.Email)
            .Must(e => string.IsNullOrEmpty(e) || e.IsEmailValid()).WithMessage(EmailIsInvalid);
        }
    }
}