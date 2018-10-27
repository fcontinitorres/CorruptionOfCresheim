using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour {

    private PlayerController controller;

    private void Start() {
        controller = gameObject.GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        controller.SetIsOnGround(true);
    }

    private void OnTriggerStay(Collider other) {
        controller.SetIsOnGround(true);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        controller.SetIsOnGround(false);
    }
}
