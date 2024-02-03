using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private string mainLevelName = "MainScene";
    
    public void StartGame()
    {
        SceneManager.LoadScene(mainLevelName);
    }
}
