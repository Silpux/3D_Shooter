using UnityEngine;

public class Weapon : MonoBehaviour{

    [SerializeField] private int bulletsTotal;
    [SerializeField] private int bulletsMax;
    [SerializeField] private int bulletsInitial;

    public int BulletsTotal{get;private set;}
    public int BulletsMax{get;private set;}
    public int BulletsCurrent{get;private set;}

    [SerializeField] private int reloadingTimerMax;

    public int ReloadingTimerMax => reloadingTimerMax;

    private void Awake(){
        BulletsTotal = bulletsTotal;
        BulletsMax = bulletsMax;
        BulletsCurrent = bulletsInitial;
    }

    [SerializeField] private float damage;
    [SerializeField] private float shootDistance;

    [SerializeField] private GameObject particlePrefab;

    public void Shoot(){

        BulletsCurrent--;

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, shootDistance)){

            GameObject ps = Instantiate(particlePrefab, hit.point, Quaternion.identity);

            Destroy(ps, 1f);

        }

    }

    public void Reload(){

        BulletsCurrent = bulletsMax > bulletsTotal ? bulletsTotal : bulletsMax;
        bulletsTotal -= BulletsCurrent;

    }

}
