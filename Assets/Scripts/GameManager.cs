using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}

    private string lastSceneBeforeGameOver; //guarda la ultima escena jugada

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadLevel01Scene()
    {
        Debug.Log("Carga Level01.");
        LoadScene("Level01");
    }
    
    public void LoadLevel02Scene()
    {
        Debug.Log("Carga Level02.");
        LoadScene("Level02"); 
    }

    public void LoadInstScene()
    {
        Debug.Log("Carga Inst.");
        LoadScene("Inst");
    }
    
    public void LoadCreditsScene()
    {
        Debug.Log("Carga Credits.");
        LoadScene("Credits");
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

    public void GoToFinal() {
        SceneManager.LoadScene("Final");
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
