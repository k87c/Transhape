using System.Collections;
using UnityEngine;

public class FInalUI : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(WaitAndReturnToMenu());
    }

    IEnumerator WaitAndReturnToMenu()
    {
        yield return new WaitForSeconds(4f);
        GameManager.Instance.LoadMenuScene();
    }
    
}
