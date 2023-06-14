using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffectApplicable
{
    public void OnApplying(float modificator, float duration, DisposableFXType type);

    public bool IsItSuitable(string id);
}
