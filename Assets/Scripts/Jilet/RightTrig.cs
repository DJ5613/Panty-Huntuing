using Bhaptics.SDK2;
using UnityEngine;

public class RightTrig : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Enemy")
        {
            Debug.Log("AAAAAAAAAAAAA");
            BhapticsLibrary.Play(eventId: BhapticsEvent.RIGHT);
        }
    }
}
