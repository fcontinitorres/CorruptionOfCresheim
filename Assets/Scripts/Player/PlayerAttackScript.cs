using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackScript : MonoBehaviour {

    private Animator animator;

	[System.NonSerialized] public float cooldownCurr;
    [SerializeField] private string animatorLabel;
    [SerializeField] private AnimationClip[] attacks;
    [SerializeField] public float cooldownTolerance;

    [SerializeField] private LayerMask whatIsEnemy;
    [SerializeField] private Transform[] colliderPoints;
    [SerializeField] private Vector2[] colliderSizes;
    [SerializeField] private int[] attackDamages;
    private int attackCount;

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();

        cooldownCurr = 0;
        attackCount = 0;
    }

    private void FixedUpdate()
    {
        cooldownCurr = Mathf.Max(0, cooldownCurr - Time.deltaTime);
        animator.SetFloat(animatorLabel+"_Cooldown", cooldownCurr);
    }

    public void Attack()
    {
        if(cooldownCurr <= cooldownTolerance)
        {
            if (cooldownCurr == 0) attackCount = 0;
            if (attackCount >= attacks.Length) return;

            animator.SetTrigger(animatorLabel + attackCount);

            StartCoroutine(DelayedHit(attackCount));

            cooldownCurr = attacks[attackCount].length;
            attackCount++;
        }
    }

    IEnumerator DelayedHit(int attackCount)
    {
        yield return new WaitForSeconds(attacks[attackCount].length/3);

        Collider2D[] enemiesToHit = Physics2D.OverlapBoxAll(colliderPoints[attackCount].position,
                colliderSizes[attackCount], 0, whatIsEnemy);

        for (int i = 0; i < enemiesToHit.Length; i++)
        {
            enemiesToHit[i].GetComponentInParent<Entity>().TakeDamage(attackDamages[attackCount]);
        }
    }
}
