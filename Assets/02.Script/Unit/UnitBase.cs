using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    const float ATTACK_RANGE = 22.5f;

    LayerMask _targetMask;
    Animator _animator;

    Enemy _targetEnemy; //현재 공격중인 타겟몬스터 참조

    UnitKind _Kind;
    UnitRank _Rank;
    float _power;
    float _damage;
    ParticleSystem _particleSystem;

    public UnitKind Kind { get => _Kind;}
    public UnitRank Rank { get => _Rank;}
    public float Power { get => _power;}
    public float Damage
    {
        get => _damage;
        set
        {
            _damage = value;
            onDamageChange?.Invoke(_damage);
        }
    }

    public event Action<float> onDamageChange;


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

        if (targets == null)
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
                //현재 타겟으로 등록된 몬스터보다 더 가까운 유닛이 있는지 체크하고 있다면 타겟 변경
            {
                _targetEnemy = target.GetComponent<Enemy>();
            }
            transform.LookAt(_targetEnemy.transform);
        }
    }

    /// <summary>
    /// 유닛 세팅 함수
    /// </summary>
    public void UnitSet(UnitKind kind,UnitRank rank)
    {
        _Kind = kind;
        _Rank = rank;
        //_power = UnitRepository.UnitKindDatas[(kind, rank)];
        ParticleSystem.MainModule main = _particleSystem.main;
        //main.startColor = UnitRepository.UnitRankDatas[rank];
        _damage = 0;
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
    void Attack()
    {
        if (_targetEnemy == null)
            return;

        _damage += _targetEnemy.Damage(_power);
    }
}
