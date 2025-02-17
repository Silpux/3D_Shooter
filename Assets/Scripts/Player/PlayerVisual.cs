using UnityEngine;

public class PlayerVisual : MonoBehaviour{

    [SerializeField] private Player player;

    private Animator animator;

    private const string STATE_ANIMATION_NAME = "State";


    private void Awake(){
        animator = GetComponent<Animator>();
    }

    private void OnEnable(){
        player.OnIdle += Idle;
        player.OnWalk += Walk;
        player.OnSprint += Sprint;
    }

    private void OnDisable(){
        player.OnIdle -= Idle;
        player.OnWalk -= Walk;
        player.OnSprint -= Sprint;
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

    private void Update(){
        
    }

}
