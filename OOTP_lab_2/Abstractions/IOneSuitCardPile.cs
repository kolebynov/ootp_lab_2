using OOTP_lab_2.Objects;

namespace OOTP_lab_2.Abstractions
{
    public interface IOneSuitCardPile : IUniqueCardPile
    {
        CardSuit Suit { get; }
    }
}
