using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HellTap.PoolKit;

public class Square : MonoBehaviour
{
    [Sirenix.OdinInspector.Button]
    public void Despawn()
    {
        // TestPool.Instance.m_Pool.Despawn(transform);
        Debug.Log(PoolKit.GetPoolContainingInstance(transform).name);
    }
}
