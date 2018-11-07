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
    
    

    //tempo
    private float totalTime;

    //animacao
    private Animator anim;

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

    }
	
	// Update is called once per frame
	void Update () {

        if (bossState == STATE.idel)
        {
            Debug.Log("Boss idel state");

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
            Debug.Log("Boss move state");
            //seguir player
            if (target.transform.position.x > this.transform.position.x)
                direction = 1;
            else
                direction = -1;
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
            Debug.Log("Boss atk state");

            totalTime += Time.deltaTime;

            if(totalTime >= 0.45f && waveInstance == false)
            {
                Instantiate(wave, new Vector3(this.transform.position.x + 7f, this.transform.position.y, transform.position.z), wave.transform.rotation);
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
            Debug.Log("Boss spcAtk state");
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
