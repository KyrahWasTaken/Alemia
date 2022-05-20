using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LootInventory : MonoBehaviour
{
    public int inventoryWidth;
    public GameObject cell;
    public GameObject canvas;
    public GameObject canvasInstance;
    public ItemStack example;
    List<ItemStack> cells;
    public void ToggleUI(Vector3 pos)
    {
        if (canvasInstance == null)
        {
            canvasInstance = Instantiate(canvas, pos, Quaternion.identity, transform);
            Render();
        }
        else
            Destroy(canvasInstance);
    }
    public void DestroyUI()
    {
        Destroy(canvasInstance);
    }

    public void addItem(ItemStack item)
    {
        cells.Add(item);
        if(canvasInstance != null)
        Render();
    }
    void Render()
    {
        
        Transform inv = canvasInstance.transform.Find("Inventory");
        foreach (Transform child in inv)
            Destroy(child.gameObject);
        int iterator = 0;
        foreach(ItemStack i in cells)
        {
            int x = (iterator) % inventoryWidth;
            int y = -(iterator) / inventoryWidth;
            Debug.Log(iterator);
            Vector2Int pos = new Vector2Int(x,y);
            Transform newCell = Instantiate(cell, (Vector2)pos, Quaternion.identity, inv).transform;
            newCell.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
            newCell.Find("Image").GetComponent<Image>().sprite = i.item.sprite;
            newCell.Find("Count").GetComponent<TextMeshProUGUI>().text = i.count.ToString();
            iterator++;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        cells = new List<ItemStack>();
        cells.Add(example);
        cells.Add(example);
        cells.Add(example);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("HI");
            ToggleUI(transform.position);
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            addItem(example);
        }
    }
}
