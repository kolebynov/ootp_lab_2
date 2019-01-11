using System.Collections.Generic;
using System.Linq;
using OOTP_lab_2.Abstractions;
using OOTP_lab_2.Objects;

namespace OOTP_lab_2.Implementations
{
    public class CardPileFactory : ICardPileFactory
    {
        public IUniqueCardPile CreateUniqueCardPile(int maxCardsInPile) => new UniqueCardPile(maxCardsInPile);

        public ISequentialOneSuitCardPile CreateSequentialOneSuitCardPile(IEnumerable<CardNumber> cardNumbersSequence, CardSuit suit) => 
            new SequentialOneSuitCardPile(cardNumbersSequence, new OneSuitCardPile(suit, new UniqueCardPile(cardNumbersSequence.Count())));
    }
}
