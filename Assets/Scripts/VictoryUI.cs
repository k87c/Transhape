using UnityEngine;

public class VictoryUI : MonoBehaviour
{
    public void OnNextLevelPressed()
    {
        GameManager.Instance.LoadLevel02Scene();
    }

    public void OnBackToMenuPressed()
    {
        GameManager.Instance.LoadMenuScene();
    }
}
