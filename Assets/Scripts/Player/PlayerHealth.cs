using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour{

    [SerializeField] private float maxHealth;

    private float health;

    private void Awake(){
        health = maxHealth;
    }

    public void Damage(float count){
        health -= count;
        if(health <= 0){
            SceneManager.LoadScene(0);
        }
    }
}
