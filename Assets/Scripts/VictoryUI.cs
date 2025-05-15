using UnityEngine;

public class VictoryUI : MonoBehaviour
{
    public void OnNextLevelPressed()
    {
        
    }

    public void OnBackToMenuPressed()
    {
        GameManager.Instance.LoadMenuScene();
    }
}
