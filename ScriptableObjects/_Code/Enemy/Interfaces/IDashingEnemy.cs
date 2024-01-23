using DG.Tweening;

public interface IDashingEnemy
{
    public float MovementSpeed { get; }
    public float DashingSpeed { get; }
    public Ease MovementEase { get; }
    public Ease DashingEase { get; }
    public float DashingTriggerDistance { get; }

}