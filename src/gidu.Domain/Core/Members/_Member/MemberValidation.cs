using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using gidu.Domain.Helpers;

namespace gidu.Domain.Core.Members
{
    public class MemberValidation : AbstractValidator<Member>
    {
        public static string OccupationIsEmpty = "Ofício do membro está vazio";
        public static string OccupationIsInvalid = "Ofício do membro é inválido";
        public static string ChurchIsEmpty = "Igreja do membro está vazia";
        public static string ChurchIsInvalid = "Igreja do membro está vazia";
        public static string PersonalDataIsEmpty = "Dados pessoais do membro está vazio.";

        public void ValidateRules(Member member)
        {
            InsertRules();
            ValidationResult results = this.Validate(member);

            if (!results.IsValid)
                throw new DomainException(results.Errors);
        }

        private void InsertRules()
        {
            RuleFor(m => m.Occupation)
                .NotEmpty().WithMessage(OccupationIsEmpty)
                .Must(o => o.AllowedValue(typeof(MemberOccupation))).WithMessage(OccupationIsInvalid);

            RuleFor(m => m.Church)
                .NotEmpty().WithMessage(ChurchIsEmpty)
                .Must(c => c.AllowedValue(typeof(MemberChurch))).WithMessage(ChurchIsInvalid);

            RuleFor(m => m.PersonalData)
                .NotNull().WithMessage(PersonalDataIsEmpty);
        }
    }
}