using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRotate : MonoBehaviour
{
    public Transform tf_Bullet;
    public Transform tf_Target;

    // private void Update()
    // {
    //     tf_Bullet.LookAt2D(tf_Target.position - tf_Bullet.position);
    // }

    [Sirenix.OdinInspector.Button]
    public void Rotate()
    {
        tf_Bullet.RotateLerp(tf_Target, 5f);
    }
}
