using Bhaptics.SDK2;
using UnityEngine;

public class LeftTrig : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Enemy")
        {
            Debug.Log("AAAAAAAAAAAAA");
            BhapticsLibrary.Play(eventId: BhapticsEvent.LEFT);
        }
    }
}
