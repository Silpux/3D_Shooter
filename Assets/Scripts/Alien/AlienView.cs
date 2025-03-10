using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AlienView : MonoBehaviour{

    [SerializeField] private Alien alien;
    [SerializeField] private Transform eyes;

    [SerializeField] private float walkTargetRadius = 20f;

    private int viewLayerMask;

    public Transform ChasingTarget{get; private set;} = null;
    private Vector3? lastChasingTargetPosition = null;

    private Vector3 walkingTarget;
    private bool targetInFOV;

    public event Action<Vector3?> OnTargetChanged;

    private void OnEnable(){
        alien.OnHit += GetHit;
    }

    private void OnDisable(){
        alien.OnHit -= GetHit;
    }

    private void Start(){
        walkingTarget = transform.position;
        viewLayerMask = ~((1 << LayerMask.NameToLayer("Alien")) | (1 << LayerMask.NameToLayer("Ignore Raycast")));
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

            if(Physics.Raycast(eyes.position, other.transform.position - eyes.position, out RaycastHit hit, 250f, viewLayerMask)){

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

    private void GetHit(Transform damageSource){

        SetTarget(damageSource);

        foreach(var alien in GetWitnesses(5f)){
            alien.SetTarget(damageSource);
        }
    }

    private List<AlienView> GetWitnesses(float maxDistance){

        Collider[] colliders = Physics.OverlapSphere(transform.position, maxDistance);
        List<AlienView> nearbyObjects = new List<AlienView>();

        foreach(Collider col in colliders){

            if(col.gameObject == gameObject) continue;

            if(col.gameObject.TryGetComponent(out AlienView alienView)){
                nearbyObjects.Add(alienView);
            }
        }
        
        return nearbyObjects;

    }

    private void SetTarget(Transform newTarget){

        Vector3? newTargetPosition = FindNavMeshPointBelow(newTarget?.position);
        OnTargetChanged?.Invoke(newTargetPosition);

        ChasingTarget = newTarget;
        if(newTarget != null){
            lastChasingTargetPosition = newTargetPosition;
        }

    }


    public Vector3? FindNavMeshPointBelow(Vector3? target, float maxDropDistance = 50f, float maxNavMeshDistance = 5f){
        
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
