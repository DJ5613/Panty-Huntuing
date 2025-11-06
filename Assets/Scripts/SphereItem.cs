using UnityEngine;
using UnityEngine.Audio;

public class SphereItem : MonoBehaviour
{
    public SphereCollector collector;
    [SerializeField] private AudioClip collectSound; // Звук сбора
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = collectSound;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && collector != null)
        {
            GetComponent<Renderer>().enabled = false;
            if (GetComponent<Collider>() != null)
                GetComponent<Collider>().enabled = false;
            if (audioSource != null && collectSound != null)
            {
                audioSource.Play();
            }
            float destroyDelay = collectSound != null ? collectSound.length : 0.1f;
            collector.CollectObject(gameObject, destroyDelay);
        }
    }
}