using UnityEngine;

public class AlienVisual : MonoBehaviour{


    [SerializeField] private Alien alien;

    private Animator animator;

    private const string STATE_ANIMATION_NAME = "State";
    private const string PUNCH_ANIMATION_NAME = "Punch";

    private void Awake(){
        animator = GetComponent<Animator>();
    }

    private void OnEnable(){
        alien.OnIdle += Idle;
        alien.OnWalk += Walk;
        alien.OnSprint += Sprint;

        alien.OnPunch += Punch;
    }

    private void OnDisable(){
        alien.OnIdle -= Idle;
        alien.OnWalk -= Walk;
        alien.OnSprint -= Sprint;

        alien.OnPunch -= Punch;
    }

    private void Idle(){
        animator.SetInteger(STATE_ANIMATION_NAME, 0);
    }

    private void Walk(){
        animator.SetInteger(STATE_ANIMATION_NAME, 1);
    }

    private void Sprint(){
        animator.SetInteger(STATE_ANIMATION_NAME, 2);
    }

    private void Punch(){
        animator.Play("Punch");
    }

    public void DoDamage(){
        alien.DoDamage();
    }

}
