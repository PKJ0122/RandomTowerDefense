using System.Collections.Generic;
using UnityEngine;

public class Enemy : PoolObject
{
    const float DEFAULT_SPEED = 0.2f;

    LinkedListNode<Vector3> _node;

    float _hp;
    public float Hp
    {
        get => _hp;
        set
        {
            _hp = value;
            if (_hp <= 0)
                Die();
        }
    }

    float _speed;

    public float Speed
    {
        get => _speed;
        set
        {
            _speed = value;
        }
    }

    public int Priority { get; protected set; }


    void FixedUpdate()
    {
        Vector3 direction = (_node.Value - transform.position).normalized;
        transform.position += direction * Speed;
        transform.LookAt(_node.Value);

        if (Vector3.Distance(transform.position, _node.Value) <= 0.2)
        {
            _node = _node == EnemySpawner.Path.Last ? EnemySpawner.Path.First : _node.Next;
        }
    }

    /// <summary>
    /// ���� ���� �� ���� �Լ�
    /// </summary>
    public void Spawn(float hp)
    {
        _node = EnemySpawner.Path.First;
        transform.position = _node.Value;
        _node = _node.Next;
        Hp = hp;
        Speed = DEFAULT_SPEED;
        GameManager.Instance.EnemyAmount++;
    }

    /// <summary>
    /// ������ ���� �Լ�
    /// </summary>
    /// <param name="damage">������ ��</param>
    /// <returns>���� �� �������� ��ȯ</returns>
    public float Damage(float damage)
    {
        if (Hp <= 0) return 0;

        Hp -= damage;
        return Hp >= 0 ? damage : damage + Hp;
    }


    protected virtual void Die()
    {
        GameManager.Instance.EnemyAmount--;
        GameManager.Instance.Gold++;
        transform.position = Vector3.zero;
        RelasePool();
    }
}
