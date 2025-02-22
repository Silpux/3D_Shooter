using System;
using UnityEngine;
using UnityEngine.AI;

public class AlienView : MonoBehaviour{

    [SerializeField] private Alien alien;
    [SerializeField] private Transform eyes;

    [SerializeField] private float walkTargetRadius = 20f;

    public Transform ChasingTarget{get; private set;} = null;
    private Vector3? lastChasingTargetPosition = null;

    private Vector3 walkingTarget;
    private bool targetInFOV;

    public event Action<Vector3?> OnTargetChanged;

    private void OnEnable(){
        alien.OnHit += SetTarget;
    }

    private void OnDisable(){
        alien.OnHit -= SetTarget;
    }

    private void Start(){
        walkingTarget = transform.position;
    }

    private void Update(){

        if(lastChasingTargetPosition is null){

            float distance = Vector3.Distance(walkingTarget, transform.position);

            if(distance < 1f){
                walkingTarget = RandomNavmeshLocation(walkTargetRadius);
                OnTargetChanged?.Invoke(walkingTarget);
            }

        }
        else if(!targetInFOV){

            Vector3 lastTargetPosition = (Vector3)lastChasingTargetPosition;

            if(Vector3.Distance(transform.position, lastTargetPosition) < 2f){
                lastChasingTargetPosition = null;
                SetTarget(null);
                walkingTarget = RandomNavmeshLocation(walkTargetRadius);
                OnTargetChanged?.Invoke(walkingTarget);
            }

        }
    }

    public Vector3 RandomNavmeshLocation(float radius){

        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        Vector3 finalPosition = Vector3.zero;

        if(NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, 1)){
            finalPosition = hit.position;            
        }

        return finalPosition;

    }

    private void OnTriggerStay(Collider other){

        if(other.gameObject.TryGetComponent(out Player player)){

            if(Physics.Raycast(eyes.position, other.transform.position - eyes.position, out RaycastHit hit, 250f)){

                if(hit.collider.gameObject.TryGetComponent(out Player p) && p == player){
                    SetTarget(other.transform);
                    targetInFOV = true;
                }

            }

        }
    
    }

    private void OnTriggerExit(Collider other){

        if(other.transform == ChasingTarget){
            targetInFOV = false;
        }

    }

    private void SetTarget(Transform newTarget){

        Vector3? newTargetPosition = FindNavMeshPointBelow(newTarget?.position);
        OnTargetChanged?.Invoke(newTargetPosition);

        ChasingTarget = newTarget;
        if(newTarget != null){
            lastChasingTargetPosition = newTargetPosition;
        }

    }


    public Vector3? FindNavMeshPointBelow(Vector3? target, float maxDropDistance = 50f, float maxNavMeshDistance = 2f){
        
        if(target is null) return null;
        Vector3 targetPosition = (Vector3)target;
        
        if(Physics.Raycast(targetPosition, Vector3.down, out RaycastHit hit, maxDropDistance)){
            Vector3 candidatePoint = hit.point;

            if(NavMesh.SamplePosition(candidatePoint, out NavMeshHit navHit, maxNavMeshDistance, NavMesh.AllAreas)){
                return navHit.position;
            }
        }

        return transform.position;
    }

}
