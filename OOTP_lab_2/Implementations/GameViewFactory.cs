using System;
using Microsoft.Extensions.DependencyInjection;
using OOTP_lab_2.Abstractions;
using OOTP_lab_2.Objects;

namespace OOTP_lab_2.Implementations
{
    public class GameViewFactory : IGameViewFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public GameViewFactory(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            _serviceProvider = serviceProvider;
        }

        public IGameView Create(IGameController gameController, ISolitaireGameModel gameModel) => 
            new ConsoleGameView(gameController, gameModel, _serviceProvider.GetRequiredService<IStringViewProvider<Card>>());
    }
}
