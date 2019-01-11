namespace OOTP_lab_2.Abstractions
{
    public interface IGameView
    {
        bool IsStartGameOptionVisible { get; set; }

        bool IsEndGameOptionVisible { get; set; }
    }
}
