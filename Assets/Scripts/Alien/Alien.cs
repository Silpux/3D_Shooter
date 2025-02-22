using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Alien : MonoBehaviour{


    [SerializeField] private AlienView alienView;

    private NavMeshAgent agent;

    private Vector3? target;
    private Health health;

    [SerializeField] private float runSpeed;
    [SerializeField] private float walkSpeed;

    [SerializeField] private float damageOnHit;

    private bool canAttack = true;

    private bool dead;

    [SerializeField] private float attackCooldown;

    public event Action OnIdle;
    public event Action OnWalk;
    public event Action OnSprint;

    public event Action OnPunch;
    public event Action<Transform> OnHit;


    private void OnEnable(){
        health.OnDeath += Death;
        alienView.OnTargetChanged += SetTarget;
    }
    private void OnDisable(){
        health.OnDeath -= Death;
        alienView.OnTargetChanged -= SetTarget;
    }

    private void SetTarget(Vector3? newTarget){
        if(!dead){
            target = newTarget;
        }
    }

    private void Awake(){
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
    }

    private void FixedUpdate(){

        if(!dead && target is not null){
            agent.SetDestination((Vector3)target);
        }

    }

    private void Update(){

        if(alienView.ChasingTarget != null){
            OnSprint?.Invoke();
            agent.speed = runSpeed;
        }
        else if(target != null){
            OnWalk?.Invoke();
            agent.speed = walkSpeed;
        }
        else{
            OnIdle?.Invoke();
        }

    }

    private void Death(){
        dead = true;
        agent.speed = 0f;
        GetComponent<CapsuleCollider>().enabled = false;
        Destroy(gameObject, 3f);
    }

    public void Attack(){

        if(!dead && canAttack){
            OnPunch?.Invoke();
            canAttack = false;
            StartCoroutine(AttackCooldown());
        }

    }

    public void Hit(Transform damageSource){
        OnHit?.Invoke(damageSource);
    }

    public void DoDamage(){
        if(alienView.ChasingTarget && alienView.ChasingTarget.TryGetComponent(out Health playerHealth)){
            playerHealth.Damage(damageOnHit);
        }
    }

    private IEnumerator AttackCooldown(){
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }


}
