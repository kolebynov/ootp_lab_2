using OOTP_lab_2.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOTP_lab_2.Implementations;
using OOTP_lab_2.Objects;

namespace OOTP_lab_2
{
    class Program
    {
        static void Main(string[] args)
        {
            var sequence = new[]
            {
                CardNumber.Six, CardNumber.Seven, CardNumber.Eight, CardNumber.Nine, CardNumber.Ten, CardNumber.Jack,
                CardNumber.Lady, CardNumber.King, CardNumber.Ace
            };

            var pile = new SequentialOneSuitCardPile(sequence, new OneSuitCardPile(CardSuit.Heart, new UniqueCardPile(2)));
            pile.Add(new Card(CardSuit.Heart, CardNumber.Six));
            pile.Add(new Card(CardSuit.Heart, CardNumber.Seven));
            pile.Add(new Card(CardSuit.Heart, CardNumber.Eight));
        }
    }
}
