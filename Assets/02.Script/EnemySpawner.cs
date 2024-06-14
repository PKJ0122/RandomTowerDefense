using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    static LinkedList<Vector3> s_path = new LinkedList<Vector3>(); //���Ͱ� �̵��ؾ��� ��������迭
    public static LinkedList<Vector3> Path => s_path; 

    WaveData _waveDate; //Resources������ �ִ� ���̺굥����


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
    /// ���带 �Ű������� ���� ����
    /// </summary>
    /// <param name="round">���� ����</param>
    private void EnemySpawn(int round)
    {
        int r = (round + 1) % 10;

        if (r == 0)
            StartCoroutine(C_BossEnemySpawn(_waveDate.HpDatas[round]));
        else
            StartCoroutine(C_NomalEnemySpawn(_waveDate.HpDatas[round]));
    }

    YieldInstruction delay = new WaitForSeconds(1f); //�����̿� �� ��ü

    /// <summary>
    /// �Ϲ� ���� ���� ���� �ڷ�ƾ
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
    /// ���� ���� ���� ���� �ڷ�ƾ
    /// </summary>
    IEnumerator C_BossEnemySpawn(float hp)
    {
        // todo => ���� ������
        yield return null;
    }
}
