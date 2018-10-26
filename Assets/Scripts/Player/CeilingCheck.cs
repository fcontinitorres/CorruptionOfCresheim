using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingCheck : MonoBehaviour {

    private PlayerController controller;

    private void Start()
    {
        controller = gameObject.GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger enter " + collision);
        controller.SetIsOnCeiling(true);
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Trigger stay " + other);
        controller.SetIsOnCeiling(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Trigger exit " + collision);
        controller.SetIsOnCeiling(false);
    }
}
