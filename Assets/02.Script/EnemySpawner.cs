using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    /// <summary>
    /// 몬스터가 이동해야할 경로참조배열
    /// </summary>
    static Vector3[] s_path;
    public static Vector3[] Path => s_path; 

    private void Awake()
    {
        s_path = new Vector3[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            s_path[i] = transform.GetChild(i).position;
        }
    }
}
