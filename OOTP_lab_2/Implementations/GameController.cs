using System;
using OOTP_lab_2.Abstractions;
using OOTP_lab_2.Objects;

namespace OOTP_lab_2.Implementations
{
    public class GameController : IGameController
    {
        private readonly ISolitaireGameModel _gameModel;

        private readonly IGameView _gameView;

        public GameController(ISolitaireGameModel gameModel, IGameViewFactory gameViewFactory)
        {
            if (gameModel == null)
            {
                throw new ArgumentNullException(nameof(gameModel));
            }

            if (gameViewFactory == null)
            {
                throw new ArgumentNullException(nameof(gameViewFactory));
            }

            _gameModel = gameModel;
            _gameView = gameViewFactory.Create(this, gameModel);
        }

        public void StartGame() => _gameModel.StartGame();

        public void MoveCardToResultPile(int fromPileIndex, CardSuit resultPile) => _gameModel.MoveCardToResultPile(fromPileIndex, resultPile);

        public void MoveCardToAnotherPile(int fromPileIndex, int toPileIndex) => _gameModel.MoveCardToAnotherPile(fromPileIndex, toPileIndex);

        public void MoveSixToResultPile(int fromPileIndex, CardSuit resultPile) => _gameModel.MoveSixToResultPile(fromPileIndex, resultPile);

        public void EndGame() => _gameModel.EndGame();
    }
}
