using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour{

    [SerializeField] private PlayerHealth playerHealth;

    [SerializeField] private Gradient gradient;
    private Image fill;

    private void Awake(){
        fill = GetComponent<Image>();
        playerHealth.OnHealthChange += UpdateFill;
    }

    private void UpdateFill(float value){
        fill.fillAmount = value;
        fill.color = gradient.Evaluate(value);
    }

}
