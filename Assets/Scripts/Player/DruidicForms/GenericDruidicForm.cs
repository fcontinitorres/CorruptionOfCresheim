using UnityEngine;
using System.Collections;

public abstract class GenericDruidicForm : MonoBehaviour
{
    // Variables so the updates don't call GetComponentInParent all the time
    protected PlayerController controller;
    protected PlayerInputManager inputManager;
    protected Entity entity;
    protected Animator animator;

    // Effect to be called when transforming to this form
    [SerializeField] private GameObject transformEffect;

    [SerializeField] protected int health_max;      // Maximum health
    [SerializeField] protected int manaCost;        // Mana cost to transform to this form

    [SerializeField] protected float jumpSpeed;     // Y speed when jumping
    [SerializeField] protected float airControl;    // Percentage of movement in the air
    [SerializeField] protected float runSpeed;      // Maximum running speed
    [SerializeField] protected float dashForce;     // Amount of force applyed on dash

    protected virtual void Awake() {
        // Getting variables
        controller = GetComponentInParent<PlayerController>();
        inputManager = GetComponentInParent<PlayerInputManager>();
        entity = GetComponentInParent<Entity>();
        animator = GetComponentInParent<Animator>();

        OnDisable();
    }

    protected virtual void OnEnable() {
        //Enabling all child components
        foreach (Collider2D child in GetComponents<Collider2D>()) child.enabled = true;
        foreach (Transform child in gameObject.transform) child.gameObject.SetActive(true);

        //Setting jump speed, air control, run speed, dash force, max health and reducing mana
        controller.SetJumpSpeed(jumpSpeed);
        controller.SetAirControl(airControl);
        controller.SetRunSpeed(runSpeed);
        controller.SetDashForce(dashForce);
        entity.SetHealthMax(health_max);
        entity.UseMana(manaCost);

        // Creating the transform effect, and destroying it after it's finished
        GameObject anim = Instantiate(transformEffect, transform.position, Quaternion.identity);
        Destroy(anim, anim.GetComponentInParent<Animator>().runtimeAnimatorController.animationClips[0].length);
    }

    protected virtual void OnDisable() {
        //Disabling all child components
        foreach (Transform child in gameObject.transform) child.gameObject.SetActive(false);
        foreach (Collider2D child in GetComponents<Collider2D>()) child.enabled = false;
    }

    public bool CanBeTransformed() { return entity.HasMana(manaCost); }

    private void FixedUpdate() { Move(); }

    // Method to receive input and to interpret them
    public abstract void Move();
}
