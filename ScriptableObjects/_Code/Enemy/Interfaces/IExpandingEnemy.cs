using DG.Tweening;

public interface IExpandingEnemy
{
    public float StartScale { get; }
    public float EndScale { get; }
    public float ScaleSpeed { get; }
    public Ease ScaleEase { get; }
}
