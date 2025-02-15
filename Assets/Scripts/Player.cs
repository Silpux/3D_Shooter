using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour{


    private CharacterController characterController;

    private InputActions inputActions;

    [SerializeField] private Camera mainCamera;

    [SerializeField] private float lookSpeedX;
    [SerializeField] private float lookSpeedY;

    [SerializeField] private float lookLimitY = 90;

    private float cameraRotation;

    private void Awake(){

        inputActions = new InputActions();

        inputActions.Player.Enable();

        characterController = GetComponent<CharacterController>();

    }

    private void OnEnable(){
        inputActions.Player.Look.performed += Look;
    }

    private void OnDisable(){
        inputActions.Player.Look.performed -= Look;

    }

    private void Look(InputAction.CallbackContext ctx){

        Vector2 value = ctx.ReadValue<Vector2>() / 10;

        cameraRotation += value.y * lookSpeedY;
        cameraRotation = Mathf.Clamp(cameraRotation, -lookLimitY, lookLimitY);
        
        Debug.Log(cameraRotation);

        mainCamera.transform.localRotation = Quaternion.Euler(-cameraRotation,0,0);

        transform.rotation *= Quaternion.Euler(0, value.x * lookSpeedX, 0);

    }


    private void Update(){

    }

}
