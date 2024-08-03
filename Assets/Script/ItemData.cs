using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/New item")]
public class ItemData : ScriptableObject
{
    [Header("Data")]
    public string itemName;
    [TextArea(2, 10)]
    public string itemDescription;
    public Sprite visual;
    public GameObject itemPrefab;
    public bool stackable;
    public int maxStack;


    public Type type;

    [Header("Effect")]
    public int healthAdd;
    public int damageToEnemyAdd;
    public float speedAdd;
    public bool canDoubleChot;
    public float timeOfCounterAdd;
    [Space]
    public float duration;



    public enum Type
    {
        Resource,
        Consumable,
    }







}

