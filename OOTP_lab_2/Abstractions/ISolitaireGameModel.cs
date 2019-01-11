﻿using System;
using OOTP_lab_2.Objects;

namespace OOTP_lab_2.Abstractions
{
    public interface ISolitaireGameModel : IObservable<GameStarted>, IObservable<StepDone>,
        IObservable<GameEnded>
    {
        IGameState GameState { get; }

        void StartGame();

        void MoveCardToResultPile(int fromPileIndex, CardSuit resultPile);

        void MoveCardToAnotherPile(int fromPileIndex, int toPileIndex);

        void MoveSixToResultPile(int fromPileIndex, CardSuit resultPile);

        void EndGame();
    }
}
