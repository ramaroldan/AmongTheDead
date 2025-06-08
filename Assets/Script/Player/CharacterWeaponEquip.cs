using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeaponEquip : MonoBehaviour
{
    [SerializeField] Animator anim;

    int weaponSelector = 0;
    int weapontTemp = 0;
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            weaponSelector = 1;
        else if(Input.GetKeyDown(KeyCode.Alpha2))
            weaponSelector = 2;
        else if(Input.GetKeyDown(KeyCode.Alpha3)) 
            weaponSelector = 3;
        else if(Input.GetKeyDown(KeyCode.Alpha0))
            weaponSelector = 0;

        if(weaponSelector != weapontTemp)
        {
            weapontTemp = weaponSelector;
            switch (weaponSelector)
            {
                case 0:
                    anim.SetBool("IsPistolEquip", false);
                    anim.SetBool("IsRifleEquip", false);
                    anim.SetBool("IsKnifeEquip", false);
                    break;
                case 1:
                    anim.SetBool("IsPistolEquip", false);
                    anim.SetBool("IsRifleEquip", false);
                    anim.SetBool("IsKnifeEquip", true);
                    break;
                case 2:
                    anim.SetBool("IsPistolEquip", true);
                    anim.SetBool("IsRifleEquip", false);
                    anim.SetBool("IsKnifeEquip", false);
                    break;
                case 3:
                    anim.SetBool("IsPistolEquip", false);
                    anim.SetBool("IsRifleEquip", true);
                    anim.SetBool("IsKnifeEquip", false);
                    break;
            }

        }

            
    }
}
