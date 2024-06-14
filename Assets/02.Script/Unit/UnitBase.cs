using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    const float ATTACK_RANGE = 22.5f;

    LayerMask _targetMask;
    Animator _animator;

    Enemy _targetEnemy; //���� �������� Ÿ�ٸ��� ����

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
                //���� Ÿ������ ��ϵ� ���ͺ��� �� ����� ������ �ִ��� üũ�ϰ� �ִٸ� Ÿ�� ����
            {
                _targetEnemy = target.GetComponent<Enemy>();
            }
            transform.LookAt(_targetEnemy.transform);
        }
    }

    /// <summary>
    /// ���� ���� �Լ�
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

        _damage += _targetEnemy.Damage(_power);
    }
}
