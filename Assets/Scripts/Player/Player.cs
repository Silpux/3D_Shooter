using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour{

    private CharacterController characterController;

    private InputActions inputActions;

    private Health health;

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

    [SerializeField] private float bulletBoxFocusDistance;

    private BulletBox currentFocusBox;

    private float cameraRotation;
    private bool sprint;
    private bool isScoping;

    private bool IsScoping{
        get => isScoping;
        set{
            if(isScoping ^ value){
                OnScope?.Invoke(value);
                ChangeLookSpeed(value);
                isScoping = value;
            }
        }
    }

    private Vector3 moveDirection = Vector3.zero;

    private bool jumping = false;

    public event Action OnIdle;
    public event Action OnWalk;
    public event Action OnSprint;

    public event Action<Transform> OnShoot;
    public event Action<bool> OnScope;

    public event Action<int> OnWeaponChange;

    public event Action<int> OnCollectBullets;

    private void Awake(){

        inputActions = new InputActions();

        characterController = GetComponent<CharacterController>();

        health = GetComponent<Health>();

        lookSpeedX = lookSpeedXNormal;
        lookSpeedY = lookSpeedYNormal;

    }

    private void OnEnable(){

        health.OnDeath += Death;

        inputActions.Player.Look.performed += Look;

        inputActions.Player.Sprint.started += SprintEnable;
        inputActions.Player.Sprint.canceled += SprintDisable;

        inputActions.Player.Jump.started += JumpStart;
        inputActions.Player.Jump.canceled += JumpCancel;

        inputActions.Player.Shoot.performed += Shoot;
        inputActions.Player.Scope.performed += Scope;

        inputActions.Player.SwitchWeapon.performed += SwitchWeapon;

        inputActions.Player.Interact.performed += CollectCurrentFocusBox;

        inputActions.Player.Enable();

        Cursor.lockState = CursorLockMode.Locked;

    }

    private void OnDisable(){

        inputActions.Player.Disable();

        health.OnDeath -= Death;

        inputActions.Player.Look.performed -= Look;

        inputActions.Player.Sprint.started -= SprintEnable;
        inputActions.Player.Sprint.canceled -= SprintDisable;

        inputActions.Player.Jump.started -= JumpStart;
        inputActions.Player.Jump.canceled -= JumpCancel;

        inputActions.Player.Shoot.performed -= Shoot;
        inputActions.Player.Scope.performed -= Scope;

        inputActions.Player.SwitchWeapon.performed -= SwitchWeapon;

        inputActions.Player.Interact.performed -= CollectCurrentFocusBox;

        Cursor.lockState = CursorLockMode.None;

    }

    private void Death(){
        SceneManager.LoadScene(0);
    }

    private void SwitchWeapon(InputAction.CallbackContext ctx){
        OnWeaponChange?.Invoke((int)ctx.ReadValue<Vector2>().y >> -1 | 1);
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
        OnShoot?.Invoke(transform);
    }

    private void Scope(InputAction.CallbackContext ctx){
        IsScoping ^= true;
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

    private void ChangeLookSpeed(bool isScoping){
        if(isScoping){
            lookSpeedX = lookSpeedXScoping;
            lookSpeedY = lookSpeedYScoping;
        }
        else{
            lookSpeedX = lookSpeedXNormal;
            lookSpeedY = lookSpeedYNormal;
        }
    }

    private void CollectCurrentFocusBox(InputAction.CallbackContext ctx){

        if(currentFocusBox != null){

            OnCollectBullets?.Invoke(currentFocusBox.BulletCount);
            currentFocusBox.Collect();
            currentFocusBox = null;

        }

    }


    private void Update(){

        Move(inputActions.Player.Move.ReadValue<Vector2>());

        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        if(Physics.Raycast(ray, out RaycastHit hit, bulletBoxFocusDistance) && hit.collider.gameObject.TryGetComponent(out BulletBox bulletBox)){

            bulletBox.Highlight(mainCamera.transform);
            currentFocusBox = bulletBox;

        }
        else if(currentFocusBox != null){

            currentFocusBox.Lowlight();
            currentFocusBox = null;

        }

    }

}
