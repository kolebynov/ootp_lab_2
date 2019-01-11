namespace OOTP_lab_2.Abstractions
{
    public interface IGameViewFactory
    {
        IGameView Create(IGameController gameController, ISolitaireGameModel gameModel);
    }
}
