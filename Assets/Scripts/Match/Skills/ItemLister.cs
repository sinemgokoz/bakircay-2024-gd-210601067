using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI elemanlarını kullanmak için gerekli
using TMPro;

namespace Match.Skills
{
    public class ItemLister : MonoBehaviour
    {
        [SerializeField] // Inspector'da görünmesi için
        private ItemSpawner itemSpawner;

        [SerializeField]
        private TextMeshProUGUI itemListText;

        public void UpdateItemList()
        {
            if (itemSpawner == null)
            {
                Debug.LogError("ItemSpawner is not assigned!");
                return;
            }

            if (itemListText == null)
            {
                Debug.LogError("Text component is not assigned!");
                return;
            }

            // Aktif item'leri al
            List<Item> items = itemSpawner.GetItems();
            if (items == null || items.Count == 0)
            {
                itemListText.text = "No active items found!";
                ClearTextAfterDelay(5f); // 5 saniye sonra temizle
                return;
            }

            // İsimleri birleştir ve Text'e yaz
            string itemNames = "Active Items:\n";
            foreach (var item in items)
            {
                if (item != null)
                {
                    itemNames += $"- {item.name}\n";
                }
            }

            itemListText.text = itemNames;

            // 5 saniye sonra metni temizle
            ClearTextAfterDelay(5f);
        }

        private void ClearTextAfterDelay(float delay)
        {
            // Belirtilen süre sonunda metni temizle
            Invoke(nameof(ClearText), delay);
        }

        private void ClearText()
        {
            itemListText.text = string.Empty; // Metni temizle
        }
    }
}