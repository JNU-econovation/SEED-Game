using System;
using UnityEngine;
using UnityEngine.UI;

public class WeaponTrigger : MonoBehaviour
{
    [SerializeField] private Sprite weaponImage;
    [SerializeField] private GameObject itemToggleBar;
    
    private Image currentWeaponImage;
    private ItemToggle_ES itemToggle;

    private void Awake()
    {
        currentWeaponImage = itemToggleBar.transform.GetChild(1).GetChild(0).GetComponent<Image>();
        itemToggle = itemToggleBar.GetComponent<ItemToggle_ES>();
    }

    public void ChangeWeapon()
    {
        currentWeaponImage.sprite = weaponImage;
        itemToggle.ChangeWeapon();
    }
}
