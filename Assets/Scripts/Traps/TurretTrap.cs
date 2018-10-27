using UnityEngine;
using System.Collections;

public class TurretTrap : MonoBehaviour {

    [SerializeField] private RangedAttack attack;

    private void FixedUpdate() {
        attack.Attack();
    }
}
