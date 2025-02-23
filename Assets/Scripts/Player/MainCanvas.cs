using TMPro;
using UnityEngine;

public class MainCanvas : MonoBehaviour{

    private int killCount;

    [SerializeField] private TMP_Text killCountText;

    private void OnEnable(){
        Alien.OnDeath += AddKill;
    }

    private void OnDisable(){
        Alien.OnDeath -= AddKill;
    }

    private void AddKill(){
        killCount++;
        killCountText.text = $"Kills: {killCount}";
    }


}
