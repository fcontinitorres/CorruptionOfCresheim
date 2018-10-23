using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResourceManager : MonoBehaviour {
    
    private int health_max = 0;
    private int health_curr =  0;

    [SerializeField] private int mana_max = 0;
    private int mana_curr = 0;

    private void Awake()
    {
        health_max = gameObject.GetComponent<HumanForm>().health_max;
        health_curr = health_max;

        mana_curr = mana_max;
    }

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

    public void healDamage(int heal)
    {
        if (heal == 0) return;
        health_curr = Mathf.Min(health_max, health_curr + heal);
        UpdateHealth();
    }

    public bool useMana(int qtt)
    {
        if (mana_curr - qtt < 0) return false;

        mana_curr -= qtt;

        UpdateMana();
        return true;
    }

    public void recoverMana(int qtt)
    {
        mana_curr = Mathf.Min(mana_max, mana_curr + qtt);

        UpdateMana();
    }

    void UpdateHealth()
    {

    }

    void UpdateMana()
    {

    }
}
