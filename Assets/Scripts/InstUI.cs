using UnityEngine;

public class InstUI : MonoBehaviour
{
    public void OnBackToMenuPressed()
    {
        GameManager.Instance.LoadMenuScene();
    }
}
