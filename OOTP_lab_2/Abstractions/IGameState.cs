using System.Collections.Generic;
using OOTP_lab_2.Objects;

namespace OOTP_lab_2.Abstractions
{
    public interface IGameState
    {
        IReadOnlyList<IReadOnlyUniqueCardPile> GameCardPiles { get; }

        IReadOnlyDictionary<CardSuit, IReadOnlySequentialOneSuitCardPile> ResultGamePiles { get; }

        int StepsCount { get; }
    }
}
