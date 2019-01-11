using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOTP_lab_2.Abstractions;
using OOTP_lab_2.Helpers;
using OOTP_lab_2.Objects;

namespace OOTP_lab_2.Implementations
{
    public class ConsoleGameView : IGameView, IObserver<GameStarted>, IObserver<StepDone>, IObserver<GameEnded>
    {
        private const int GamePilesInRow = 4;

        private const int LeftPadding = 2;

        private const int TopPadding = 1;

        private const int ColumnsBetweenPiles = 5;

        private const int RowsBetweenPiles = 4;

        private readonly IGameController _gameController;

        private readonly ISolitaireGameModel _gameModel;

        private readonly IStringViewProvider<Card> _cardStringViewProvider;

        private readonly IStringViewProvider<CardSuit> _cardSuitStringViewProvider;

        private readonly List<GameAction> _gameActions;

        private GameModelEvent _lastModelEvent;

        private string _error;

        public ConsoleGameView(
            IGameController gameController,
            ISolitaireGameModel gameModel, 
            IStringViewProvider<Card> cardStringViewProvider,
            IStringViewProvider<CardSuit> cardSuitStringViewProvider)
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

            if (cardSuitStringViewProvider == null)
            {
                throw new ArgumentNullException(nameof(cardSuitStringViewProvider));
            }

            _gameController = gameController;
            _gameModel = gameModel;
            _cardStringViewProvider = cardStringViewProvider;
            _cardSuitStringViewProvider = cardSuitStringViewProvider;

            _gameModel.Subscribe((IObserver<GameStarted>) this);
            _gameModel.Subscribe((IObserver<StepDone>) this);
            _gameModel.Subscribe((IObserver<GameEnded>) this);

            _gameActions = new List<GameAction>
            {
                new GameAction
                {
                    Action = MoveCardToResultPileAction,
                    Name = "Переместить карту в результирующую стопку"
                },
                new GameAction
                {
                    Action = MoveCardToAnotherPileAction,
                    Name = "Переместить карту из одной стопки в другую"
                },
                new GameAction
                {
                    Action = MoveSixCardToResultPile,
                    Name = "Переместить шестерку из середины стопки в результирующую стопку"
                },
                new GameAction
                {
                    Action = EndGameAction,
                    Name = "Завершить игру"
                }
            };

            Console.OutputEncoding = Encoding.UTF8;
        }

        #region Observer

        public void OnNext(GameStarted value)
        {
            _lastModelEvent = new GameModelEvent
            {
                Event = value
            };

            MainLoop();
        }

        public void OnNext(StepDone value)
        {
            _lastModelEvent = new GameModelEvent
            {
                Event = value
            };
        }

        public void OnNext(GameEnded value)
        {
            _lastModelEvent = new GameModelEvent
            {
                Event = value
            };
        }

        public void OnError(Exception error)
        {
            _lastModelEvent = new GameModelEvent
            {
                Error = error
            };
        }

        public void OnCompleted()
        {
        }

        void IObserver<StepDone>.OnError(Exception error)
        {
            _lastModelEvent = new GameModelEvent
            {
                Error = error
            };
        }

        void IObserver<StepDone>.OnCompleted()
        {
        }

        void IObserver<GameStarted>.OnError(Exception error)
        {
            _lastModelEvent = new GameModelEvent
            {
                Error = error
            };
        }

        void IObserver<GameStarted>.OnCompleted()
        {
        }

        #endregion

        private void MainLoop()
        {
            while (Update())
            {
            }

            Console.WriteLine("Нажмите <Enter> для выхода");
            Console.ReadLine();
        }

        private bool Update()
        {
            if (_lastModelEvent.Error != null)
            {
                _error = _lastModelEvent.Error.Message;
            }

            Render();

            _error = null;

            var endGameEvent = _lastModelEvent.Event as GameEnded;
            if (endGameEvent == null)
            {
                SelectAndPerformGameAction();
                return true;
            }
            else
            {
                return false;
            }
        }

        #region Render

        private void Render()
        {
            Console.Clear();

            RenderGameCardPiles();
            RenderResultCardPiles();

            var rowPosition = CalculatePileConsoleRowPosition((int)Math.Ceiling(_gameModel.GameState.GameCardPiles.Count / (double)GamePilesInRow));
            Console.SetCursorPosition(0, rowPosition);

            var endGameEvent = _lastModelEvent.Event as GameEnded;
            if (endGameEvent == null)
            {
                RenderError();
                RenderGameActions();
            }
            else
            {
                RenderEndGameInfo(endGameEvent.EndGameReason);
            }
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

        public void RenderError()
        {
            if (!string.IsNullOrEmpty(_error))
            {
                var oldColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(_error);
                Console.ForegroundColor = oldColor;
            }
        }

        private void RenderGameActions()
        {
            Console.WriteLine("Выберите действие:");

            for (int i = 0; i < _gameActions.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_gameActions[i].Name}");
            }
        }

        private void RenderCardPile(IReadOnlyUniqueCardPile cardPile, int row, int column)
        {
            var rowPosition = CalculatePileConsoleRowPosition(row);
            var columnPosition = CalculatePileConsoleColumnPosition(column);

            foreach (var card in cardPile)
            {
                Console.SetCursorPosition(columnPosition, rowPosition);
                Console.Write(_cardStringViewProvider.ToString(card));
                rowPosition++;
            }
        }

        private void RenderEndGameInfo(EndGameReason endGameReason)
        {
            Console.Write("Игра завершена. ");

            switch (endGameReason)
            {
                case EndGameReason.Win:
                    Console.WriteLine("Вы победили");
                    break;
                case EndGameReason.Defeat:
                    Console.WriteLine("Вы проиграли");
                    break;
                case EndGameReason.Manual:
                    Console.WriteLine("Вы завершили игру досрочно");
                    break;
            }
            
            Console.WriteLine("Ваша статистика:");
            Console.WriteLine($"Количество ходов: {_gameModel.GameState.StepsCount}");
        }

        #endregion

        #region GameActions

        private void MoveCardToResultPileAction()
        {
            var gamePileIndex = SelectGamePile("Индекс стопки, откуда брать карту");
            var suit = SelectCardSuit("Масть результирующей стопки, куда положить карту");

            _gameController.MoveCardToResultPile(gamePileIndex, suit);
        }

        private void MoveCardToAnotherPileAction()
        {
            var fromPileIndex = SelectGamePile("Индекс стопки, откуда брать карту");
            var toPileIndex = SelectGamePile("Индекс стопки, куда положить карту");

            _gameController.MoveCardToAnotherPile(fromPileIndex, toPileIndex);
        }

        private void MoveSixCardToResultPile()
        {
            var fromPileIndex = SelectGamePile("Индекс стопки");
            var suit = SelectCardSuit("Масть результирующей стопки, куда положить шестерку");

            _gameController.MoveSixToResultPile(fromPileIndex, suit);
        }

        private void EndGameAction() => _gameController.EndGame();

        #endregion

        #region Helpers

        public void SelectAndPerformGameAction()
        {
            var selectedOption =
                ConsoleHelper.ReadInt(new[] { new RangeConsoleValueValidator<int>(1, _gameActions.Count) }) - 1;
            _gameActions[selectedOption].Action();
        }

        private int SelectGamePile(string message)
        {
            Console.Write($"{message} (1 - {_gameModel.GameState.GameCardPiles.Count}): ");
            return ConsoleHelper.ReadInt(new[]
                       { new RangeConsoleValueValidator<int>(1, _gameModel.GameState.GameCardPiles.Count) }) - 1;
        }

        private CardSuit SelectCardSuit(string message)
        {
            var suits = Enum.GetValues(typeof(CardSuit))
                .Cast<int>()
                .OrderBy(x => x)
                .ToArray();
            var suitString = suits
                .Select(x => $"{x + 1} - {_cardSuitStringViewProvider.ToString((CardSuit)x)}");

            Console.Write($"{message} ({string.Join(", ", suitString)}): ");

            return (CardSuit)ConsoleHelper.ReadInt(new[]
                {new RangeConsoleValueValidator<int>(suits[0] + 1, suits[suits.Length - 1] + 1)}) - 1;
        }

        private int CalculatePileConsoleRowPosition(int row) => TopPadding + row * RowsBetweenPiles;

        private int CalculatePileConsoleColumnPosition(int column) => LeftPadding + column * ColumnsBetweenPiles;

        #endregion

        private class GameAction
        {
            public string Name { get; set; }

            public Action Action { get; set; }
        }

        private class GameModelEvent
        {
            public object Event { get; set; }

            public Exception Error { get; set; }
        }
    }
}
