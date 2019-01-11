using OOTP_lab_2.Objects;

namespace OOTP_lab_2.Abstractions
{
    public interface IReadOnlyOneSuitCardPile : IReadOnlyUniqueCardPile
    {
        CardSuit Suit { get; }
    }
}
