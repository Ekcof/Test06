using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Ammo : Item
{
    [SerializeField] private Weapon[] suitableWeapons;
    [SerializeField] private bool hasBulletCase;
    [SerializeField] private Image bulletCase;
}
