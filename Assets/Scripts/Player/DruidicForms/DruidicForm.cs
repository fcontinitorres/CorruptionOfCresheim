using UnityEngine;
using System.Collections;

public abstract class DruidicForm : MonoBehaviour
{
    protected PlayerController controller;
    protected PlayerInputManager inputManager;
    protected PlayerResourceManager resourceManager;
    protected Animator animator;

    [SerializeField] private GameObject transformEffect;

    [SerializeField] protected int health_max;      // Maximum health

    [SerializeField] protected float jumpSpeed;     // Y speed when jumping
    [SerializeField] protected float airControl;    // Percentage of movement in the air
    [SerializeField] protected float runSpeed;      // Maximum running speed
    [SerializeField] protected float dashForce;       // Amount of force applyed on dash
    [SerializeField] protected int manaCost;        // Mana cost to transform to this form

    private void Awake()
    {
        controller = GetComponentInParent<PlayerController>();
        inputManager = GetComponentInParent<PlayerInputManager>();
        resourceManager = GetComponentInParent<PlayerResourceManager>();
        animator = GetComponentInParent<Animator>();

        OnDisable();
    }

    void OnEnable()
    {
        //Setting jump speed, air control, run speed, dash force, max health and reducing mana
        controller.SetJumpSpeed(jumpSpeed);
        controller.SetAirControl(airControl);
        controller.SetRunSpeed(runSpeed);
        controller.SetDashForce(dashForce);
        resourceManager.SetHealthMax(health_max);
        resourceManager.UseMana(manaCost);

        // Creating the transform effect, and destroying it after it's finished
        GameObject anim = Instantiate(transformEffect, transform.position, Quaternion.identity);
        Destroy(anim, anim.GetComponentInParent<Animator>().runtimeAnimatorController.animationClips[0].length);

        //Enabling all child colliders
        Collider2D[] colls = GetComponents<Collider2D>();
        for (int i = 0; i < colls.Length; i++) colls[i].enabled = true;
        colls = GetComponentsInChildren<Collider2D>();
        for (int i = 0; i < colls.Length; i++) colls[i].enabled = true;

        //Child enable method
        FormEnable();
    }

    private void OnDisable()
    {
        //Enabling all child colliders
        Collider2D[] colls = GetComponents<Collider2D>();
        for (int i = 0; i < colls.Length; i++) colls[i].enabled = false;
        colls = GetComponentsInChildren<Collider2D>();
        for (int i = 0; i < colls.Length; i++) colls[i].enabled = false;
        FormDisable();
    }

    public bool CanBeTransformed() { return resourceManager.HasMana(manaCost); }

    private void FixedUpdate()
    {
        Move();
    }

    // Method to do stuff that happens when transforming to this form and when distransforming
    public abstract void FormEnable();
    public abstract void FormDisable();

    // Method to receive input and to interpret them
    public abstract void Move();
}
