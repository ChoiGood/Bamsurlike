using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Item : MonoBehaviour
{
    public ItemData data;
    public int level;
    public Weapon weapon;
    public Gear gear;


    Image icon;
    Text textLevel;
    Text textName;
    Text textDesc;

    private void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1]; // GetComponentsInchildren에서 두번째 값으로 가져오기 ( 첫번째는 자기자신)
        icon.sprite = data.itemIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        // GetComponents 의 순서는 계층구조의 순서를 따라간다
        textLevel = texts[0];
        textName = texts[1];
        textDesc = texts[2];
        textName.text = data.itemName;
    }

    void OnEnable()
    {
        textLevel.text = "Lv." + (level + 1);

        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100);
                break;
            default:
                textDesc.text = string.Format(data.itemDesc);
                break;
        }

       
    }

    public void OnClick()
    {
        switch(data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                if (level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();  // AddComponent 함수 반환 값을 미리 선언한 변수에 저장.
                    weapon.Init(data);
                }
                else
                {
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;

                    nextDamage += data.baseDamage * data.damages[level];
                    nextCount += data.counts[level];

                    weapon.LevelUp(nextDamage, nextCount);
                }

                level++;
                break;

            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                if(level == 0)
                {
                    GameObject newGear = new GameObject();
                    gear = newGear.AddComponent<Gear>();  // AddComponent 함수 반환 값을 미리 선언한 변수에 저장.
                    gear.Init(data);
                }
                else
                {
                    float nextRate = data.damages[level];
                    gear.LevelUp(nextRate);
                }

                level++;
                break;

            case ItemData.ItemType.Heal:
                GameManager.instance.health = GameManager.instance.maxHealth;
                break;
        }

        

        if(level == data.damages.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }
}    
