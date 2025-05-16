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
    public void LoadLevel01Scene()
    {
        Debug.Log("Carga Level01.");
        LoadScene("Level01"); // Aseg�rate de que el nombre coincida con el de tu escena
    }

    public void LoadInstScene()
    {
        Debug.Log("Carga Inst.");
        LoadScene("Inst"); 
    }

    public void LoadMenuScene()
    {
        Debug.Log("Carga Menu.");
        LoadScene("Menu"); 
    }

    public void QuitGame()
    {   
        Debug.Log("Cerrando el juego...");
        Application.Quit();
    }

    public void GoToGameOver()
    {
        // Guarda la escena activa antes de ir al GameOver
        lastSceneBeforeGameOver = SceneManager.GetActiveScene().name;
        Debug.Log("Escena guardada antes de GameOver: " + lastSceneBeforeGameOver);
        SceneManager.LoadScene("GameOver");
    }

    public void GoToVictory() {
        SceneManager.LoadScene("Victory");
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
