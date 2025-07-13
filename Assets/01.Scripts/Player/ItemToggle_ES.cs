using UnityEngine;
using UnityEngine.UI;

public class ItemToggle_ES: MonoBehaviour
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

    //무기
    public string currentState = "fist";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
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
        if (ItemToggleBar1.sprite == fistSprite)
        {
            AttackInfos chosenAttack = weapon.GetAttackInfo(0);
            weapon.SetAttackInfo(chosenAttack);
        }
        else if (ItemToggleBar1.sprite == pencilcaseSprite)
        {
            AttackInfos chosenAttack = weapon.GetAttackInfo(1);
            weapon.SetAttackInfo(chosenAttack);
        }
        else if (ItemToggleBar1.sprite == laptopweaponSprite)
        {
            AttackInfos chosenAttack = weapon.GetAttackInfo(2);
            weapon.SetAttackInfo(chosenAttack);
        }
        else if (ItemToggleBar1.sprite == mouseSprite)
        {
            AttackInfos chosenAttack = weapon.GetAttackInfo(3);
            weapon.SetAttackInfo(chosenAttack);
        }
        else if (ItemToggleBar1.sprite == beamprojectorSprite)
        {
            AttackInfos chosenAttack = weapon.GetAttackInfo(4);
            weapon.SetAttackInfo(chosenAttack);
        }
        else
        {
            currentState = "unknown";
        }
    }

}
