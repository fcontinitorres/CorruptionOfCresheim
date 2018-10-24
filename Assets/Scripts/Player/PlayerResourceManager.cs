using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerResourceManager : MonoBehaviour {

    public Image[] heartsUI;
    public Sprite[] heartSprites;
    
    private int health_max = 0;
    private float health_curr = -1;

    public Image[] manaUI;
    public Sprite[] manaSprites;

    [SerializeField] private int mana_max = 0;
    private int mana_curr = -1;

    private void Awake()
    {
        SetManaMax(mana_max);
        UpdateMana();
    }

    public int GetHealth() { return (int) health_curr; }
    public void SetHealthMax(int health)
    {
        if (health_curr == -1) health_curr = health;
        else health_curr = Mathf.Max(0, (float)health * (float)health_curr / (float)health_max);
        health_max = health;

        UpdateHealth();

        if (health_curr == 0) GetComponent<PlayerController>().Die();
    }
    public void TakeDamage(int dmg)
    {
        if (dmg <= 0) return;
        health_curr = Mathf.Max(0, health_curr - dmg);
        UpdateHealth();
        if(health_curr == 0) GetComponent<PlayerController>().Die();
    }
    public void HealDamage(int heal) {
        if (heal <= 0) return;
        health_curr = Mathf.Min(health_max, health_curr + heal);
        UpdateHealth();
    }

    public int GetMana() { return mana_curr; }
    public void SetManaMax(int mana) {

        if (mana_curr == -1) mana_curr = mana;
        else mana_curr = (int)((float)mana * (float)mana_curr / (float)mana_max);
        mana_max = mana;
        UpdateMana();
    }
    public bool HasMana(int qtt)
    {
        if (qtt < 0) return false;
        return mana_curr >= qtt;
    }
    public void UseMana(int qtt) {
        if (qtt <= 0) return;
        mana_curr = Mathf.Max(0, mana_curr - qtt);

        UpdateMana();
    }
    public void RecoverMana(int qtt) {
        if (qtt <= 0) return;
        mana_curr = Mathf.Min(mana_max, mana_curr + qtt);

        UpdateMana();
    }

    void UpdateHealth() {
        int aux = (int)health_curr;

        for (int i = 0; i < heartsUI.Length; i++) {
            //If the player has that heart slot
            if ((float)health_max / heartSprites.Length >= i) {
                //It will enable it
                heartsUI[i].enabled = true;

                heartsUI[i].sprite = heartSprites[Mathf.Min(aux, heartSprites.Length - 1)];
                aux = Mathf.Max(0, aux - (heartSprites.Length - 1));
            }
            //Otherwise, it will disable that heart slot
            else heartsUI[i].enabled = false;
        }
    }

    void UpdateMana() {
        int aux = mana_curr;

        for (int i = 0; i < manaUI.Length; i++)
        {
            //If the player has that heart slot
            if ((float)mana_max / manaSprites.Length >= i)
            {
                //It will enable it
                manaUI[i].enabled = true;

                manaUI[i].sprite = manaSprites[Mathf.Min(aux, manaSprites.Length - 1)];
                aux = Mathf.Max(0, aux - (manaSprites.Length - 1));
            }
            //Otherwise, it will disable that heart slot
            else manaUI[i].enabled = false;
        }
    }
}
