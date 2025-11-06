using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SphereCollector : MonoBehaviour
{

    [Header("Settings")]
    public GameObject objectToSpawn; // Объект для спавна (выбирается в инспекторе)
    public Transform[] spawnPoints = new Transform[10]; // 10 точек спавна
    public TMP_Text counterText; // Ссылка на UI текст
    public Canvas endGame;
    public TMP_Text endText;
    public AudioSource source;

    public int collectedCount = 0;
    private List<GameObject> activeObjects = new List<GameObject>();

    void Start()
    {
        SpawnObjects();
    }

    void SpawnObjects()
    {
        // Проверяем, выбран ли объект для спавна
        if (objectToSpawn == null)
        {
            Debug.LogError("No object to spawn assigned!");
            return;
        }

        // Спавним по одному объекту в каждой точке
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (spawnPoint != null)
            {
                CreateObject(spawnPoint.position);
            }
        }
        UpdateCounter();
    }

    void CreateObject(Vector3 position)
    {
        position.y =  1;
        // Создаем объект
        GameObject newObject = Instantiate(objectToSpawn, position, Quaternion.identity);

        // Добавляем скрипт для сбора
        SphereItem item = newObject.GetComponent<SphereItem>();
        item.collector = this;

        activeObjects.Add(newObject);
    }

    public void CollectObject(GameObject collectedObject, float destroyDelay)
    {
        collectedCount++;
        activeObjects.Remove(collectedObject);
        Destroy(collectedObject, destroyDelay);
        UpdateCounter();
        if (collectedCount >= 7)
        {
            endGame.gameObject.SetActive(true);
            source.Play();
            endText.text = "Вы выиграли! Нажмите LeftShift для выхода в меню";
            Time.timeScale = 0f;
        }
    }

    void UpdateCounter()
    {
        if (counterText != null)
        {
            counterText.text = $"Collected: {collectedCount}/7";
        }
    }
}