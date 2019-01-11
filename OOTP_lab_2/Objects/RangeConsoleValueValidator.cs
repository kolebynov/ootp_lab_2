using System;

namespace OOTP_lab_2.Objects
{
    class RangeConsoleValueValidator<T> : ConsoleValueValidator<T>
        where T : IComparable<T>
    {
        public RangeConsoleValueValidator(T start, T end) 
            : base(value => value.CompareTo(start) >= 0 && value.CompareTo(end) <= 0,
                $"Значение должно находится в диапазоне от {start} до {end}")
        {
        }
    }
}
