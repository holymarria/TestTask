using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioSource audioSource;

    private void Start()
    {
        audioSource.Play(); 
    }
}