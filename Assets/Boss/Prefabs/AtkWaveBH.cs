using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkWaveBH : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<MeleeAttack>().Attack();
        Destroy(gameObject, 0.5f);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
