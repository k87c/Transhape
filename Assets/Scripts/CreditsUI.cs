using UnityEngine;

public class CreditsUI : MonoBehaviour
{
    public void OnBackToMenuPressed()
    {
        GameManager.Instance.LoadMenuScene();
    }
}
