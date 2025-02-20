using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour{

    [SerializeField] private float maxHealth;

    public event Action<float> OnDamage;
    public event Action<float> OnHeal;
    public event Action OnDeath;

    private float value;

    private float Value{
        get => value;
        set{
            if(value <= 0){
                OnDeath?.Invoke();
            }
            else if(value > this.value){
                OnHeal?.Invoke(value / maxHealth);
            }
            else{
                OnDamage?.Invoke(value / maxHealth);
            }
            this.value = value;
        }
    }

    private void Awake(){
        Value = maxHealth;
    }

    public void Damage(float count){
        Value -= count;
    }
}
