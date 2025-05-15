using UnityEngine;

public class MenuUI : MonoBehaviour
{
    public void OnStartPressed()
    {
        GameManager.Instance.LoadLevel01Scene();
    }

    public void OnInstPressed()
    {
        GameManager.Instance.LoadInstScene();
    }

    public void OnQuitPressed()
    {
        GameManager.Instance.QuitGame();
    }
}
