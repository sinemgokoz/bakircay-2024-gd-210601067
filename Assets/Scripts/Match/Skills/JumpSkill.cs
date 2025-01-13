using System.Collections.Generic;
using UnityEngine;

namespace Match.Skills
{
    public class JumpSkill : MonoBehaviour
    {
        public float jumpForce = 5f; // Zıplama kuvveti

        [SerializeField] // Inspector'da görünmesi için
        private ItemSpawner itemSpawner;

        [SerializeField] // Partikül prefab'ı
        private GameObject particlePrefab;

        // Skill'i çalıştıran ana metot
        public void UseJumpSkill()
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
                Debug.LogWarning("No items found to jump!");
                return;
            }

            // Her item'e yukarı doğru zıplama kuvveti uygula ve partikül oluştur
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

                // Partikül efekti oluştur
                SpawnParticleEffect(item.transform.position);

                // Zıplama kuvveti uygula
                rb.isKinematic = false; // Fizik etkisini aktif et
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
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
