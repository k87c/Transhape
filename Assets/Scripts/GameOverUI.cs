using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public void OnRestartPressed()
    {
        GameManager.Instance.RestartLastScene();
    }

    public void OnBackToMenuPressed()
    {
        GameManager.Instance.LoadMenuScene();
    }
}
