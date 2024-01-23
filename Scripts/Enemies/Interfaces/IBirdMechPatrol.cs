using System;

public interface IBirdMechPatrol : IPatrol
{
    public event Action OnAscend;
    public event Action OnDescend;
}
