using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}

    private string lastSceneBeforeGameOver; //guarda la ultima escena jugada

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

    public void LoadMenuScene()
    {
        LoadScene("Menu"); 
    }

    public void GoToGameOver()
    {
        // Guarda la escena activa antes de ir al GameOver
        lastSceneBeforeGameOver = SceneManager.GetActiveScene().name;
        Debug.Log("Escena guardada antes de GameOver: " + lastSceneBeforeGameOver);
        SceneManager.LoadScene("GameOver");
    }

    public void RestartLastScene()
    {
        if (!string.IsNullOrEmpty(lastSceneBeforeGameOver))
        {
            SceneManager.LoadScene(lastSceneBeforeGameOver);
        }
        else
        {
            Debug.LogWarning("No se encontró una escena previa para reiniciar.");
        }
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destruye duplicados
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
