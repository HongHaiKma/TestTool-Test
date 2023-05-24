using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HellTap.PoolKit;

public class TestPool : Singleton<TestPool>
{
    public Pool m_Pool;
    public Spawner m_Spawner;
    public GameObject Square;

    public LazyLoadReference<GameObject> square;

    [Sirenix.OdinInspector.Button]
    public void TestSpawn()
    {
        m_Spawner.Play();
    }

    [Sirenix.OdinInspector.Button]
    public void Spawn()
    {
        // m_Spawner.Spawn(Square);
        Transform myInstance = m_Pool.Spawn("Square");
        // Transform myInstance = m_Pool.Spawn(Square, Vector3.zero, Vector3.zero, null);
        // Transform myInstance = m_Pool.Spawn(square);
    }
}
