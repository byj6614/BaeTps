using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension
{
   public static bool Isvaild(this GameObject go)
    {
        return go != null && go.activeInHierarchy;
    }

    public static bool IsVaild(this Component component)
    {
        return component != null && component.gameObject.activeInHierarchy;
    }
}
