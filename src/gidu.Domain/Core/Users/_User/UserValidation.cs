using System;
using FluentValidation;
using FluentValidation.Results;
using gidu.Domain.Helpers;

namespace gidu.Domain.Core.Users
{
    public class UserValidation : AbstractValidator<User>
    {
        public static string NameIsEmpty = "Nome do usuário está vazio";
        public static string NumberOfCharactersName = "Nome do usuário deve ter entre 4 e 60 caracteres";
        public static string EmailIsEmpty = "Email do usuário está vazio";
        public static string EmailIsInvalid = "Formato de email do usuário inválido";
        public static string PasswordIsEmpty = "Senha do usuário está vazia";
        public static string NumberOfCharactersPassword = "Senha do usuário deve ter entre 4 e 15 caracteres";
        public static string PermissionIsEmpty = "Permissão do usuário está vazia";
        public static string NonexistentPermission = "Permissão de usuário não existente.";

        public UserValidation() { }

        public void ValidateRules(User user, bool ValidatePassword = false, bool ValidateOnlyPassword = false)
        {
            if (!ValidateOnlyPassword)
                InsertRules();

            if (ValidatePassword || ValidateOnlyPassword)
                InsertPasswordRules();

            ValidationResult results = this.Validate(user);

            if (!results.IsValid)
                throw new DomainException(results.Errors);
        }

        private void InsertRules()
        {
            RuleFor(u => u.Name)
                .NotEmpty().WithMessage(NameIsEmpty)
                .Length(4, 60).WithMessage(NumberOfCharactersName);

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage(EmailIsEmpty)
                .EmailAddress().WithMessage(EmailIsInvalid);

            RuleFor(u => u.Permission)
                .NotEmpty().WithMessage(PermissionIsEmpty)
                .Must(p => p.AllowedValue(typeof(UserPermission))).WithMessage(NonexistentPermission);
        }

        private void InsertPasswordRules()
        {
            RuleFor(u => u.Password)
                .NotEmpty().WithMessage(PasswordIsEmpty)
                .Length(4, 15).WithMessage(NumberOfCharactersPassword);
        }
    }
}