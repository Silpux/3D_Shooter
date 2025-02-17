using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour{


    private CharacterController characterController;

    private InputActions inputActions;

    [SerializeField] private Camera mainCamera;

    private float lookSpeedX;
    private float lookSpeedY;

    [SerializeField] private float lookSpeedXNormal = 1f;
    [SerializeField] private float lookSpeedYNormal = 0.5f;

    [SerializeField] private float lookSpeedXScoping = 0.5f;
    [SerializeField] private float lookSpeedYScoping = 0.25f;

    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravity;

    [SerializeField] private float lookLimitY = 90;

    private float cameraRotation;
    private bool sprint;
    private Vector3 moveDirection = Vector3.zero;

    private bool jumping = false;

    public event Action OnIdle;
    public event Action OnWalk;
    public event Action OnSprint;

    public event Action OnShoot;
    public event Action OnScope;

    public event Action<int> OnWeaponChange;


    private void Awake(){

        inputActions = new InputActions();

        characterController = GetComponent<CharacterController>();

        lookSpeedX = lookSpeedXNormal;
        lookSpeedY = lookSpeedYNormal;

    }

    private void OnEnable(){

        inputActions.Player.Look.performed += Look;

        inputActions.Player.Sprint.started += SprintEnable;
        inputActions.Player.Sprint.canceled += SprintDisable;

        inputActions.Player.Jump.started += JumpStart;
        inputActions.Player.Jump.canceled += JumpCancel;

        inputActions.Player.Shoot.performed += Shoot;

        inputActions.Player.Scope.performed += Scope;

        inputActions.Player.SwitchWeapon.performed += SwitchWeapon;
        inputActions.Player.Enable();
    }

    private void OnDisable(){

        inputActions.Player.Disable();
        inputActions.Player.Look.performed -= Look;

        inputActions.Player.Sprint.started -= SprintEnable;
        inputActions.Player.Sprint.canceled -= SprintDisable;

        inputActions.Player.Jump.started -= JumpStart;
        inputActions.Player.Jump.canceled -= JumpCancel;

        inputActions.Player.Shoot.performed -= Shoot;
        inputActions.Player.Scope.performed -= Scope;
        inputActions.Player.SwitchWeapon.performed -= SwitchWeapon;
    }

    private void SwitchWeapon(InputAction.CallbackContext ctx){
        OnWeaponChange((int)ctx.ReadValue<Vector2>().y >> -1 | 1);
    }

    private void JumpStart(InputAction.CallbackContext ctx){
        jumping = true;
    }
    private void JumpCancel(InputAction.CallbackContext ctx){
        jumping = false;
    }

    private void SprintEnable(InputAction.CallbackContext ctx){
        sprint = true;
    }
    private void SprintDisable(InputAction.CallbackContext ctx){
        sprint = false;
    }

    private void Shoot(InputAction.CallbackContext ctx){
        OnShoot?.Invoke();
    }

    private void Scope(InputAction.CallbackContext ctx){
        OnScope?.Invoke();
    }

    private void Look(InputAction.CallbackContext ctx){

        Vector2 value = ctx.ReadValue<Vector2>() / 10;

        cameraRotation += value.y * lookSpeedY;
        cameraRotation = Mathf.Clamp(cameraRotation, -lookLimitY, lookLimitY);

        mainCamera.transform.localRotation = Quaternion.Euler(-cameraRotation,0,0);

        transform.rotation *= Quaternion.Euler(0, value.x * lookSpeedX, 0);

    }

    private void Move(Vector2 direction){

        if(direction == Vector2.zero){
            OnIdle?.Invoke();
        }
        else if(sprint){
            OnSprint?.Invoke();
        }
        else{
            OnWalk?.Invoke();
        }

        Vector3 vectorForward = direction.y * transform.TransformDirection(Vector3.forward);
        Vector3 vectorRigth = direction.x * transform.TransformDirection(Vector3.right);

        float verticalVelocity = moveDirection.y;

        moveDirection = (vectorForward + vectorRigth) * (sprint ? runSpeed : walkSpeed);

        if(jumping && characterController.isGrounded){
            moveDirection.y = jumpForce;
        }
        else{
            moveDirection.y = verticalVelocity;
        }

        if(!characterController.isGrounded){
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);

    }

    public void ChangeLookSpeed(bool isScoping){
        if(isScoping){
            lookSpeedX = lookSpeedXScoping;
            lookSpeedY = lookSpeedYScoping;
        }
        else{
            lookSpeedX = lookSpeedXNormal;
            lookSpeedY = lookSpeedYNormal;
        }
    }


    private void Update(){

        Move(inputActions.Player.Move.ReadValue<Vector2>());

    }

}
