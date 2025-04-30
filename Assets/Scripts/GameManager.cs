using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // M�todo p�blico que puedes llamar desde los botones
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Tambi�n puedes agregar m�todos espec�ficos si prefieres
    public void LoadTrainingScene()
    {
        LoadScene("Training"); // Aseg�rate de que el nombre coincida con el de tu escena
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

}
