using BalticAmadeusTask.Services;
using FluentValidation;
using FluentValidation.Validators;
using System;

namespace BalticAmadeusTask.Models
{
    public class RegisteredUserValidator : AbstractValidator<RegisteredUser>
    {
        private readonly IPasswordPolicyService _passwordPolicyService;

        public RegisteredUserValidator(IPasswordPolicyService passwordPolicyService)
        {
            _passwordPolicyService = passwordPolicyService;

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Email is mandatory.")
                .EmailAddress().WithMessage("Email is incorrect.");

            RuleFor(x => x.DateOfBirth)
                .Cascade(CascadeMode.StopOnFirstFailure).NotEmpty()
                .Must(date => date != default(DateTime))
                .WithMessage("Start date is required");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is mandatory.")
                .MaximumLength(50);

            RuleFor(x => x.Password)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Password is mandatory.")
                .Custom(ValidatePassword);

            RuleFor(x => x.AdditionalInfo)
                .MaximumLength(1024);
        }

        private void ValidatePassword(string password, CustomContext context)
        {
            var errors = _passwordPolicyService.PasswordErrors(password);
            errors?.ForEach(
                error =>
                {
                    if (!string.IsNullOrEmpty(error))
                    {
                        context.AddFailure(error);
                    }
                }
                );
        }
    }
}
