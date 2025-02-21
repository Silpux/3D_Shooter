using Unity.Mathematics;
using UnityEngine;

public class AlienVisual : MonoBehaviour{


    [SerializeField] private Alien alien;
    [SerializeField] private Health health;

    [SerializeField] private ParticleSystem deathEffect;

    private Animator animator;

    private const string STATE_PARAM_NAME = "State";
    private const string PUNCH_ANIMATION_NAME = "Punch";
    private const string DAMAGE_ANIMATION_NAME = "Damage";
    private const string DEATH_ANIMATION_NAME = "Death";

    private void Awake(){
        animator = GetComponent<Animator>();
    }

    private void OnEnable(){
        alien.OnIdle += Idle;
        alien.OnWalk += Walk;
        alien.OnSprint += Sprint;

        alien.OnPunch += Punch;

        health.OnDamage += TakeDamage;
        health.OnDeath += Death;
    }

    private void OnDisable(){
        alien.OnIdle -= Idle;
        alien.OnWalk -= Walk;
        alien.OnSprint -= Sprint;

        alien.OnPunch -= Punch;

        health.OnDamage -= TakeDamage;
        health.OnDeath -= Death;
    }


    private void TakeDamage(float _){
        animator.Play(DAMAGE_ANIMATION_NAME, 1, 0f);
    }
    private void Idle(){
        animator.SetInteger(STATE_PARAM_NAME, 0);
    }

    private void Walk(){
        animator.SetInteger(STATE_PARAM_NAME, 1);
    }

    private void Sprint(){
        animator.SetInteger(STATE_PARAM_NAME, 2);
    }

    private void Punch(){
        animator.Play(PUNCH_ANIMATION_NAME);
    }

    public void DoDamage(){
        alien.DoDamage();
    }

    private void Death(){
        animator.Play(DEATH_ANIMATION_NAME);
    }

    public void DeathParticles(){
        Instantiate(deathEffect, transform.position + transform.up * 1.5f, Quaternion.Euler(-90, 0, 0), null);
    }

}
