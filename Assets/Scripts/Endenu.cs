using LogitechG29.Sample.Input;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Edenu : MonoBehaviour
{
    public InputControllerReader inputControllerReader;
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(0);
        Debug.Log("Загрузка сцены");
    }

    private void Update()
    {
        if (inputControllerReader.LeftShift) PlayGame();
    }
}