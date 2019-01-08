using System.Collections.Generic;
using OOTP_lab_2.Objects;

namespace OOTP_lab_2.Abstractions
{
    public interface ISequentialOneSuitCardPile : IOneSuitCardPile
    {
        IEnumerable<CardNumber> CardNumbersSequence { get; }
    }
}
