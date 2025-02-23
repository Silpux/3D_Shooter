using UnityEngine;

public class Weapon : MonoBehaviour{

    [SerializeField] private int bulletsTotal;
    [SerializeField] private int bulletsMax;
    [SerializeField] private int bulletsInitial;

    public int BulletsTotal{get; set;}
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

    public void Shoot(Transform player){

        BulletsCurrent--;

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        if(Physics.Raycast(ray, out RaycastHit hit, shootDistance)){

            GameObject ps = Instantiate(particlePrefab, hit.point, Quaternion.identity);

            Destroy(ps, 1f);

            if(hit.collider.gameObject.TryGetComponent(out Health health)){
                health.Damage(damage);
            }
            if(hit.collider.gameObject.TryGetComponent(out Alien alien)){
                alien.Hit(player);
            }
            if(hit.collider.gameObject.TryGetComponent(out Rigidbody rb)){
                rb.AddForce(ray.direction * 15f, ForceMode.Impulse);
            }

        }

    }

    public void Reload(){

        BulletsCurrent = BulletsMax > BulletsTotal ? BulletsTotal : BulletsMax;
        BulletsTotal -= BulletsCurrent;

    }

}
