using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "Paint", menuName = "Paint/New paint")]
public class PaintingData : ScriptableObject
{
    public string paintName;
    [TextArea(2, 10)]
    public string paintDescrption;
    public Sprite paintVisual;
    public float duration;
    public ItemInInventory[] colorToPaint;
}
