using DG.Tweening;

public interface ILadderEnemy
{
    public float MovementSpeed { get; }
    public float WaitDuration { get; }
    public Ease MovementEase { get; }
    public Ease RotateEase { get; }
}