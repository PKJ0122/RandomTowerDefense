using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    static LinkedList<Vector3> s_path = new LinkedList<Vector3>(); //���Ͱ� �̵��ؾ��� ��������迭
    public static LinkedList<Vector3> Path => s_path;

    WaveData _waveDate; //Resources������ �ִ� ���̺굥����

    Enemy[] _nomalEnemyPrefab;
    Boss[] _bossEnemyPrefab;
    Boss _goldBossPrefab;

    int nomalEnemycount;
    int bossEnemycount;


    void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            s_path.AddLast(transform.GetChild(i).position);
        }
        _waveDate = Resources.Load<WaveData>("WaveData");
        _nomalEnemyPrefab = _waveDate.nomalEnemyPrefab;
        _bossEnemyPrefab = _waveDate.bossEnemyPrefab;
        _goldBossPrefab = _waveDate.goldBossPrefab;
        foreach (Enemy prefab in _nomalEnemyPrefab)
        {
            ObjectPoolManager.Instance.CreatePool($"{prefab.name}", prefab,99);
        }
        foreach (Boss prefab in _bossEnemyPrefab)
        {
            ObjectPoolManager.Instance.CreatePool($"{prefab.name}", prefab, 2);
        }
        ObjectPoolManager.Instance.CreatePool($"{_goldBossPrefab.name}", _goldBossPrefab, 2);
    }

    void Start()
    {
        GameManager.Instance.OnWaveChange += value => EnemySpawn(value);
        UIManager.Instance.Get<GoldBossUI>().OnBossSpawnButtonClick += GoldBossSpawn;
    }

    void OnDisable()
    {
        //GameManager.Instance.onWaveChange -= value => EnemySpawn(value);
    }

    /// <summary>
    /// ���带 �Ű������� ���� ����
    /// </summary>
    /// <param name="round">���� ����</param>
    private void EnemySpawn(int round)
    {
        int r = (round + 1) % 10;

        if (r == 0) BossEnemySpawn(_waveDate.HpDatas[round]);
        else StartCoroutine(C_NomalEnemySpawn(_waveDate.HpDatas[round]));
    }

    YieldInstruction _delay = new WaitForSeconds(1f); //�����̿� �� ��ü

    /// <summary>
    /// �Ϲ� ���� ���� ���� �ڷ�ƾ
    /// </summary>
    IEnumerator C_NomalEnemySpawn(float hp)
    {
        int count = 0;
        
        while (count < 40)
        {
            count++;
            ObjectPoolManager.Instance.Get($"{_nomalEnemyPrefab[nomalEnemycount].name}")
                                      .Get()
                                      .GetComponent<Enemy>()
                                      .Spawn(hp);
            yield return _delay;
        }
    }

    /// <summary>
    /// ���� ���� ���� ���� �Լ�
    /// </summary>
    void BossEnemySpawn(float hp)
    {
        ObjectPoolManager.Instance.Get($"{_bossEnemyPrefab[bossEnemycount].name}")
                          .Get()
                          .GetComponent<Enemy>()
                          .Spawn(hp);

        if (nomalEnemycount++ >= _nomalEnemyPrefab.Length) nomalEnemycount = 0;
        if (bossEnemycount++ >= _bossEnemyPrefab.Length) bossEnemycount = 0;
    }

    void GoldBossSpawn()
    {
        ObjectPoolManager.Instance.Get($"{_goldBossPrefab.name}")
                                  .Get()
                                  .GetComponent<Enemy>()
                                  .Spawn(1);
    }
}
