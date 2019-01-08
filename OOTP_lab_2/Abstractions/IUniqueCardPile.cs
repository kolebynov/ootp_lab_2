using OOTP_lab_2.Objects;
using System.Collections.Generic;

namespace OOTP_lab_2.Abstractions
{
    public interface IUniqueCardPile : ICollection<Card>
    {
        int MaxCardsInPile { get; }
    }
}
