using UnityEngine;
using System.Collections;

public class RangedAttack : MonoBehaviour
{
    // Entity to reduce mana from the attack
    private Entity entity;
    //Player animator to trigger attack animations
    private Animator animator;

    // Current cooldown
    [System.NonSerialized] public float cooldownCurr;
    //Label to trigger the corresponding animation, eg: "Attack_Ground"
    [SerializeField] private string animatorLabel;
    //Animation clips of the attack, to get the duration of the attack
    [SerializeField] private AnimationClip[] attacks;
    //Float to represent at what point of the current animation the attack will hit, eg: 0.5 means the attacks will hit in the middle of the animation
    [Range(0, 1)] [SerializeField] private float hitAnimationProportion;
    //Float to represent the time of input tolerance to continue the combo animation
    [SerializeField] public float cooldownTolerance;
    
    // Projetile tipe for each attack
    [SerializeField] private GameObject[] projectiles;
    // The middle of the attack hit box
    [SerializeField] private Transform[] firePoints;
    // Attacks of each damage
    [SerializeField] private int[] attackDamages;
    // Current attack count
    private int attackCount;

    // Mana consumption
    [SerializeField] private int manaUsage;

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
        animator = GetComponentInParent<Animator>();

        cooldownCurr = 0;
        attackCount = 0;
    }

    private void FixedUpdate()
    {
        // Decreasing current cooldown
        cooldownCurr = Mathf.Max(0, cooldownCurr - Time.deltaTime);
        animator.SetFloat(animatorLabel + "_Cooldown", cooldownCurr);
    }

    public void Attack()
    {
        // If the attack has the necessary cooldown
        if (cooldownCurr <= cooldownTolerance)
        {
            // If the cooldown is 0, will reset combo
            if (cooldownCurr == 0) attackCount = 0;
            // Breaking the combo when there's no following attack
            if (attackCount >= attacks.Length) return;

            // Triggering animation
            animator.SetTrigger(animatorLabel + attackCount);

            // Firing the current projectile
            StartCoroutine(DelayedShot(attackCount));

            // Setting current attack cooldown
            cooldownCurr = attacks[attackCount].length;
            // Increasing attack combo
            attackCount++;
        }
    }

    IEnumerator DelayedShot(int attackCount)
    {
        // Wait the proportion time to hit
        yield return new WaitForSeconds(attacks[attackCount].length * hitAnimationProportion);

        if (!entity.HasMana(manaUsage)) yield break;
        entity.UseMana(manaUsage);

        Instantiate(projectiles[attackCount % projectiles.Length],
                firePoints[attackCount].position, firePoints[attackCount].rotation);
    }
}
