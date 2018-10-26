using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapScript : MonoBehaviour {

    public int damageHealth;
    public int damageMana;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Entity prm = collision.GetComponentInParent<Entity>();
        if (prm)
        {
            prm.TakeDamage(damageHealth);
            prm.UseMana(damageMana);
        }
    }
}
