using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace gidu.Domain.Helpers
{
    public class DomainException : ArgumentException
    {
        public IList<ValidationFailure> Errors { get; set; }

        public DomainException(IList<ValidationFailure> errors) : base(string.Join(" --- ", errors.Select(s => s.ErrorMessage)))
        {
            Errors = errors;
        }

        public DomainException(string source, string error) : base(error)
        {
            Errors = new List<ValidationFailure>();
            Errors.Add(new ValidationFailure(source, error));
        }
    }
}