using System;
using UnityEngine;

[Serializable]
public class ItemEffect
{
    [SerializeField] private float modifier;
    [SerializeField] private float duration;
    public float Modifier => modifier;
    public float Duration => duration;

    public DisposableFXType Type { get; private set; }
    public bool IsMultiplier { get; private set; }
}

public enum DisposableFXType
{
    modifyHp,
    modifySpeed,
    modifyItemState
}
