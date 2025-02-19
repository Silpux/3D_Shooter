using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Alien : MonoBehaviour{


    [SerializeField] private AlienView alienView;

    private NavMeshAgent agent;

    private Transform target;
    private Health health;

    public float runSpeed;
    public float walkSpeed;

    public float damageOnHit;

    private bool canAttack = true;

    public float attackCooldown;

    
    public event Action OnIdle;
    public event Action OnWalk;
    public event Action OnSprint;

    public event Action OnPunch;


    private void OnEnable(){
        health.OnDeath += Death;
        alienView.OnTargetChanged += SetTarget;
    }
    private void OnDisable(){
        health.OnDeath -= Death;
        alienView.OnTargetChanged -= SetTarget;
    }

    private void SetTarget(Transform newTarget){
        target = newTarget;
    }

    private void Awake(){
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
    }

    private void FixedUpdate(){

        if(target){
            agent.SetDestination(target.position);
            agent.speed = runSpeed;
        }

    }

    private void Update(){

        if(agent.velocity.magnitude > 0.5f){
            OnSprint?.Invoke();
        }
        else if(agent.velocity.magnitude > 0){
            OnWalk?.Invoke();
        }
        else{
            OnIdle?.Invoke();
        }

    }

    private void Death(){
        Destroy(gameObject);
    }

    public void Attack(){

        if(canAttack){
            OnPunch?.Invoke();
            canAttack = false;
            StartCoroutine(AttackCooldown());
        }

    }

    public void DoDamage(){
        if(target && target.TryGetComponent(out Health playerHealth)){
            playerHealth.Damage(damageOnHit);
        }
    }

    private IEnumerator AttackCooldown(){
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }


}
