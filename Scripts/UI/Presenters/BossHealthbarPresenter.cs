
using System;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class BossHealthbarPresenter : MonoBehaviour
{
    [SerializeField] private Image _bossHealthImage;

    [BoxGroup("Tweening"), SerializeField] private float _fillSpeed = 3f;
    [BoxGroup("Tweening"), SerializeField] private Ease _fillEase = Ease.Linear;

    private Tween _tweenFillAmount;

    private void Awake()
    {
        Assert.IsNotNull(_bossHealthImage,
            "Variable _bossHealthImage in BossHealthbarPresenter.cs is unassigned. Assign it in the editor on the Canvas_Boss gameobject.");
    }

    private void OnEnable()
    {
        if (BaseBoss.Instance != null && BaseBoss.Instance is IDamageable boss)
            boss.OnHealthChanged += SecondBoss_OnHealthChanged;
    }

    private void OnDisable()
    {
        if (BaseBoss.Instance != null && BaseBoss.Instance is IDamageable boss)
            boss.OnHealthChanged -= SecondBoss_OnHealthChanged;
    }

    private void SecondBoss_OnHealthChanged(int currHealth, int maxHealth)
    {
        float healthPercentage = (float)currHealth / maxHealth;

        if (_tweenFillAmount != null) _tweenFillAmount.Kill();
        if (currHealth == maxHealth)
        {
            _bossHealthImage.fillAmount = healthPercentage;
            return;
        }
        _tweenFillAmount = _bossHealthImage
            .DOFillAmount(healthPercentage, _fillSpeed)
            .SetSpeedBased()
            .SetEase(_fillEase);
    }
}