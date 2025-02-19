using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour{

    [SerializeField] private Health playerHealth;

    [SerializeField] private Gradient gradient;

    private Image fill;

    private void Awake(){
        fill = GetComponent<Image>();
    }

    private void OnEnable(){
        playerHealth.OnDamage += UpdateFill;
        playerHealth.OnHeal += UpdateFill;
    }

    private void OnDisable(){
        playerHealth.OnDamage -= UpdateFill;
        playerHealth.OnHeal -= UpdateFill;
    }

    private void UpdateFill(float value){
        fill.fillAmount = value;
        fill.color = gradient.Evaluate(value);
    }

}
