using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int health;
    private float stamina = 100;

    public int maxHealth = 100;
    public float staminAmount = 0.01f;
    public bool isStaminaRestoring = false;

    public GameObject Player;

    public static GameManager ManagerInstance;

    private void Awake()
    {
        ManagerInstance = this; 
    }

    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = false;

        health = 50;
    }

    void Update()
    {
        StaminaCheck();
    }

    private void StaminaCheck()
    {
        Debug.Log($"Stamina {stamina}");
        if (stamina <= 0) StartCoroutine(StaminaRestore());
    }

    private IEnumerator StaminaRestore()
    {
        isStaminaRestoring = true;
        yield return new WaitForSeconds(3);
        stamina = 100f;
        isStaminaRestoring = false;
    }

    public void SpendStamina() => stamina -= staminAmount;

    public void DamagePlayer(int Count)
    {
        if (health > 0)
        {
            health -= Count;
            Debug.Log("Вам пизда, вам нанесли " + Count);
        }
        if (health <= 0) Debug.Log("ты помена хуй пидор хуй соси член");
    }


    public void Healing(int HealthPointCount)
    {
        if (health + HealthPointCount >= maxHealth) health = maxHealth;
        else health += HealthPointCount;

        Debug.Log("HP: " + health);
    }
}
