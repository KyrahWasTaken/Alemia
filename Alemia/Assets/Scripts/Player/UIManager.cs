using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public RectTransform healthbar;
    private float width;
    private void Start()
    {
        width = healthbar.rect.width;
        FindObjectOfType<PlayerController>().GetComponent<Entity>().OnHPChanged += HealthBarChange;
    }
    private void HealthBarChange(int max, int current)
    {
        healthbar.sizeDelta = new Vector2(width / max * current, healthbar.sizeDelta.y);
    }
}
