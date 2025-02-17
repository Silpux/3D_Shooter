using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour{

    private Player player;

    private Animator playerAnimator;
    private Animator cameraAnimator;

    private List<Weapon> weapons;


    private int currentWeaponIndex;

    private int CurrentWeaponIndex{
        get => currentWeaponIndex;
        set{
            weapons[currentWeaponIndex].gameObject.SetActive(false);
            currentWeaponIndex = value;
            weapons[currentWeaponIndex].gameObject.SetActive(true);

            IsReloading = false;
            ReloadTimerImage.fillAmount = 1;

            UpdateUI();
        }
    }

    private Weapon CurrentWeapon => weapons[CurrentWeaponIndex];

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

        for(int i=0;i<hand.transform.childCount;i++){
            weapons.Add(hand.GetChild(i).gameObject.GetComponent<Weapon>());
        }

        CurrentWeaponIndex = 0;

    }

    private void OnEnable(){
        player.OnShoot += Shoot;
        player.OnScope += Scope;

        player.OnWeaponChange += SwitchWeapon;
    }

    private void OnDisable(){
        player.OnShoot -= Shoot;
        player.OnScope -= Scope;

        player.OnWeaponChange -= SwitchWeapon;
    }

    private void Shoot(){

        if(!IsReloading){

            if(CurrentWeapon.BulletsCurrent > 0){
                CurrentWeapon.Shoot();
            }

            if(CurrentWeapon.BulletsCurrent <= 0){
                reloadTimer = CurrentWeapon.ReloadingTimerMax;
                IsReloading = true;
            }

            UpdateUI();

        }

    }

    private void SwitchWeapon(int direction){

        CurrentWeaponIndex = (CurrentWeaponIndex + direction + weapons.Count) % weapons.Count;

    }

    private void Scope(){
        IsScoping ^= true;
    }
    
    private void FixedUpdate(){

        if(IsReloading){

            reloadTimer--;

            ReloadTimerImage.fillAmount = 1 - (float)reloadTimer / CurrentWeapon.ReloadingTimerMax;

            if(reloadTimer <= 0){

                CurrentWeapon.Reload();
                IsReloading = false;
                UpdateUI();

            }
    
        }
    
    }

    private void UpdateUI(){

        currentBulltesText.text = $"{CurrentWeapon.BulletsCurrent}/{CurrentWeapon.BulletsMax}";
        totalBulltesText.text =$"{CurrentWeapon.BulletsTotal}";

    }

}
