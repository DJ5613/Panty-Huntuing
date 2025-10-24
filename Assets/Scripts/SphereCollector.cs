using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SphereCollector : MonoBehaviour
{
    [Header("Settings")]
    public GameObject objectToSpawn; // Объект для спавна (выбирается в инспекторе)
    public Transform[] spawnPoints = new Transform[10]; // 10 точек спавна
    public TMP_Text counterText; // Ссылка на UI текст

    private int collectedCount = 0;
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
        position.y =  (float) 9.5;
        // Создаем объект
        GameObject newObject = Instantiate(objectToSpawn, position, Quaternion.identity);

        // Добавляем скрипт для сбора
        SphereItem item = newObject.AddComponent<SphereItem>();
        item.collector = this;

        activeObjects.Add(newObject);
    }

    public void CollectObject(GameObject collectedObject)
    {
        collectedCount++;
        activeObjects.Remove(collectedObject);
        Destroy(collectedObject);
        UpdateCounter();
    }

    void UpdateCounter()
    {
        if (counterText != null)
        {
            counterText.text = $"Collected: {collectedCount}/10";
        }
    }

    // Метод для перезапуска (можно вызвать из других скриптов)
    public void RespawnObjects()
    {
        // Очищаем старые объекты
        foreach (GameObject obj in activeObjects)
        {
            if (obj != null)
                Destroy(obj);
        }
        activeObjects.Clear();
        collectedCount = 0;

        // Создаем новые
        SpawnObjects();
    }
}