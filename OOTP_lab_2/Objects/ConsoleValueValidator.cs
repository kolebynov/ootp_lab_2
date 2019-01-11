using System;

namespace OOTP_lab_2.Objects
{
    public class ConsoleValueValidator<T>
    {
        public string ErrorMessage { get; }

        public Func<T, bool> Validator { get; }

        public ConsoleValueValidator(Func<T, bool> validator, string errorMessage)
        {
            Validator = validator;
            ErrorMessage = errorMessage;
        }
    }
}
