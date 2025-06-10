using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterWeaponEquip : MonoBehaviour
{
    [SerializeField] Animator anim;

    [SerializeField] private List<GameObject> weaponList;
    [SerializeField] private Transform currentWeaponPos;

    [Header("Weapon Positions")]
    [SerializeField] private Transform knifePos;
    [SerializeField] private Transform pistolPos;
    [SerializeField] private Transform riflePos;

    [Header("Right Hand Target")]
    [SerializeField] private TwoBoneIKConstraint rightHandIK;
    [SerializeField] private Transform rightHandTarget;

    [Header("Left Hand Target")]
    [SerializeField] private TwoBoneIKConstraint leftHandIK;
    [SerializeField] private Transform leftHandTarget;

    [SerializeField] private Transform IKRightHandPos;
    [SerializeField] private Transform IKLeftHandPos;

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

                    weaponList[0].transform.parent = riflePos.transform;
                    weaponList[0].transform.position = riflePos.position;
                    weaponList[0].transform.rotation = riflePos.rotation;
                    weaponList[0].SetActive(true);
                    
                    break;
            }

        }

        if(weaponSelector != 0)
        {
            leftHandTarget.position = IKLeftHandPos.position;
            leftHandTarget.rotation = IKLeftHandPos.rotation;
            rightHandTarget.position = IKRightHandPos.position;
            rightHandTarget.rotation = IKRightHandPos.rotation;
            leftHandIK.weight = 1f;
            rightHandIK.weight = 1f;
        }


    }
}
