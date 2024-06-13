using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    const float SPEED = 0.2f;

    LinkedListNode<Vector3> _node;

    float _hp;


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
    }

    /// <summary>
    /// ������ ���� �Լ�
    /// </summary>
    /// <param name="damage">������ ��</param>
    /// <returns>���� �� �������� ��ȯ</returns>
    public float Damage(float damage)
    {
        _hp -= damage;
        return _hp >= 0 ? _hp : damage + _hp;
    }
}
