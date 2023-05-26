using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    public GameObject currentCanvas; // —сылка на текущий Canvas
    public GameObject nextCanvas; // —сылка на Canvas, который нужно активировать

    public void OpenMenuScene()
    {
        currentCanvas.SetActive(false); // ƒеактивируем текущий Canvas
        nextCanvas.SetActive(true); // јктивируем следующий Canvas
    }
}
