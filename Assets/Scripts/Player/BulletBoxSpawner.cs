using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BulletBoxSpawner : MonoBehaviour{

    private BoxCollider boxCollider;

    [SerializeField] private BulletBox bulletBox;
    [SerializeField] private float spawnTime;

    private float timeToSpawn;

    private void Awake(){
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Start(){
        timeToSpawn = spawnTime * Random.Range(0.85f, 1.15f);
    }

    private void Update(){

        timeToSpawn -= Time.deltaTime;

        if(timeToSpawn <= 0){

            timeToSpawn = spawnTime * Random.Range(0.85f, 1.15f);

            if(BulletBox.Count <= 200){
                SpawnBulletBox();
            }

        }

    }

    private void SpawnBulletBox(){

        Vector3 center = boxCollider.bounds.center;
        Vector3 extents = boxCollider.bounds.extents;

        float x = Random.Range(center.x - extents.x, center.x + extents.x);
        float y = Random.Range(center.y - extents.y, center.y + extents.y);
        float z = Random.Range(center.z - extents.z, center.z + extents.z);

        BulletBox newBox = Instantiate(bulletBox, new Vector3(x, y, z), Quaternion.identity);
        newBox.GetComponent<Rigidbody>().AddTorque(Random.insideUnitSphere * 5f, ForceMode.Impulse);

    }

}
