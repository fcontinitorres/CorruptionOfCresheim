using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Entity : MonoBehaviour {

    // Damage pop up text
    [SerializeField] private GameObject popupPrefab;

    // UI components to draw the heart sprites
    [SerializeField] private Image[] heartsUI;
    // Heart sprites
    [SerializeField] private Sprite[] heartSprites;
    // Text colors
    [SerializeField] private Color[] healthDamageTextColor;
    [SerializeField] private Color[] healthHealTextColor;

    // Max health and current health
    [SerializeField] private int health_max = 0;
    public float health_curr = -1;

    // UI components to draw the mana sprites
    [SerializeField] private Image[] manaUI;
    // Mana sprites
    [SerializeField] private Sprite[] manaSprites;
    // Text colors
    [SerializeField] private Color[] manaUsageTextColors;
    [SerializeField] private Color[] manaRecoverTextColors;

    // Max mana and current mana
    [SerializeField] private int mana_max = 0;
    private int mana_curr = -1;

    protected virtual void Awake() {
        // Initiating variables
        SetHealthMax(health_max);
        SetManaMax(mana_max);
        //Updating UI
        UpdateHealth();
        UpdateMana();
    }

    public int GetHealth() { return (int)health_curr; }
    public void SetHealthMax(int health) {
        // Setting max health, will keep the current health proportional
        if (health_curr == -1) health_curr = health;
        else health_curr = Mathf.Max(0, (float)health * (float)health_curr / (float)health_max);
        health_max = health;

        UpdateHealth();
        if (health_curr == 0) Die();
    }
    public virtual void TakeDamage(int dmg) {
        // Taking damage, reducing to a minimum of zero
        if (dmg <= 0) return;
        health_curr = Mathf.Max(0, health_curr - dmg);

        UpdateHealth();
        CreatePopUpText("" + dmg, healthDamageTextColor);
        if (health_curr == 0) Die();
    }
    public void HealDamage(int heal) {
        // Healing health
        if (heal <= 0) return;
        health_curr = Mathf.Min(health_max, health_curr + heal);

        UpdateHealth();
        CreatePopUpText("" + heal, healthHealTextColor);
    }

    public int GetMana() { return mana_curr; }
    public void SetManaMax(int mana) {
        if (mana_curr == -1) mana_curr = mana;
        else mana_curr = (int)((float)mana * (float)mana_curr / (float)mana_max);
        mana_max = mana;
        UpdateMana();
    }
    public bool HasMana(int qtt) {
        if (qtt < 0) return false;
        return mana_curr >= qtt;
    }
    public void UseMana(int qtt) {
        if (qtt <= 0) return;
        mana_curr = Mathf.Max(0, mana_curr - qtt);

        UpdateMana();
        CreatePopUpText("" + qtt, manaUsageTextColors);
    }
    public void RecoverMana(int qtt) {
        if (qtt <= 0) return;
        mana_curr = Mathf.Min(mana_max, mana_curr + qtt);

        UpdateMana();
        CreatePopUpText("" + qtt, manaRecoverTextColors);
    }

    void UpdateHealth() {
        if (heartsUI.Length == 0) return;
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
        if (manaUI.Length == 0) return;
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

    private void CreatePopUpText(string text, Color[] colors) {
        if (popupPrefab) {
            GameObject popup = Instantiate(popupPrefab, transform.position, Quaternion.identity);
            popup.GetComponent<PopUpText>().SetTextAndColor(text, colors[0], colors[1 % colors.Length]);
        }
    }

    public virtual void Die() {
        Destroy(this.gameObject);
    }
}
