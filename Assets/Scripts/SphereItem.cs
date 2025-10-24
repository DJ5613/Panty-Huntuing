using UnityEngine;

public class SphereItem : MonoBehaviour
{
    public SphereCollector collector;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && collector != null)
        {
            collector.CollectObject(gameObject);
        }
    }
}