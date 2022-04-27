using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName ="Item")]
public class Item : ScriptableObject
{
    public string displayName; //Displayed Name
    public string nameID; //ID name 
    public int StackCount;
    public Sprite sprite;
}