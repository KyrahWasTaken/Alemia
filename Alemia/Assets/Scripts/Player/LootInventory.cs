using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LootInventory : MonoBehaviour
{
    public RectTransform inventory;
    public RectTransform cell;
    public Entity player;
    [Range(0, 1)]
    public float lerp;
    public float searchRadius;

    public bool isInventoryToggled;

    public void CheckInventory(List<LootTable> Loots)
    {
        foreach (Transform child in inventory)
        {
            Destroy(child.gameObject);
        }

        int it = 0;
        foreach (LootTable e in Loots)
        foreach (ItemStack i in e.inventory)
        {
            RectTransform c = Instantiate(cell, transform);
            c.anchoredPosition = new Vector3(0, cell.rect.height * it * -1);
            c.Find("Name").GetComponent<TextMeshProUGUI>().text = i.item.name;
            c.Find("Count").GetComponent<TextMeshProUGUI>().text = i.count.ToString();
            c.Find("Image").GetComponent<Image>().sprite = i.item.sprite;
            it++;
        }
    }
    IEnumerator ToggleInventory()
    {
        float targetX;
        if (!isInventoryToggled)
        {
            isInventoryToggled = true;
            targetX = inventory.rect.width * -1;
        }
        else
        {
            isInventoryToggled = false;
            targetX = 0;
        }
        while (Mathf.Abs(inventory.anchoredPosition.x - targetX) > 0.1f)
        {
            float x = Mathf.Lerp(inventory.anchoredPosition.x, targetX, lerp);
            inventory.anchoredPosition = Vector3.right * x;
            yield return new WaitForEndOfFrame();
        }
    }
    private void Start()
    {
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StopAllCoroutines();
            CheckInventory(FindAllLoots(player.transform.position, searchRadius));
            StartCoroutine(ToggleInventory());

        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            StopAllCoroutines();
            StartCoroutine(ToggleInventory());
        }
    }

    private List<LootTable> FindAllLoots(Vector3 pos, float radius)
    {
        List<LootTable> loots = new List<LootTable>();
        foreach (Collider2D c in Physics2D.OverlapCircleAll(pos, radius))
        {
            if (c.TryGetComponent(out LootTable l))
            {
                loots.Add(l);
            }
        }
        return loots;
    }
}
