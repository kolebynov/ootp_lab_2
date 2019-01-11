using System.Collections.Generic;
using OOTP_lab_2.Objects;

namespace OOTP_lab_2.Abstractions
{
    public interface ICardPileFactory
    {
        IUniqueCardPile CreateUniqueCardPile(int maxCardsInPile);

        ISequentialOneSuitCardPile CreateSequentialOneSuitCardPile(IEnumerable<CardNumber> cardNumbersSequence,
            CardSuit suit);
    }
}
