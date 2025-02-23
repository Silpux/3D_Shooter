using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuCanvas : MonoBehaviour{


    public void OpenGame(){
        SceneManager.LoadScene(1);
    }

    public void Exit(){

#if UNITY_EDITOR

        UnityEditor.EditorApplication.isPlaying = false;

#else

        Application.Quit();

#endif

    }

}
