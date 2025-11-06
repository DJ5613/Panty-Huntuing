using Bhaptics.SDK2;
using UnityEngine;

public class CarBOOM : MonoBehaviour
{
    [Header("Настройки сбора")]
    [SerializeField] private AudioClip collectSound; // Звук сбора

    [Header("Визуальные эффекты")]
    [SerializeField] private GameObject explosionEffect; // Префаб взрыва
    [SerializeField] private bool destroyAfterEffect = true; // Уничтожать ли объект после эффекта

    public bool alive = true;

    [Header("Настройки звука")]
    [SerializeField] private float soundVolume = 1f;

    private AudioSource audioSource;
    private bool isCollected = false;

    private void Start()
    {
        // Настройка AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = collectSound;
        audioSource.volume = soundVolume;

        // Проверяем наличие коллайдера
        if (GetComponent<Collider>() == null)
        {
            Debug.LogError("Объект " + gameObject.name + " не имеет коллайдера!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            BhapticsLibrary.Play(eventId: BhapticsEvent.BOOM);
            Collect();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isCollected)
        {
            Collect();
        }
    }

    private void Collect()
    {
        isCollected = true;

        // Создаем эффект взрыва
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }

        // Проигрываем звук
        if (audioSource != null && collectSound != null)
        {
            audioSource.Play();
        }

        // Отключаем визуальную часть и коллайдер
        GetComponent<Renderer>().enabled = false;
        if (GetComponent<Collider>() != null)
            GetComponent<Collider>().enabled = false;

        // Отключаем физику если есть Rigidbody
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = true;

        // Уничтожаем объект после завершения звука и эффекта
        if (destroyAfterEffect)
        {
            float destroyDelay = collectSound != null ? collectSound.length : 0.1f;
            Destroy(gameObject, destroyDelay);
        }
    }

    // Метод для принудительного вызова сбора (можно вызывать из других скриптов)
    public void ForceCollect()
    {
        if (!isCollected)
        {
            Collect();
        }
    }
}