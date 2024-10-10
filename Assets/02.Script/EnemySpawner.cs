using System;
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

    List<Enemy> _enemys = new List<Enemy>(100);

    BossHpBar _bossHpBarPrefab;
    public static event Action<Boss> OnBossSpawn;


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
            ObjectPoolManager.Instance.CreatePool($"{prefab.name}", prefab, 99);
        }
        foreach (Boss prefab in _bossEnemyPrefab)
        {
            ObjectPoolManager.Instance.CreatePool($"{prefab.name}", prefab, 2);
        }
        ObjectPoolManager.Instance.CreatePool($"{_goldBossPrefab.name}", _goldBossPrefab, 2);
        GameManager.Instance.OnGameEnd += v =>
        {
            nomalEnemycount = 0;
            bossEnemycount = 0;
        };

        _bossHpBarPrefab = Resources.Load<BossHpBar>("Canvas - BossHp");
        ObjectPoolManager.Instance.CreatePool("BossHPBar", _bossHpBarPrefab, 2);
        OnBossSpawn += boss =>
        {
            ObjectPoolManager.Instance.Get("BossHPBar")
                                      .Get()
                                      .GetComponent<BossHpBar>()
                                      .Show(boss);
        };
    }

    void Start()
    {
        GameManager.Instance.OnWaveChange += value => EnemySpawn(value);
        UIManager.Instance.Get<GoldBossUI>().OnBossSpawnButtonClick += GoldBossSpawn;
        GameManager.Instance.OnGameEnd += v =>
        {
            StopAllCoroutines();
        };
    }

    private void OnDestroy()
    {
        OnBossSpawn = null;
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
            Enemy enemy = ObjectPoolManager.Instance.Get($"{_nomalEnemyPrefab[nomalEnemycount].name}")
                                                    .Get()
                                                    .GetComponent<Enemy>()
                                                    .Spawn(1);

            _enemys.Add(enemy);
            enemy.OnRelasePool += () => _enemys.Remove(enemy);

            yield return _delay;
        }
    }

    /// <summary>
    /// ���� ���� ���� ���� �Լ�
    /// </summary>
    void BossEnemySpawn(float hp)
    {
        Enemy enemy = ObjectPoolManager.Instance.Get($"{_bossEnemyPrefab[bossEnemycount].name}")
                                                .Get()
                                                .GetComponent<Enemy>()
                                                .Spawn(1);

        _enemys.Add(enemy);
        enemy.OnRelasePool += () => _enemys.Remove(enemy);
        Boss boss = (Boss)enemy;
        OnBossSpawn?.Invoke(boss);

        if (nomalEnemycount++ >= _nomalEnemyPrefab.Length) nomalEnemycount = 0;
        if (bossEnemycount++ >= _bossEnemyPrefab.Length) bossEnemycount = 0;
    }

    void GoldBossSpawn()
    {
        int wave = GameManager.Instance.Wave + 1;

        int start = wave / 10;

        if (start == 5) return;

        int now = Mathf.Max(1, wave % 10);

        int startHp = start == 0 ? 0 : start * 10 - 1;
        int endHp = start == 0 ? 9 : start + 10;

        float hp = Mathf.Lerp(_waveDate.HpDatas[startHp], _waveDate.HpDatas[endHp], (float)now / 10f) * 0.65f;

        Enemy enemy = ObjectPoolManager.Instance.Get($"{_goldBossPrefab.name}")
                                                .Get()
                                                .GetComponent<Enemy>()
                                                .Spawn(hp);

        _enemys.Add(enemy);
        enemy.OnRelasePool += () => _enemys.Remove(enemy);
        OnBossSpawn?.Invoke((Boss)enemy);
    }
}
