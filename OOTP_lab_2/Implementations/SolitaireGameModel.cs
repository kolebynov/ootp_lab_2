using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OOTP_lab_2.Abstractions;
using OOTP_lab_2.Exceptions;
using OOTP_lab_2.Objects;

namespace OOTP_lab_2.Implementations
{
    public class SolitaireGameModel : ISolitaireGameModel
    {
        private const int CardsInGamePile = 3;

        private const int GamePilesCount = 12;

        private static readonly CardNumber[] CardNumbersSequence = 
        {
            CardNumber.Six, CardNumber.Seven, CardNumber.Eight, CardNumber.Nine, CardNumber.Ten, CardNumber.Jack,
            CardNumber.Queen, CardNumber.King, CardNumber.Ace
        };

        private readonly ICardDeck _cardDeck;

        private readonly ICardPileFactory _cardPileFactory;

        private readonly IObserversCollection<GameStarted> _gameStartedObservers;

        private readonly IObserversCollection<StepDone> _stepDoneObservers;

        private readonly IObserversCollection<GameEnded> _gameEndedObservers;

        private Dictionary<CardSuit, ISequentialOneSuitCardPile> _resultCardPiles;

        private IUniqueCardPile[] _gameCardPiles;

        private int _stepsCount = 0;

        private bool _gameIsRunning;

        public IGameState GameState { get; }

        public SolitaireGameModel(
            ICardDeck cardDeck, 
            ICardPileFactory cardPileFactory, 
            IObserversCollection<GameStarted> gameStartedObservers, 
            IObserversCollection<StepDone> stepDoneObservers, 
            IObserversCollection<GameEnded> gameEndedObservers)
        {
            if (cardDeck == null)
            {
                throw new ArgumentNullException(nameof(cardDeck));
            }

            if (cardPileFactory == null)
            {
                throw new ArgumentNullException(nameof(cardPileFactory));
            }

            if (gameStartedObservers == null)
            {
                throw new ArgumentNullException(nameof(gameStartedObservers));
            }

            if (stepDoneObservers == null)
            {
                throw new ArgumentNullException(nameof(stepDoneObservers));
            }

            if (gameEndedObservers == null)
            {
                throw new ArgumentNullException(nameof(gameEndedObservers));
            }

            _cardDeck = cardDeck;
            _cardPileFactory = cardPileFactory;
            _gameStartedObservers = gameStartedObservers;
            _stepDoneObservers = stepDoneObservers;
            _gameEndedObservers = gameEndedObservers;

            GameState = new InternalGameState(this);
        }

        public void StartGame()
        {
            _cardDeck.Shuffle();
            InitCardPiles();

            int index = 0;
            foreach (var card in _cardDeck)
            {
                _gameCardPiles[index / CardsInGamePile].Push(card);
                ++index;
            }

            _gameIsRunning = true;

            _gameStartedObservers.OnNext(new GameStarted());
        }

        public void MoveCardToResultPile(int fromPileIndex, CardSuit resultPile) => 
            DoStep(() => MoveLastCard(_gameCardPiles[fromPileIndex], _resultCardPiles[resultPile]));

        public void MoveCardToAnotherPile(int fromPileIndex, int toPileIndex) => DoStep(() =>
        {
            if (fromPileIndex == toPileIndex)
            {
                throw new CantDoStepException("Нельзя перемещать карту в пределах одной стопки");
            }

            var fromPile = _gameCardPiles[fromPileIndex];
            var toPile = _gameCardPiles[toPileIndex];

            if (!toPile.Any())
            {
                throw new CantDoStepException("Нельзя добавлять карту в пустую стопку.");
            }

            if (fromPile.Peek().Number != toPile.Peek().Number)
            {
                throw new CantDoStepException("Карта верхнего уровня принимающей стопки имеет другой номер.");
            }

            MoveLastCard(fromPile, toPile);
        });

        public void MoveSixToResultPile(int fromPileIndex, CardSuit resultPile) => DoStep(() =>
        {
            if (_stepsCount > 0 || _gameCardPiles.Any(cardPile => cardPile.Peek().Number == CardNumber.Six))
            {
                throw new CantDoStepException("Доставать шестерку можно только в начале игры и, если снизу стопок нет ни одной шестерки.");
            }

            var sixCard = _gameCardPiles[fromPileIndex]
                .First(card => card.Number == CardNumber.Six && card.Suit == resultPile);

            _resultCardPiles[resultPile].Push(sixCard);
            _gameCardPiles[fromPileIndex].Remove(sixCard);
        });

        public void EndGame() => EndGame(EndGameReason.Manual);

        #region Subscribe

        public IDisposable Subscribe(IObserver<GameStarted> observer) => _gameStartedObservers.Add(observer);

        public IDisposable Subscribe(IObserver<StepDone> observer) => _stepDoneObservers.Add(observer);

        public IDisposable Subscribe(IObserver<GameEnded> observer) => _gameEndedObservers.Add(observer);

        #endregion

        private void InitCardPiles()
        {
            _gameCardPiles = Enumerable.Range(0, GamePilesCount)
                .Select(_ => _cardPileFactory.CreateUniqueCardPile(CardsInGamePile))
                .ToArray();

            _resultCardPiles = Enum.GetValues(typeof(CardSuit))
                .Cast<CardSuit>()
                .ToDictionary(suit => suit, suit => _cardPileFactory.CreateSequentialOneSuitCardPile(CardNumbersSequence, suit));
        }

        private void DoStep(Action step)
        {
            try
            {
                step();
                _stepsCount++;

                if (IsWin())
                {
                    EndGame(EndGameReason.Win);
                }
                else if (IsDefeat())
                {
                    EndGame(EndGameReason.Defeat);
                }
                else
                {
                    _stepDoneObservers.OnNext(new StepDone());
                }
            }
            catch (PileFullException e)
            {
                _stepDoneObservers.OnError(new CantDoStepException("Принимающая стопка заполнена.", e));
            }
            catch (PileEmptyException e)
            {
                _stepDoneObservers.OnError(new CantDoStepException("Стопка пуста.", e));
            }
            catch (IncompatibleCardSuitException e)
            {
                _stepDoneObservers.OnError(new CantDoStepException("Стопка не принимает такую масть.", e));
            }
            catch (IncompatibleCardNumberException e)
            {
                _stepDoneObservers.OnError(new CantDoStepException("Стопка не принимает такой номер карты в данный момент.", e));
            }
            catch (CantDoStepException e)
            {
                _stepDoneObservers.OnError(e);
            }
        }

        private void MoveLastCard(IUniqueCardPile fromPile, IUniqueCardPile toPile)
        {
            var moveCard = fromPile.Peek();
            toPile.Push(moveCard);
            fromPile.Remove(moveCard);
        }

        private bool IsWin() => _gameCardPiles.All(cardPile => cardPile.Count == 0);

        private bool IsDefeat()
        {
            var notEmptyCardPiles = _gameCardPiles.Where(cardPile => cardPile.Count > 0).ToArray();
            var notFullCardPiles = notEmptyCardPiles.Where(cardPile => cardPile.Count < cardPile.MaxCardsInPile).ToArray();
            var resultPileNextCards = _resultCardPiles
                .Where(pair => pair.Value.Count < pair.Value.MaxCardsInPile)
                .Select(pair => new Card(pair.Key, pair.Value.NextNumber));

            return _stepsCount > 0
                   && notEmptyCardPiles.All(cardPile => !resultPileNextCards.Contains(cardPile.Peek()))
                   && notEmptyCardPiles.All(notEmpty =>
                       notFullCardPiles.Where(notFull => notFull != notEmpty).All(notFull => notEmpty.Peek().Number != notFull.Peek().Number));
        }

        private void EndGame(EndGameReason endGameReason)
        {
            _gameIsRunning = false;
            _gameEndedObservers.OnNext(new GameEnded
            {
                EndGameReason = endGameReason
            });
        }

        private class InternalGameState : IGameState
        {
            private readonly SolitaireGameModel _gameModel;

            public IReadOnlyList<IReadOnlyUniqueCardPile> GameCardPiles => _gameModel._gameCardPiles;

            public IReadOnlyDictionary<CardSuit, IReadOnlySequentialOneSuitCardPile> ResultGamePiles =>
                _gameModel._resultCardPiles.ToDictionary(x => x.Key, x => (IReadOnlySequentialOneSuitCardPile)x.Value);

            public int StepsCount => _gameModel._stepsCount;

            public InternalGameState(SolitaireGameModel gameModel)
            {
                if (gameModel == null)
                {
                    throw new ArgumentNullException(nameof(gameModel));
                }

                _gameModel = gameModel;
            }
        }
    }
}
