using DG.Tweening;

public interface IMovingEnemy
{
    public float MovementSpeed { get; }
    public float WaitDuration { get; }
    public Ease MovementEase { get; }
}