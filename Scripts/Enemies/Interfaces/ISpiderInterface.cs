using System;


public interface ISpiderInterface : IPatrol
{
    public event Action SpiderOnIdle;
    public event Action SpiderOnAttack;
}
