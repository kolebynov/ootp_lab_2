using System;
using System.Text;
using OOTP_lab_2.Abstractions;
using OOTP_lab_2.Objects;

namespace OOTP_lab_2.Implementations
{
    public class ConsoleGameView : IGameView, IObserver<GameStarted>, IObserver<StepDone>
    {
        private const int GamePilesInRow = 4;

        private const int LeftPadding = 2;

        private const int TopPadding = 1;

        private const int ColumnsBetweenPiles = 5;

        private const int RowsBetweenPiles = 4;

        private readonly IGameController _gameController;

        private readonly ISolitaireGameModel _gameModel;

        private readonly IStringViewProvider<Card> _cardStringViewProvider;

        private string _error;

        private bool _isStartGameOptionVisible;

        private bool _isEndGameOptionVisible;

        public bool IsStartGameOptionVisible
        {
            get { return _isStartGameOptionVisible; }
            set
            {
                _isStartGameOptionVisible = value;
                Render();
            }
        }

        public bool IsEndGameOptionVisible
        {
            get { return _isEndGameOptionVisible; }
            set
            {
                _isEndGameOptionVisible = value;
                Render();
            }
        }

        public ConsoleGameView(IGameController gameController, ISolitaireGameModel gameModel, IStringViewProvider<Card> cardStringViewProvider)
        {
            if (gameController == null)
            {
                throw new ArgumentNullException(nameof(gameController));
            }

            if (gameModel == null)
            {
                throw new ArgumentNullException(nameof(gameModel));
            }

            if (cardStringViewProvider == null)
            {
                throw new ArgumentNullException(nameof(cardStringViewProvider));
            }

            _gameController = gameController;
            _gameModel = gameModel;
            _cardStringViewProvider = cardStringViewProvider;

            _gameModel.Subscribe((IObserver<GameStarted>) this);
            _gameModel.Subscribe((IObserver<StepDone>) this);

            Console.OutputEncoding = Encoding.UTF8;
        }

        public void OnNext(GameStarted value)
        {
            Render();
        }

        public void OnNext(StepDone value)
        {
            Render();
        }

        void IObserver<StepDone>.OnError(Exception error)
        {
            _error = error.Message;
            Render();
        }

        void IObserver<StepDone>.OnCompleted()
        {
            throw new NotImplementedException();
        }

        void IObserver<GameStarted>.OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        void IObserver<GameStarted>.OnCompleted()
        {
            throw new NotImplementedException();
        }

        private void Render()
        {
            Console.Clear();

            RenderGameCardPiles();
            RenderResultCardPiles();
            RenderOptions();
        }

        private void RenderGameCardPiles()
        {
            for (int i = 0; i < _gameModel.GameState.GameCardPiles.Count; ++i)
            {
                var cardPile = _gameModel.GameState.GameCardPiles[i];
                RenderCardPile(cardPile, i / GamePilesInRow, i % GamePilesInRow);
            }
        }

        private void RenderResultCardPiles()
        {
            var i = 1;
            foreach (var resultGamePile in _gameModel.GameState.ResultGamePiles)
            {
                RenderCardPile(resultGamePile.Value, 0, GamePilesInRow + i);
                i++;
            }
        }

        private void RenderOptions()
        {
            var rowPosition = CalculatePileConsoleRowPosition((int)Math.Ceiling(_gameModel.GameState.GameCardPiles.Count / (double)GamePilesInRow));

            Console.SetCursorPosition(0, rowPosition);
            Console.WriteLine("Выберите действие:");
        }

        private void RenderCardPile(IReadOnlyUniqueCardPile cardPile, int row, int column)
        {
            var rowPosition =  CalculatePileConsoleRowPosition(row);
            var columnPosition = CalculatePileConsoleColumnPosition(column);

            foreach (var card in cardPile)
            {
                Console.SetCursorPosition(columnPosition, rowPosition);
                Console.Write(_cardStringViewProvider.ToString(card));
                rowPosition++;
            }
        }

        private int CalculatePileConsoleRowPosition(int row) => TopPadding + row * RowsBetweenPiles;

        private int CalculatePileConsoleColumnPosition(int column) => LeftPadding + column * ColumnsBetweenPiles;
    }
}
