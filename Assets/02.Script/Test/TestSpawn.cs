using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawn : MonoBehaviour
{
    IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            Enemy enemy = ObjectPoolManager.Instance.Get("Enemy-Slime")
                                        .Get()
                                        .GetComponent<Enemy>()
                                        .Spawn(float.MaxValue);
        }
    }
}
