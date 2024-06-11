using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    const float SPEED = 0.2f;

    LinkedListNode<Vector3> _node;

    float _Hp;
    public float Hp
    {
        set => _Hp = value;
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
    /// 몬스터 스폰 밑 세팅 함수
    /// </summary>
    public void Spawn(float hp)
    {
        _node = EnemySpawner.Path.First;
        transform.position = _node.Value;
        _node = _node.Next;
        Hp = hp;
    }
}
