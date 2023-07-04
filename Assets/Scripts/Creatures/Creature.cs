using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Creature : MonoBehaviour, IEffectApplicable
{
    [SerializeField] private int maxHp;
    [SerializeField] private int currentHp;
    [SerializeField] private float currentSpeed;

    [SerializeField] private bool isLootable;
    [SerializeField] private Image hpImage;
    private Transform hpTransform;
    private Action onDeath;

    public virtual bool IsItSuitable(string id) { return false; }

    private void Awake()
    {
        onDeath += OnDeath;
        hpTransform = hpImage.transform;
    }

    private void OnDestroy()
    {
        onDeath -= OnDeath;
    }

    private protected void OnChangeHP(int change)
    {
        currentHp = Math.Clamp(currentHp + change, 0, maxHp);
        if (currentHp <= 0)
            onDeath?.Invoke();

        if (hpImage != null)
        {
            hpImage.fillAmount = currentHp / maxHp;
        }
            
    }

    public virtual void OnApplying(float modificator, float duration, DisposableFXType type)
    { 

    }

    private virtual protected void OnDeath()
    {

    }

    private virtual protected void Initialize()
    {

    }

    private virtual protected void Deinitialize()
    {

    }
}
