using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetection : MonoBehaviour
{
    private List<GameObject> currentObjects = new List<GameObject>(); // Alandaki mevcut nesneleri takip eder
    public float ejectForce = 10f; // F�rlatma kuvveti
    public float shieldEjectForce = 15f; // Kalkan�n f�rlatma kuvveti
    private bool isTriggerActive = true; // Trigger durumunu takip eder

    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggerActive)
        {
            // E�er trigger kapal�ysa nesneyi kabul etme
            return;
        }

        if (currentObjects.Count > 0)
        {
            // E�er alanda zaten bir nesne varsa yeni gelen nesneyi kalkanla f�rlat
            ActivateShield(other);
            return;
        }

        if (other.attachedRigidbody != null)
        {
            if (currentObjects.Contains(other.gameObject))
            {
                // E�er nesne zaten alandaysa i�lem yapma
                return;
            }

            // Gelen objeyi alan�n ortas�na yerle�tir ve listeye ekle
            PlaceObjectInCenter(other);
        }
    }

    private void PlaceObjectInCenter(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        rb.isKinematic = true; // Fizik etkilerini devre d��� b�rak
        Vector3 centerPosition = transform.position;
        rb.transform.position = centerPosition;
        rb.transform.rotation = Quaternion.identity; // Dik yerle�tir

        // Obje ba�ar�yla yerle�tirildi, listeye ekle
        currentObjects.Add(other.gameObject);
    }

    private void EjectObject(GameObject obj, float force)
    {
        // D��ar� atmak i�in fizik kuvveti uygular.
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false; // Fizik etkile�imini a�

            // Rasgele bir y�n olu�tur
            Vector3 randomDirection = new Vector3(
                Random.Range(-1f, 1f), // X ekseni
                Random.Range(0.5f, 1f), // Y ekseni
                Random.Range(-1f, 1f)  // Z ekseni
            ).normalized; // Y�n� normalize ederek birim vekt�r yap

            rb.AddForce(randomDirection * force, ForceMode.Impulse); // Kuvvet uygula
        }
        else
        {
            // E�er Rigidbody yoksa pozisyonu do�rudan g�ncelle.
            obj.transform.position += new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(0.5f, 1f),
                Random.Range(-1f, 1f)
            ).normalized * force;
        }
    }

    private void ActivateShield(Collider incomingObject)
    {
        // Kalkan gelen nesneyi farkl� bir y�ne f�rlat�r
        Debug.Log("Kalkan aktif: Yeni nesne f�rlat�l�yor!");
        EjectObject(incomingObject.gameObject, shieldEjectForce);
    }

    private void OnMouseDown()
    {
        if (currentObjects.Count > 0)
        {
            // Trigger'� kapat ve i�indeki nesneyi d��ar� at
            isTriggerActive = false;
            GetComponent<Collider>().isTrigger = false;

            GameObject objectToEject = currentObjects[0];
            currentObjects.Remove(objectToEject);
            EjectObject(objectToEject, ejectForce);

            // Tekrar aktif duruma getirmek i�in zaman� geciktirebilirsiniz
            StartCoroutine(ReactivateTrigger());
        }
    }

    private IEnumerator ReactivateTrigger()
    {
        yield return new WaitForSeconds(1f); // 1 saniye bekle
        isTriggerActive = true;
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (currentObjects.Contains(other.gameObject))
        {
            // E�er nesne alan� terk ettiyse listeden ��kar
            currentObjects.Remove(other.gameObject);
        }
    }
}