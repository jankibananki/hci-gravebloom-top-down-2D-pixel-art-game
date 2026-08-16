using System.Collections.Generic;
using UnityEngine;

public class UltimateBeamDamage : MonoBehaviour
{
    public int damage = 5;

    private HashSet<EnemyHealth> hitEnemies =
        new HashSet<EnemyHealth>();

    void OnTriggerEnter2D(Collider2D other)
    {
        TryDamageEnemy(other);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        TryDamageEnemy(other);
    }

    void TryDamageEnemy(Collider2D other)
    {
        EnemyHealth enemy =
            other.GetComponentInParent<EnemyHealth>();

        if (enemy == null)
            return;

        if (hitEnemies.Contains(enemy))
            return;

        hitEnemies.Add(enemy);

        Debug.Log("ULTI HIT ENEMY!");

        enemy.TakeDamage(damage);
    }
}