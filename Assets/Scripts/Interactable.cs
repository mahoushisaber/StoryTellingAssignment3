using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Interactable")]
public class Interactable : ScriptableObject
{
    public List<Item> items;
}

[System.Serializable]
public struct Item
{
    public Sprite image;
    public string story;
    public string itemName;
}
