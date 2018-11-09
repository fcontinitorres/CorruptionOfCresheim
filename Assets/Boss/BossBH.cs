using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBH : Entity {

    public GameObject player;

    public float velocity;

    //movimento
    private Transform target;
    private int direction;

    //atk
    private float distAtk;
    private float atkingTime;
    public float idelTime;
    public GameObject wave;
    private bool waveInstance;
    //spc Atk
    private float playerHeight;
    public GameObject rock;
    private bool rockInstance;

    //boss HP
    public GameObject healthBar;

    //tempo
    private float totalTime;

    //animacao
    private Animator anim;

    private Vector3 bossScale;

    private STATE bossState;
    private enum STATE
    {
        idel,
        move,
        atk,
        specialAtk
    }

	// Use this for initialization
	void Start () {
        target = player.transform;
        bossState = STATE.idel;
        totalTime = 0f;
        anim = GetComponent<Animator>();

        waveInstance = false;
        rockInstance = false;
        distAtk = 7f;
        atkingTime = 0.65f;
        playerHeight = 6.5f;

        bossScale = transform.localScale;

    }
	
	// Update is called once per frame
	void Update () {

        Vector3 v = new Vector3((health_curr/20f), 1f, 1f);
        healthBar.transform.localScale = v;

        if (target.transform.position.x > this.transform.position.x)
            direction = 1; //direita
        else
            direction = -1; //esquerda

        bossScale = new Vector3(direction * 2,2f,2f);
        transform.localScale = bossScale;

        if (bossState == STATE.idel)
        {
            //permiter boss atkar
            waveInstance = false;
            rockInstance = false;

            totalTime += Time.deltaTime;

            if (totalTime >= idelTime)
            {
                anim.SetInteger("bossState", 1);
                
                bossState = STATE.move;
            }
                
        }
        else if(bossState == STATE.move)
        {
            
            //seguir player
            this.transform.Translate(direction * velocity * Time.deltaTime, 0f, 0f);

            if (Mathf.Abs(this.transform.position.x - target.transform.position.x) < distAtk)
            {
                anim.SetInteger("bossState", 2);
                totalTime = 0;
                if (target.transform.position.y <= playerHeight)
                    bossState = STATE.atk;
                else
                    bossState = STATE.specialAtk;
            }
                
        }
        else if(bossState == STATE.atk)
        {

            totalTime += Time.deltaTime;

            if(totalTime >= 0.45f && waveInstance == false)
            {
                wave.transform.localScale = new Vector3(direction * 4, 8f, 1f);
                Instantiate(wave, new Vector3( this.transform.position.x + (direction * 7f) , this.transform.position.y, transform.position.z), wave.transform.rotation);
                waveInstance = true;
            }

            if (totalTime >= atkingTime)
            {
                anim.SetInteger("bossState", 0);
                totalTime = 0f;
                bossState = STATE.idel;
            }
        }
        else if(bossState == STATE.specialAtk)
        {
            totalTime += Time.deltaTime;

            if (totalTime >= 0.6f && rockInstance == false)
            {
                Instantiate(rock, new Vector3(target.transform.position.x, player.transform.position.y + 5f, rock.transform.position.z), rock.transform.rotation);
                rockInstance = true;
            }

            if (totalTime >= atkingTime)
            {
                anim.SetInteger("bossState", 0);
                totalTime = 0f;
                bossState = STATE.idel;
            }
        }
    }

}
