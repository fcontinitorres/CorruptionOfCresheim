using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerResourceManager : MonoBehaviour {

    public Image[] heartsUI;
    public Sprite[] heartSprites;
    
    private int health_max = 0;
    private int health_curr =  0;

    public Image[] manaUI;
    public Sprite[] manaSprites;

    [SerializeField] private int mana_max = 0;
    private int mana_curr = 0;

    private void Awake()
    {
        health_max = gameObject.GetComponent<HumanForm>().health_max;
        health_curr = health_max;

        mana_curr = mana_max;

        UpdateHealth();
        UpdateMana();
    }

    public int getHealth() { return health_curr; }
    public void setHealthMax(int health)
    {
        health_curr = (int)((float)health * (float)health_curr / (float)health_max);
        health_max = health;

        UpdateHealth();
    }
    public void takeDamage(int dmg)
    {
        if (dmg == 0) return;
        health_curr = Mathf.Max(0, health_curr - dmg);
        UpdateHealth();
    }
    public void healDamage(int heal) {
        if (heal == 0) return;
        health_curr = Mathf.Min(health_max, health_curr + heal);
        UpdateHealth();
    }

    public int getMana() { return mana_curr; }
    public void setManaMax(int mana) {
        mana_max = mana;
        UpdateMana();
    }
    public bool useMana(int qtt) {
        if (mana_curr - qtt < 0) return false;

        mana_curr -= qtt;

        UpdateMana();
        return true;
    }
    public void recoverMana(int qtt) {
        mana_curr = Mathf.Min(mana_max, mana_curr + qtt);

        UpdateMana();
    }

    void UpdateHealth() {
        int aux = health_curr;

        for (int i = 0; i < heartsUI.Length; i++) {
            //If the player has that heart slot
            if ((float)health_max / heartSprites.Length >= (i)) {
                //It will enable it
                heartsUI[i].enabled = true;

                heartsUI[i].sprite = heartSprites[Mathf.Min(aux, heartSprites.Length - 1)];
                aux = Mathf.Max(0, aux - heartSprites.Length);
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
            if ((float)mana_max / manaSprites.Length >= (i))
            {
                //It will enable it
                manaUI[i].enabled = true;

                manaUI[i].sprite = manaSprites[Mathf.Min(aux, manaSprites.Length - 1)];
                aux = Mathf.Max(0, aux - manaSprites.Length);
            }
            //Otherwise, it will disable that heart slot
            else manaUI[i].enabled = false;

        }
    }
}
