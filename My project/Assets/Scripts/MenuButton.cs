using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    public GameObject currentCanvas; // ������ �� ������� Canvas
    public GameObject nextCanvas; // ������ �� Canvas, ������� ����� ������������

    public void OpenMenuScene()
    {
        currentCanvas.SetActive(false); // ������������ ������� Canvas
        nextCanvas.SetActive(true); // ���������� ��������� Canvas
    }
}
