using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetection : MonoBehaviour
{
    private List<Collider> detectedObjects = new List<Collider>();
    private bool isOccupied = false; // Alanın doluluk durumunu kontrol eder
    private Vector3 ejectDirection = new Vector3(0f, 0.5f, 1f).normalized;
    private float ejectForce = 10f;

    private void OnTriggerEnter(Collider other)
    {
        // Eğer alan doluysa yeni nesneler kabul edilmez
        if (isOccupied)
        {
            EjectObject(other); // Yeni nesneyi dışarı fırlat
            return;
        }

        if (IsMatchableTag(other.tag))
        {
            detectedObjects.Add(other);
            isOccupied = true; // Alanın dolu olduğunu işaretle
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Nesne tetikleme alanından çıkarsa, alanın boş olduğunu işaretle
        if (detectedObjects.Contains(other))
        {
            detectedObjects.Remove(other);
            isOccupied = false; // Alan boşaldı
        }
    }

    private void EjectObject(Collider obj)
    {
        Rigidbody rb = obj.attachedRigidbody;
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.AddForce(ejectDirection * ejectForce, ForceMode.Impulse);
        }
        else
        {
            obj.transform.position += ejectDirection * ejectForce;
        }
    }

    private bool IsMatchableTag(string tag)
    {
        return tag == "cubeMatchable" || tag == "circleMatchable" || tag == "capsuleMatchable";
    }
}
