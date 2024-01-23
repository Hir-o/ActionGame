using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDashingInterface
{
    public event Action EnemyBotOnIdle;
    public event Action EnemyBotOnDashing;
}
