using UnityEngine;

public class BulletBox : MonoBehaviour{

    public static int Count{get; private set;}
    private Outline outline;

    [field: SerializeField] public int BulletCount{get; private set;}
    [SerializeField] private Canvas hintCanvas;

    private Transform lookAt;
    
    private void Awake(){
        outline = GetComponent<Outline>();
        hintCanvas.gameObject.SetActive(false);
        Count++;
    }

    public void Highlight(Transform lookAt){
        outline.enabled = true;
        this.lookAt = lookAt;
        hintCanvas.gameObject.SetActive(true);
    }

    public void Lowlight(){
        outline.enabled = false;
        this.lookAt = null;
        hintCanvas.gameObject.SetActive(false);
    }

    public void Collect(){

        Destroy(gameObject);
        Count--;

    }

    private void LateUpdate(){

        if(lookAt){
            
            hintCanvas.transform.position = transform.position + Vector3.up;
            hintCanvas.transform.LookAt(transform.position * 2 - lookAt.transform.position);

        }

    }

}
