using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFixable
{
    public bool IsBroken();
    public void ChangeState(float change);
}
