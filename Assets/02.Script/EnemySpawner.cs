using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    static LinkedList<Vector3> s_path = new LinkedList<Vector3>(); //몬스터가 이동해야할 경로참조배열
    public static LinkedList<Vector3> Path => s_path;

    WaveData _waveDate; //Resources폴더에 있는 웨이브데이터

    Enemy[] _nomalEnemyPrefab;
    Boss[] _bossEnemyPrefab;
    Boss _goldBossPrefab;

    int nomalEnemycount;
    int bossEnemycount;

    List<Enemy> _enemys = new List<Enemy>(100);


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
    }

    void Start()
    {
        GameManager.Instance.OnWaveChange += value => EnemySpawn(value);
        UIManager.Instance.Get<GoldBossUI>().OnBossSpawnButtonClick += GoldBossSpawn;
        UIManager.Instance.Get<GameEndUI>().OnReStartButtonClick += () =>
        {
            for (int i = _enemys.Count - 1; i >= 0; i--)
            {
                _enemys[i].RelasePool();
            }
            StopAllCoroutines();
        };
    }

    void OnDisable()
    {
        //GameManager.Instance.onWaveChange -= value => EnemySpawn(value);
    }

    /// <summary>
    /// 라운드를 매개변수로 몬스터 스폰
    /// </summary>
    /// <param name="round">현재 라운드</param>
    private void EnemySpawn(int round)
    {
        int r = (round + 1) % 10;

        if (r == 0) BossEnemySpawn(_waveDate.HpDatas[round]);
        else StartCoroutine(C_NomalEnemySpawn(_waveDate.HpDatas[round]));
    }

    YieldInstruction _delay = new WaitForSeconds(1f); //딜레이에 쓸 객체

    /// <summary>
    /// 일반 라운드 몬스터 스폰 코루틴
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
                                                    .Spawn(hp);

            _enemys.Add(enemy);
            enemy.OnRelasePool += () => _enemys.Remove(enemy);

            yield return _delay;
        }
    }

    /// <summary>
    /// 보스 라운드 몬스터 스폰 함수
    /// </summary>
    void BossEnemySpawn(float hp)
    {
        Enemy enemy = ObjectPoolManager.Instance.Get($"{_bossEnemyPrefab[bossEnemycount].name}")
                                                .Get()
                                                .GetComponent<Enemy>()
                                                .Spawn(hp);

        _enemys.Add(enemy);
        enemy.OnRelasePool += () => _enemys.Remove(enemy);

        if (nomalEnemycount++ >= _nomalEnemyPrefab.Length) nomalEnemycount = 0;
        if (bossEnemycount++ >= _bossEnemyPrefab.Length) bossEnemycount = 0;
    }

    void GoldBossSpawn()
    {
        Enemy enemy = ObjectPoolManager.Instance.Get($"{_goldBossPrefab.name}")
                                                .Get()
                                                .GetComponent<Enemy>()
                                                .Spawn(1);

        _enemys.Add(enemy);
        enemy.OnRelasePool += () => _enemys.Remove(enemy);
    }
}
