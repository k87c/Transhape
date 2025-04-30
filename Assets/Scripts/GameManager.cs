using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Método público que puedes llamar desde los botones
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // También puedes agregar métodos específicos si prefieres
    public void LoadTrainingScene()
    {
        LoadScene("Training"); // Asegúrate de que el nombre coincida con el de tu escena
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

}
