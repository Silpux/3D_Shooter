using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour{


    private CharacterController characterController;

    private InputActions inputActions;

    [SerializeField] private Camera mainCamera;

    [SerializeField] private float lookSpeedX;
    [SerializeField] private float lookSpeedY;

    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    [SerializeField] private float lookLimitY = 90;

    private float cameraRotation;
    private bool sprint;
    private Vector2 moveDirection = Vector2.zero;


    private void Awake(){

        inputActions = new InputActions();

        inputActions.Player.Enable();

        characterController = GetComponent<CharacterController>();

    }

    private void OnEnable(){
        inputActions.Player.Look.performed += Look;

        inputActions.Player.Sprint.started += SprintEnable;
        inputActions.Player.Sprint.canceled += SprintDisable;

    }

    private void OnDisable(){
        inputActions.Player.Look.performed -= Look;

        inputActions.Player.Sprint.started -= SprintEnable;
        inputActions.Player.Sprint.canceled -= SprintDisable;
    }

    private void SprintEnable(InputAction.CallbackContext ctx){
        sprint = true;
    }
    private void SprintDisable(InputAction.CallbackContext ctx){
        sprint = false;
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
            return;
        }

        Vector3 vectorForward = direction.y * transform.TransformDirection(Vector3.forward);
        Vector3 vectorRigth = direction.x * transform.TransformDirection(Vector3.right);

        Vector3 moveDirection = (vectorForward + vectorRigth) * (sprint ? runSpeed : walkSpeed);

        characterController.Move(moveDirection * Time.deltaTime);


    }


    private void Update(){

        Move(inputActions.Player.Move.ReadValue<Vector2>());

    }

}
