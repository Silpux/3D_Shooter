using System;
using UnityEngine;

public class AlienView : MonoBehaviour{

    [SerializeField] private Alien alien;
    [SerializeField] private Transform eyes;

    private Transform currentTarget;
    public event Action<Transform> OnTargetChanged;

    private void OnEnable(){
        alien.OnHit += SetTarget;
    }
    private void OnDisable(){
        alien.OnHit -= SetTarget;
    }

    private void OnTriggerStay(Collider other){

        if(other.gameObject.TryGetComponent(out Player player)){

            if(Physics.Raycast(eyes.position, other.transform.position - eyes.position, out RaycastHit hit, 25f)){

                if(hit.collider.gameObject.TryGetComponent(out Player p) && p == player){
                    SetTarget(other.transform);
                }

            }

        }
    
    }
    private void OnTriggerExit(Collider other){
        
        if(other.transform == currentTarget){
            SetTarget(null);
        }
    
    }

    private void SetTarget(Transform newTarget){
        OnTargetChanged?.Invoke(newTarget);
        currentTarget = newTarget;
    }

}
