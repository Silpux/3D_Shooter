using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour{

    [SerializeField] private float maxHealth;

    public event Action<float> OnHealthChange;

    private float health;

    private float Health{
        get => health;
        set{
            health = value;
            OnHealthChange?.Invoke(health / maxHealth);
        }
    }

    private void Awake(){
        Health = maxHealth;
    }

    public void Damage(float count){
        Health -= count;
        if(Health <= 0){
            SceneManager.LoadScene(0);
        }
    }
}
