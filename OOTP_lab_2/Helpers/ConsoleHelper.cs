using System;
using System.Collections.Generic;
using OOTP_lab_2.Objects;

namespace OOTP_lab_2.Helpers
{
    public static class ConsoleHelper
    {
        public static int ReadInt(IEnumerable<ConsoleValueValidator<int>> validators = null) =>
            Read(new ConsoleValueConverter<int>("Вы ввели не числовое значение", int.Parse), validators);

        public static T Read<T>(ConsoleValueConverter<T> converter, IEnumerable<ConsoleValueValidator<T>> validators = null)
        {
            var hasError = true;
            T value = default(T);

            while (hasError)
            {
                hasError = false;

                try
                {
                    value = converter.Converter(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine(converter.ConvertErrorMessage);
                    hasError = true;
                    continue;
                }

                foreach (var validator in validators)
                {
                    if (!validator.Validator(value))
                    {
                        Console.WriteLine(validator.ErrorMessage);
                        hasError = true;
                        break;
                    }
                }
            }

            return value;
        }
    }
}
