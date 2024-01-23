public class FirstBoss : BaseBoss
{
    private BossDashTweener _bossDashTweener;

    protected override void Awake()
    {
        base.Awake();
        _bossDashTweener = GetComponent<BossDashTweener>();
    }
}