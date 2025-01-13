using System.Collections.Generic;
using UnityEngine;

namespace Match.Skills
{
    public class BombSkill : MonoBehaviour
    {
        public float pushForce = 10f; // İtme kuvveti

        [SerializeField] // Inspector'da görünmesi için
        private ItemSpawner itemSpawner;

        [SerializeField] // Partikül prefab'ı
        private GameObject particlePrefab;

        public void UseBombSkill()
        {
            if (itemSpawner == null)
            {
                Debug.LogError("ItemSpawner is not assigned!");
                return;
            }

            if (particlePrefab == null)
            {
                Debug.LogError("Particle prefab is not assigned!");
                return;
            }

            // Sahnedeki tüm aktif item'leri al
            List<Item> items = itemSpawner.GetItems();
            if (items == null || items.Count == 0)
            {
                Debug.LogWarning("No items found to push!");
                return;
            }

            Vector3 centerPosition = itemSpawner.transform.position; // Kuvvetin merkez noktası

            // Her item'e merkezden dışa doğru kuvvet uygula ve partikül oluştur
            foreach (var item in items)
            {
                if (item == null || item.transform == null)
                    continue;

                Rigidbody rb = item.GetComponent<Rigidbody>();
                if (rb == null)
                {
                    Debug.LogWarning($"Item {item.name} has no Rigidbody!");
                    continue;
                }

                // Merkezden dışa doğru yön hesapla
                Vector3 direction = (item.transform.position - centerPosition).normalized;

                // Kuvvet uygula
                rb.isKinematic = false; // Fizik etkisini aktif et
                rb.AddForce(direction * pushForce, ForceMode.Impulse);

                // Partikül efekti oluştur
                SpawnParticleEffect(item.transform.position);
            }
        }

        private void SpawnParticleEffect(Vector3 position)
        {
            // Partikül prefab'ını belirtilen pozisyonda oluştur
            GameObject particle = Instantiate(particlePrefab, position, Quaternion.identity);

            // Partikülün belirli bir süre sonra yok edilmesi
            Destroy(particle, 3f); // Örneğin 3 saniye sonra
        }
    }
}
