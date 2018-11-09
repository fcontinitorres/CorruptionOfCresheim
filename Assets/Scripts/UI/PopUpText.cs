using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpText : MonoBehaviour {

    [SerializeField] private float moveSpeed;
    private Vector3 movingTo;

    private bool canMove = false;

	void Start () {
        movingTo = new Vector3(Random.Range(-1f, 1f), 0, 0);
        Destroy(gameObject, GetComponentInChildren<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);
	}

    private void Update() {
        if (canMove) {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + movingTo,
                moveSpeed * Time.deltaTime);
        }
    }

    public void SetTextAndColor(string text, Color main, Color outline) {
        Text txt = GetComponentInChildren<Text>();
        txt.text = text;
        txt.color = main;
        GetComponentInChildren<Outline>().effectColor = outline;

        canMove = true;
    }
}
