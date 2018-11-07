using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTrap : MonoBehaviour {

    public int damageHealth;
    public int damageMana;

    private void OnTriggerEnter2D(Collider2D collision) {
        Entity prm = collision.GetComponentInParent<Entity>();
        if (prm != null) {
            prm.TakeDamage(damageHealth);
            prm.UseMana(damageMana);
        }
    }
}
