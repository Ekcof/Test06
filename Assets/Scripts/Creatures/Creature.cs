using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour, IEffectApplicable
{
    [SerializeField] private int maxHp;
    [SerializeField] private float currentHp;
    [SerializeField] private float currentSpeed;

    private Inventory inventory;
    [SerializeField] private bool isLootable;
    [SerializeField] private SpriteRenderer hpRenderer;
    private Transform hpTransform;

    private void Awake()
    {
        hpTransform = hpRenderer.transform;
    }

    private protected void OnGetDammage()
    {

    }

    public virtual void OnApplying(float modificator, float duration, DisposableFXType type)
    { 

    }

    public virtual bool IsItSuitable(string id) { return false; }
}
