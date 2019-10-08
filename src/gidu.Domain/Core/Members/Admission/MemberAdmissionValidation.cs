using System;
using FluentValidation;
using FluentValidation.Results;
using gidu.Domain.Helpers;

namespace gidu.Domain.Core.Members
{
    public class MemberAdmissionValidation : AbstractValidator<MemberAdmission>
    {
        public static string DateIsInvalid = "Data de admissão do membro maior que a data atual";
        public static string ReceptionIsInvalid = "Recepção da admissão do membro inválida";

        public void ValidateRules(MemberAdmission memberAdmission)
        {
            InsertRules();
            ValidationResult results = this.Validate(memberAdmission);

            if (!results.IsValid)
                throw new DomainException(results.Errors);
        }

        private void InsertRules()
        {
            RuleFor(a => a.Date)
                .Must(d => d == null || d <= DateTime.Now).WithMessage(DateIsInvalid);

            RuleFor(a => a.Minutes);

            RuleFor(a => a.Reception)
                .Must(r => string.IsNullOrEmpty(r) || r.AllowedValue(typeof(MemberAdmissionReception))).WithMessage(ReceptionIsInvalid);

            RuleFor(a => a.OrderNumber);

            RuleFor(a => a.Pastor);
        }
    }
}