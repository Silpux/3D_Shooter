using UnityEngine;

public class AlienAttackZone : MonoBehaviour{

    [SerializeField] private Alien alien;

    private void OnTriggerStay(Collider other){
        if(other.gameObject.TryGetComponent<Player>(out _)){
            alien.Attack();
        }
    }

}
