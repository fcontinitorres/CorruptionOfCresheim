using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingCheck : MonoBehaviour {

    private PlayerController player;

    private void Start()
    {
        player = gameObject.GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player.SetIsOnCeiling(true);
    }

    private void OnTriggerStay(Collider other)
    {
        player.SetIsOnCeiling(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        player.SetIsOnCeiling(false);
    }
}
