using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    static LinkedList<Vector3> s_path = new LinkedList<Vector3>(); //몬스터가 이동해야할 경로참조배열
    public static LinkedList<Vector3> Path => s_path; 

    WaveData _waveDate; //Resources폴더에 있는 웨이브데이터


    void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            s_path.AddLast(transform.GetChild(i).position);
        }

        _waveDate = Resources.Load<WaveData>("WaveData");

        GameManager.Instance.onWaveChange += value => EnemySpawn(value);
    }

    void OnDisable()
    {
        GameManager.Instance.onWaveChange -= value => EnemySpawn(value);
    }

    /// <summary>
    /// 라운드를 매개변수로 몬스터 스폰
    /// </summary>
    /// <param name="round">현재 라운드</param>
    private void EnemySpawn(int round)
    {
        int r = (round + 1) % 10;

        if (r == 0)
            StartCoroutine(C_BossEnemySpawn(_waveDate.HpDatas[round]));
        else
            StartCoroutine(C_NomalEnemySpawn(_waveDate.HpDatas[round]));
    }

    YieldInstruction delay = new WaitForSeconds(1f); //딜레이에 쓸 객체

    /// <summary>
    /// 일반 라운드 몬스터 스폰 코루틴
    /// </summary>
    IEnumerator C_NomalEnemySpawn(float hp)
    {
        int count = 0;
        GameObject go = Resources.Load<GameObject>("Enemy");

        while (count < 40)
        {
            count++;
            Instantiate(go).GetComponent<Enemy>().Spawn(hp);
            yield return delay;
        }
    }

    /// <summary>
    /// 보스 라운드 몬스터 스폰 코루틴
    /// </summary>
    IEnumerator C_BossEnemySpawn(float hp)
    {
        // todo => 보스 리스폰
        yield return null;
    }
}
