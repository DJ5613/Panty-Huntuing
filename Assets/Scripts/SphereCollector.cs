using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SphereCollector : MonoBehaviour
{
    [Header("Settings")]
    public GameObject objectToSpawn; // ������ ��� ������ (���������� � ����������)
    public Transform[] spawnPoints = new Transform[10]; // 10 ����� ������
    public TMP_Text counterText; // ������ �� UI �����

    private int collectedCount = 0;
    private List<GameObject> activeObjects = new List<GameObject>();

    void Start()
    {
        SpawnObjects();
    }

    void SpawnObjects()
    {
        // ���������, ������ �� ������ ��� ������
        if (objectToSpawn == null)
        {
            Debug.LogError("No object to spawn assigned!");
            return;
        }

        // ������� �� ������ ������� � ������ �����
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
        // ������� ������
        GameObject newObject = Instantiate(objectToSpawn, position, Quaternion.identity);

        // ��������� ������ ��� �����
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

    // ����� ��� ����������� (����� ������� �� ������ ��������)
    public void RespawnObjects()
    {
        // ������� ������ �������
        foreach (GameObject obj in activeObjects)
        {
            if (obj != null)
                Destroy(obj);
        }
        activeObjects.Clear();
        collectedCount = 0;

        // ������� �����
        SpawnObjects();
    }
}