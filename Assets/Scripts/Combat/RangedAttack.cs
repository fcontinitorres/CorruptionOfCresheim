using UnityEngine;
using System.Collections;

public class RangedAttack : MonoBehaviour
{
    // Entity to reduce mana from the attack
    private Entity entity;
    //Player animator to trigger attack animations
    private Animator animator;
    //Label to trigger the corresponding animation, eg: "Attack_Ground"
    [SerializeField] private string animatorLabel;
    //Player audio manager to trigger attack sounds
    private AudioManager audioManager;
    [SerializeField] private string audioLabel;

    //Animation clips of the attack, to get the duration of the attack
    [SerializeField] private AnimationClip[] attacks;
    //Float to represent at what point of the current animation the attack will hit, eg: 0.5 means the attacks will hit in the middle of the animation
    [Range(0, 1)] [SerializeField] private float hitAnimationProportion;
    //Float to represent the time of input tolerance to continue the combo animation
    [SerializeField] public float cooldownTolerance;
    // Current cooldown
    [HideInInspector] public float cooldownCurr;

    // Projetile tipe for each attack
    [SerializeField] private GameObject[] projectiles;
    // The middle of the attack hit box
    [SerializeField] private Transform[] firePoints;
    // Current attack count
    private int attackCount;

    // Mana consumption
    [SerializeField] private int manaUsage;

    private void Awake()
    {
        if (manaUsage > 0)
            entity = GetComponentInParent<Entity>();
        animator = GetComponentInParent<Animator>();
        audioManager = GetComponentInParent<AudioManager>();

        cooldownCurr = 0;
        attackCount = 0;
    }

    private void FixedUpdate()
    {
        // Decreasing current cooldown
        cooldownCurr = Mathf.Max(0, cooldownCurr - Time.deltaTime);
        if (animator)
            animator.SetFloat(animatorLabel + "_Cooldown", cooldownCurr);
    }

    public void Attack()
    {
        // If the attack has the necessary cooldown
        if (cooldownCurr <= cooldownTolerance) {
            // If the cooldown is 0, will reset combo
            if (cooldownCurr == 0)
                attackCount = 0;
            // Breaking the combo when there's no following attack
            if (attacks.Length > 0 && attackCount >= attacks.Length)
                return;

            // Triggering animation
            if (animator)
                animator.SetTrigger(animatorLabel + attackCount);

            if (audioManager)
                audioManager.Play(audioLabel);

            // Firing the current projectile
            StartCoroutine(DelayedShot(attackCount));

            // If has an attack animation
            if (attacks.Length > 0) {
                // Setting current attack cooldown
                cooldownCurr = attacks[attackCount].length;
                // Increasing attack combo
                attackCount++;
            }
            // Otherwise will wait cooldownTolerance seconds
            else
                cooldownCurr = 2 * cooldownTolerance;
        }
    }

    IEnumerator DelayedShot(int attackCount)
    {
        // If has an attack animation
        if (attacks.Length > 0) {
            // Will wait the proportion time to hit
            yield return new WaitForSeconds(attacks[attackCount].length * hitAnimationProportion);
        }

        if (entity) {
            if (!entity.HasMana(manaUsage))
                yield break;
            entity.UseMana(manaUsage);
        }

        Instantiate(projectiles[attackCount % projectiles.Length],
                firePoints[attackCount].position, firePoints[attackCount].rotation);
    }
}
