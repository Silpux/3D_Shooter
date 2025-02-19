using System;
using UnityEngine;

public class AlienView : MonoBehaviour{

    [SerializeField] private Transform eyes;

    private Transform currentTarget;
    public event Action<Transform> OnTargetChanged;

    private void OnTriggerStay(Collider other){

        if(other.gameObject.TryGetComponent<Player>(out Player player)){

            if(Physics.Raycast(eyes.position, other.transform.position - eyes.position, out RaycastHit hit, 25f)){

                if(hit.collider.gameObject.TryGetComponent<Player>(out Player p) && p == player){
                    OnTargetChanged?.Invoke(other.transform);
                    currentTarget = other.transform;
                }

            }

        }
    
    }
    private void OnTriggerExit(Collider other){
        
        if(other.transform == currentTarget){
            OnTargetChanged?.Invoke(null);
        }
    
    }

}
