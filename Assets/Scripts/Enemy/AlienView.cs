using System;
using UnityEngine;

public class AlienView : MonoBehaviour{

    [SerializeField] private Transform eyes;

    private Transform currentTarget;
    public event Action<Transform> OnTargetChanged;

    private void OnTriggerStay(Collider other){

        if(other.gameObject.TryGetComponent<Player>(out _)){

            //Debug.Log("eyes: " + eyes.position + "||| other: " + other.transform.position);
            Debug.DrawRay(eyes.position, other.transform.position - eyes.position, Color.green);

            if(Physics.Raycast(eyes.position, other.transform.position - eyes.position, out RaycastHit hit, 25f)){

                //Debug.Log(hit.collider.gameObject.tag);

                if(hit.collider.gameObject.TryGetComponent<Player>(out _)){
                    //Debug.Log("yes");
                    OnTargetChanged?.Invoke(other.transform);
                    currentTarget = other.transform;
                }
                else{
                    Debug.Log($"{hit.collider.gameObject.name}");
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
