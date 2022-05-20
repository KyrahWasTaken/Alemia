using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InventoryUI : MonoBehaviour
{
    public RectTransform inventory;
    public RectTransform cell;
    public Entity player;
    [Range(0,1)]
    public float lerp;
    public bool isInventoryToggled;
    public void CheckInventory(Entity player)
    {
        foreach(Transform child in inventory)
        {
            Destroy(child.gameObject);
        }

        int it = 0;
        foreach(ItemStack i in player.inventory)
        {
            RectTransform c = Instantiate(cell, transform);
            c.anchoredPosition = new Vector3(0, cell.rect.height * it * -1);
            c.Find("Name").GetComponent<TextMeshProUGUI>().text = i.item.name;
            c.Find("Count").GetComponent<TextMeshProUGUI>().text = i.count.ToString();
            c.Find("Image").GetComponent<Image>().sprite = i.item.sprite;
            it++;
        }
    }
    IEnumerator toggleInventory()
    {

        float targetX;
        if(isInventoryToggled)
        {
            isInventoryToggled = false;
            targetX = inventory.rect.width * -1;
        }
        else
        {
            isInventoryToggled = true;
            targetX = 0;
        }
        while(Mathf.Abs(inventory.anchoredPosition.x - targetX) > 0.1f)
        {
            float x = Mathf.Lerp(inventory.anchoredPosition.x, targetX, lerp);
            inventory.anchoredPosition = Vector3.right * x;
            yield return new WaitForEndOfFrame();
        }
    }
    private void Start()
    {
        player.OnInventoryChanged += CheckInventory;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            CheckInventory(player);
        }
        if(Input.GetKeyDown(KeyCode.I))
        {
            StopAllCoroutines();
            StartCoroutine(toggleInventory());
        }    
    }
}
