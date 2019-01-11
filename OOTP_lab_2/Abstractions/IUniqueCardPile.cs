using OOTP_lab_2.Objects;

namespace OOTP_lab_2.Abstractions
{
    public interface IUniqueCardPile : IReadOnlyUniqueCardPile
    {
        Card Pop();

        void Push(Card card);

        bool Remove(Card card);

        void Clear();
    }
}
