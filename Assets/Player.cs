using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

public class Player : NetworkBehaviour {

    [SyncVar]
    private bool isDead = false;
    public bool dead {
        get { return isDead; }
        protected set { isDead = value; }
    }

    [SerializeField]
    private int maxHealth = 100;

    [SyncVar]
    private int currentHealth;

    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

    private Image healthBarFiller;

    public void Setup() {
		healthBarFiller = transform.FindChild ("GameUI").FindChild ("Healthbar").FindChild("Mask").FindChild("Filler").gameObject.GetComponentInChildren<Image>();
        wasEnabled = new bool[disableOnDeath.Length];
        for(int i = 0; i < wasEnabled.Length; i++) {
            wasEnabled[i] = disableOnDeath[i].enabled;
        }
        SetDefaults();
    }

    void SetDefaults() {
        isDead = false;
        currentHealth = maxHealth;
		healthBarFiller.fillAmount = 1f;
        for(int i = 0; i < disableOnDeath.Length; i++) {
            disableOnDeath[i].enabled = wasEnabled[i];
        }
    }

    [ClientRpc]
    public void RpcTakeDamage (int amount) {
        if (isDead)
            return;

        currentHealth -= amount;
        Debug.Log(transform.name + " now has " + currentHealth + " health");
		healthBarFiller.fillAmount = currentHealth * 1.0f / maxHealth;
        if(currentHealth <= 0) {
            Die();
        }
    }

    private void Die() {
        isDead = true;

        // disable components

        Debug.Log(transform.name + " is dead");

        for (int i = 0; i < disableOnDeath.Length; i++) {
            disableOnDeath[i].enabled = false;
        }

        transform.Rotate(Vector3.back * 90);
        transform.Translate(Vector3.right * 0.4f);

        // call respawn method

        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn () {
        yield return new WaitForSeconds(10f);

        SetDefaults();
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        GameManager.RegisterPlayer(transform.name, this);
    }

}
