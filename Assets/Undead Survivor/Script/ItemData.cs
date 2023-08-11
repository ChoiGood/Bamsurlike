using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptble Object/ItemData")]   // CreatAssetMenu : 커스텀 메뉴를 생성하는 속성
public class ItemData : ScriptableObject   
{
    public enum ItemType { Melee, Range , Glove, Shoe, Heal} // 근접, 원거리, 글러브, 슈즈 , 힐

    [Header("# Main Info")]
    public ItemType itemType;
    public int itemId;
    public string itemName;
    public string itemDesc;
    public Sprite itemIcon;

    [Header("# Level Data")]
    public float baseDamage;
    public int baseCount;
    public float[] damages;
    public int[] counts;


    [Header("# Weapon")]
    public GameObject projectile;
}
