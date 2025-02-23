using UnityEngine;

public class AlienSpawner : MonoBehaviour{

    [SerializeField] private Alien alienPrefab;

    [SerializeField] private float spawnTime;

    private float timeToSpawn;

    private void Start(){
        timeToSpawn = spawnTime * Random.Range(0.85f, 1.15f);
    }

    private void Update(){
        
        timeToSpawn -= Time.deltaTime;

        if(timeToSpawn <= 0){

            timeToSpawn = spawnTime * Random.Range(0.85f, 1.15f);

            SpawnEnemy();

        }

    }

    private void SpawnEnemy(){

        if(Alien.TotalCount < 100){
            Instantiate(alienPrefab, transform.position, Quaternion.identity);
        }

    }

}
