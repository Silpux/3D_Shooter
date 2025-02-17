using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour{

    private Player player;

    private Animator playerAnimator;
    private Animator cameraAnimator;

    private List<Weapon> weapons;
    public Camera mainCamera;

    private bool isScoping = false;
    private bool isReloading = false;

    private bool IsScoping{
        get => isScoping;
        set{
            cameraAnimator.SetBool(SCOPING_ANIMATION_NAME, !IsReloading && value);
            playerAnimator.SetBool(SCOPING_ANIMATION_NAME, !IsReloading && value);

            isScoping = value;
        }
    }

    private bool IsReloading{
        get => isReloading;
        set{
            if(value){
                cameraAnimator.SetBool(SCOPING_ANIMATION_NAME, false);
            }
            else if(IsScoping){
                cameraAnimator.SetBool(SCOPING_ANIMATION_NAME, true);
            }

            playerAnimator.SetBool(RELOADING_ANIMATION_NAME, value);

            isReloading = value;
        }
    }


    private int currentWeapon;

    private int reloadTimer;

    private const string SCOPING_ANIMATION_NAME = "IsScoping";
    private const string RELOADING_ANIMATION_NAME = "IsReloading";


    [SerializeField] private Transform hand;
    [SerializeField] private TMP_Text currentBulltesText;
    [SerializeField] private TMP_Text totalBulltesText;

    [SerializeField] private Image ReloadTimerImage;
    [SerializeField] private Image ReloadTimerImageBg;

    private void Awake(){
        
        player = GetComponent<Player>();
        playerAnimator = hand.GetComponent<Animator>();

        cameraAnimator = mainCamera.GetComponent<Animator>();
        
    }
    private void Start(){


        weapons = new List<Weapon>();
        currentWeapon = 0;

        for(int i=0;i<hand.transform.childCount;i++){
            weapons.Add(hand.GetChild(i).gameObject.GetComponent<Weapon>());
        }

        UpdateUI();

    }

    private void OnEnable(){
        player.OnShoot += Shoot;
        player.OnScope += Scope;
    }

    private void OnDisable(){
        player.OnShoot -= Shoot;
        player.OnScope -= Scope;
    }

    private void Shoot(){

        if(!IsReloading){

            if(weapons[currentWeapon].BulletsCurrent > 0){
                weapons[currentWeapon].Shoot();
            }

            if(weapons[currentWeapon].BulletsCurrent <= 0){
                reloadTimer = weapons[currentWeapon].ReloadingTimerMax;
                IsReloading = true;
            }

            UpdateUI();
        }

    }

    private void Scope(){

        IsScoping = !IsScoping;

    }

    private void Update(){
        
    }

    
    private void FixedUpdate(){

        if(IsReloading){

            reloadTimer--;

            ReloadTimerImage.fillAmount = 1 - (float)reloadTimer / weapons[currentWeapon].ReloadingTimerMax;

            if(reloadTimer <= 0){

                weapons[currentWeapon].Reload();
                IsReloading = false;
                UpdateUI();

            }
    
        }
    
    }

    private void UpdateUI(){

        currentBulltesText.text = $"{weapons[currentWeapon].BulletsCurrent}/{weapons[currentWeapon].BulletsMax}";
        totalBulltesText.text =$"{weapons[currentWeapon].BulletsTotal}";

    }

}
