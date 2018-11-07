using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float lifeTime;
    [SerializeField] private int damage;
    [SerializeField] private LayerMask whatIsSolid;
    [SerializeField] private LayerMask whatToDamage;

    [SerializeField] private GameObject destroyEffect;
    [SerializeField] private float destroyEffectLifeTime;
    
    private void Start() { Invoke("DestroyProjectile", lifeTime); }

    private void OnTriggerEnter2D(Collider2D collision) {
        if ((whatIsSolid.value & 1 << collision.gameObject.layer) != 0) {
            if ((whatToDamage.value & 1 << collision.gameObject.layer) != 0) {
                Debug.Log("Dealing " + damage + " points of damage to " + collision.gameObject);
                collision.gameObject.GetComponentInParent<Entity>().TakeDamage(damage);
            }
            DestroyProjectile();
        }
    }

    private void FixedUpdate() {
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
    }

    public void DestroyProjectile() {
        if (destroyEffect != null) {
            GameObject eff = Instantiate(destroyEffect, transform.position, Quaternion.identity);
            Destroy(eff, destroyEffectLifeTime);
        }
        Destroy(gameObject);
    }
}
