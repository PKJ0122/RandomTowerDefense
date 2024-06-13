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
    /// 몬스터 스폰 밑 세팅 함수
    /// </summary>
    public void Spawn(float hp)
    {
        _node = EnemySpawner.Path.First;
        transform.position = _node.Value;
        _node = _node.Next;
        _hp = hp;
    }

    /// <summary>
    /// 데미지 적용 함수
    /// </summary>
    /// <param name="damage">데미지 값</param>
    /// <returns>실제 준 데미지를 반환</returns>
    public float Damage(float damage)
    {
        _hp -= damage;
        return _hp >= 0 ? _hp : damage + _hp;
    }
}
