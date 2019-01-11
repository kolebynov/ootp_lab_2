using System;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using OOTP_lab_2.Abstractions;
using OOTP_lab_2.Implementations;
using OOTP_lab_2.Implementations.StringViewProviders;
using OOTP_lab_2.Objects;

namespace OOTP_lab_2
{
    class Program
    {
        public static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            InitServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var gameController = serviceProvider.GetRequiredService<IGameController>();
            gameController.StartGame();
        }

        private static void InitServices(IServiceCollection services)
        {
            services.AddTransient<ICardDeck, CardDeck>();
            services.AddTransient<IGameView, ConsoleGameView>();
            services.AddTransient(typeof(IObserversCollection<>), typeof(ObserversCollection<>));
            services.AddTransient<ISolitaireGameModel, SolitaireGameModel>();
            services.AddTransient<IGameController, GameController>();

            services.AddSingleton<ICardPileFactory, CardPileFactory>();
            services.AddSingleton<IGameViewFactory, GameViewFactory>();
            services.AddSingleton<IStringViewProvider<CardNumber>, CardNumberStringViewProvider>();
            services.AddSingleton<IStringViewProvider<CardSuit>, CardSuitStringViewProvider>();
            services.AddSingleton<IStringViewProvider<Card>, CardStringViewProvider>();
        }
    }
}
