using UnityEngine;
using UnityEngine.UI;

public class ItemToggle_ES : MonoBehaviour
{
    [SerializeField] Image ItemToggleBar2;
    [SerializeField] Image ItemToggleBar1;

    //무기들
    [SerializeField] private Sprite fistSprite;
    [SerializeField] private Sprite pencilcaseSprite;
    [SerializeField] private Sprite laptopweaponSprite;
    [SerializeField] private Sprite mouseSprite;
    [SerializeField] private Sprite beamprojectorSprite;

    [SerializeField] private Weapon weapon;
    [SerializeField] private GameObject laptopModel;
    [SerializeField] private GameObject projectorModel;
    [SerializeField] private PlayerAttack_MK playerAttack;



    //무기
    public string currentState = "fist";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // 공격 중이면 무기 변경 안 함
            if (playerAttack != null && playerAttack.IsAttackingOrBusy())
                return;

            // 스프라이트 전환
            Sprite temp = ItemToggleBar1.sprite;
            ItemToggleBar1.sprite = ItemToggleBar2.sprite;
            ItemToggleBar2.sprite = temp;

            ChangeWeapon();
        }
    }


    public void ChangeWeapon()
    {
        // 상태 업데이트
        UpdateState();
    }

    void UpdateState()
    {
        DeactivateAllWeaponModels();

        if (ItemToggleBar1.sprite == fistSprite)
        {
            weapon.SetAttackInfo(weapon.GetAttackInfo(0));
        }
        else if (ItemToggleBar1.sprite == pencilcaseSprite)
        {
            weapon.SetAttackInfo(weapon.GetAttackInfo(1));
        }
        else if (ItemToggleBar1.sprite == laptopweaponSprite)
        {
            weapon.SetAttackInfo(weapon.GetAttackInfo(2));
            laptopModel.SetActive(true);
        }
        else if (ItemToggleBar1.sprite == mouseSprite)
        {
            weapon.SetAttackInfo(weapon.GetAttackInfo(3));
        }
        else if (ItemToggleBar1.sprite == beamprojectorSprite)
        {
            weapon.SetAttackInfo(weapon.GetAttackInfo(4));
            projectorModel.SetActive(true);
        }
        else
        {
            currentState = "unknown";
        }
    }
    void DeactivateAllWeaponModels()
    {
        laptopModel.SetActive(false);
        projectorModel.SetActive(false);
    }



}
