using System;

namespace OOTP_lab_2.Objects
{
    public class ConsoleValueConverter<T>
    {
        public string ConvertErrorMessage { get; }

        public Func<string, T> Converter { get; }

        public ConsoleValueConverter(string convertErrorMessage, Func<string, T> converter)
        {
            ConvertErrorMessage = convertErrorMessage;
            Converter = converter;
        }
    }
}
