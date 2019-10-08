using System;
using FluentValidation;
using FluentValidation.Results;
using gidu.Domain.Helpers;

namespace gidu.Domain.Core.Members
{
    public class MemberPersonalDataValidation : AbstractValidator<MemberPersonalData>
    {
        public static string NameIsEmpty = "Nome do membro está vazio";
        public static string NameInvalidNumberOfCharacters = "Nome do membro permite entre 4 e 60 caracteres";
        public static string DateOfBirthIsInvalid = "Date de nascimento do membro inválida";
        public static string MaritalStatusIsEmpty = "Estado civil do membro vazia";
        public static string MaritalStatusIsInvalid = "Estado civil do membro é inválido";
        public static string SchoolingIsEmpty = "Escolaridade do membro está vazia";
        public static string SchoolingIsInvalid = "Escolaridade do membro inválida";

        public void ValidateRules(MemberPersonalData memberPersonalData)
        {
            InsertRules();
            ValidationResult results = this.Validate(memberPersonalData);

            if (!results.IsValid)
                throw new DomainException(results.Errors);
        }

        private void InsertRules()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage(NameIsEmpty)
                .Length(4, 60).WithMessage(NameInvalidNumberOfCharacters);

            RuleFor(p => p.DateOfBirth)
                .Must(d => d != DateTime.MinValue && d < DateTime.Now).WithMessage(DateOfBirthIsInvalid);

            RuleFor(p => p.Naturalness);

            RuleFor(p => p.MaritalStatus)
                .NotEmpty().WithMessage(MaritalStatusIsEmpty)
                .Must(m => m.AllowedValue(typeof(MemberMaritalStatus))).WithMessage(MaritalStatusIsInvalid);

            RuleFor(p => p.Schooling)
                .NotEmpty().WithMessage(SchoolingIsEmpty)
                .Must(s => s.AllowedValue(typeof(MemberSchooling))).WithMessage(SchoolingIsInvalid);

            RuleFor(p => p.Profession);
        }
    }
}