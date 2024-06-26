using System;
using UnityEngine;

public class UnitBase : PoolObject
{
    const float ATTACK_RANGE = 22.5f;
    const int MP_RECOVERY_AMOUNT = 10;

    LayerMask _targetMask;
    Animator _animator;

    Slot _slot;
    Enemy _targetEnemy; //���� �������� Ÿ�ٸ��� ����

    UnitKind _Kind;
    UnitRank _Rank;
    float _power;
    float _damage;
    SkillBase _skill;
    int _mp;
    int _skillNeedMp;
    ParticleSystem _particleSystem;

    public UnitKind Kind { get => _Kind; }
    public UnitRank Rank { get => _Rank; }
    public float Power 
    { 
        get => _power; 
        set
        {
            _power = value;
            onPowerChange?.Invoke(value);
        }
    }
    public float Damage
    {
        get => _damage;
        set
        {
            _damage = value;
            onDamageChange?.Invoke(value);
        }
    }
    public int Mp
    {
        get => _mp;
        set
        {
            _mp = value;
            onMpChange?.Invoke(value,_skillNeedMp);
        }
    }

    public event Action<float> onPowerChange;
    public event Action<float> onDamageChange;
    public event Action<int,int> onMpChange;
    public event Action onDisable;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _particleSystem = transform.Find("Rank").GetComponent<ParticleSystem>();
        _targetMask = LayerMask.GetMask("Enemy");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ATTACK_RANGE);
    }

    void Update()
    {
        _animator.SetBool("IsTargeting", _targetEnemy != null);

        Collider[] targets = Physics.OverlapSphere(transform.position, ATTACK_RANGE, _targetMask);

        if (targets.Length == 0)
        {
            _targetEnemy = null;
            return;
        }

        foreach (Collider target in targets)
        {
            if (_targetEnemy == null)
            {
                _targetEnemy = target.GetComponent<Enemy>();
                continue;
            }
            if (Vector3.SqrMagnitude(_targetEnemy.transform.position - transform.position) >
                Vector3.SqrMagnitude(target.transform.position - transform.position))
            //���� Ÿ������ ��ϵ� ���ͺ��� �� ����� ������ �ִ��� üũ�ϰ� �ִٸ� Ÿ�� ����
            {
                _targetEnemy = target.GetComponent<Enemy>();
            }
        }
    }

    /// <summary>
    /// ���� ���� �Լ�
    /// </summary>
    public UnitBase UnitSet(Slot slot, UnitKind kind, UnitRank rank)
    {
        _slot = slot;
        _Kind = kind;
        _Rank = rank;
        UnitData data = UnitRepository.UnitKindDatas[kind];
        _power = data.unitPowerDatas[(int)rank];
        _skill = data.skill;
        _skillNeedMp = data.skill.needMp;
        Mp = 0;
        ParticleSystem.MainModule main = _particleSystem.main;
        main.startColor = UnitRepository.UnitRankDatas[rank].unitRankColor;
        Damage = 0;

        return this;
    }

    /// <summary>
    /// ������ ����(������) 0���� �������ִ� �Լ�
    /// </summary>
    public void DamageReSet()
    {
        Damage = 0;
    }

    /// <summary>
    /// ���� �����Լ� / ȣ���� �ִϸ��̼� Ŭ������ ȣ��
    /// </summary>
    void Attack()
    {
        if (_targetEnemy == null)
            return;

        transform.LookAt(_targetEnemy.transform);

        if (_mp >= _skillNeedMp)
        {
            Skill();
            return;
        }

        Mp += MP_RECOVERY_AMOUNT;
        Damage += _targetEnemy.Damage(_power);
    }

    void Skill()
    {
        Mp = 0;
        _skill.Skill(this, _targetEnemy, _power);
    }

    public override void RelasePool()
    {
        onDisable?.Invoke();
        base.RelasePool();
        GameManager.Instance.Units[Kind].Remove(_slot);
    }
}
