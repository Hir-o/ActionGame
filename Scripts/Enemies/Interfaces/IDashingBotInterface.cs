using System;


public interface IDashingBotInterface : IPatrol
{
    public event Action DashingBotOnIdle;
    public event Action DashingBotOnAttack;
}
