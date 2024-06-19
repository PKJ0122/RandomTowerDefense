using System.Collections.Generic;
using UnityEngine;

public class Enemy : PoolObject
{
    const float SPEED = 0.2f;

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


    void FixedUpdate()
    {
        Vector3 direction = (_node.Value - transform.position).normalized;
        transform.position += direction * SPEED;
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
        _hp = hp;
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


    void Die()
    {
        GameManager.Instance.EnemyAmount--;
        RelasePool();
    }
}
