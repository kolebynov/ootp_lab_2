using System.Collections.Generic;
using OOTP_lab_2.Objects;

namespace OOTP_lab_2.Abstractions
{
    public interface IReadOnlyUniqueCardPile : IEnumerable<Card>
    {
        int MaxCardsInPile { get; }

        int Count { get; }

        Card Peek();
    }
}
