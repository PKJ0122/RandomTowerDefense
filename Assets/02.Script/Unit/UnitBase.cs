using System;
using UnityEngine;

public class UnitBase : PoolObject
{
    protected const float ATTACK_RANGE = 22.5f;
    protected const int MP_RECOVERY_AMOUNT = 10;

    protected LayerMask _targetMask;
    protected Animator _animator;

    protected Slot _slot;
    protected Enemy _targetEnemy; //현재 공격중인 타겟몬스터 참조

    protected UnitKind _Kind;
    protected UnitRank _Rank;
    protected float _power;
    protected float _damage;
    protected SkillBase _skill;
    protected int _mp;
    protected int _skillNeedMp;
    protected ParticleSystem _particleSystem;

    public UnitKind Kind { get => _Kind; }
    public UnitRank Rank { get => _Rank; }
    public Slot Slot
    {
        get => _slot;
        set
        {
            _slot = value;
            transform.position = _slot.transform.position;
            SlotManager.Slots[_slot] = this;
        }
    }
    public virtual float Power 
    { 
        get => _power; 
        set
        {
            _power = value;
            OnPowerChange?.Invoke(value);
        }
    }
    public float Damage
    {
        get => _damage;
        set
        {
            _damage = value;
            OnDamageChange?.Invoke(value);
        }
    }
    public int Mp
    {
        get => _mp;
        set
        {
            _mp = value;
            OnMpChange?.Invoke(value,_skillNeedMp);
        }
    }

    public Action<float> OnPowerChange;
    public Action<float> OnDamageChange;
    public Action<int,int> OnMpChange;
    public Action OnDisable;


    protected virtual void Awake()
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
            Enemy enemy = target.GetComponent<Enemy>();

            if (_targetEnemy == null)
            {
                _targetEnemy = enemy;
                continue;
            }

            if (_targetEnemy.Priority < enemy.Priority)
            {
                _targetEnemy = enemy;
                break;
            }

            if (Vector3.SqrMagnitude(_targetEnemy.transform.position - transform.position) >
                Vector3.SqrMagnitude(target.transform.position - transform.position))
            //현재 타겟으로 등록된 몬스터보다 더 가까운 유닛이 있는지 체크하고 있다면 타겟 변경
            {
                _targetEnemy = target.GetComponent<Enemy>();
            }
        }

        transform.LookAt(_targetEnemy.transform);
    }

    /// <summary>
    /// 유닛 세팅 함수
    /// </summary>
    public virtual UnitBase UnitSet(Slot slot, UnitKind kind, UnitRank rank)
    {
        Slot = slot;
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
    /// 유닛의 딜량(데미지) 0으로 리셋해주는 함숨
    /// </summary>
    public void DamageReSet()
    {
        Damage = 0;
    }

    /// <summary>
    /// 유닛 공격함수 / 호출은 애니메이션 클립에서 호출
    /// </summary>
    protected void Attack()
    {
        if (_targetEnemy == null)
            return;

        if (Mp >= _skillNeedMp)
        {
            Skill();
            return;
        }

        Mp += MP_RECOVERY_AMOUNT;
        Damage += _targetEnemy.Damage(Power);
    }

    protected void Skill()
    {
        Mp = 0;
        _skill.Skill(this, _targetEnemy, Power);
    }

    public override void RelasePool()
    {
        OnDisable?.Invoke();
        OnDisable = null;
        GameManager.Instance.Units[Kind].Remove(this);
        SlotManager.Slots[Slot] = null;
        base.RelasePool();
    }
}