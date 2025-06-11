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

    [Header("Knife IK Positions")]
    [SerializeField] private Transform knifeIKRightHandPos;
    [SerializeField] private Transform knifeIKLeftHandPos;

    [Header("Pistol IK Positions")]
    [SerializeField] private Transform pistolIKRightHandPos;
    [SerializeField] private Transform pistolIKLeftHandPos;

    [Header("Rifle IK Positions")]
    [SerializeField] private Transform rifleIKRightHandPos;
    [SerializeField] private Transform rifleIKLeftHandPos;

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
                    UnEquip();


                    break;
                case 1:
                    anim.SetBool("IsPistolEquip", false);
                    anim.SetBool("IsRifleEquip", false);
                    anim.SetBool("IsKnifeEquip", true);
                    UnEquip();


                    break;
                case 2:
                    anim.SetBool("IsPistolEquip", true);
                    anim.SetBool("IsRifleEquip", false);
                    anim.SetBool("IsKnifeEquip", false);
                    UnEquip();

                    weaponList[1].transform.parent = pistolPos.transform;
                    weaponList[1].transform.position = pistolPos.position;
                    weaponList[1].transform.rotation = pistolPos.rotation;
                    weaponList[1].SetActive(true);

                    break;
                case 3:
                    anim.SetBool("IsPistolEquip", false);
                    anim.SetBool("IsRifleEquip", true);
                    anim.SetBool("IsKnifeEquip", false);
                    UnEquip();

                    weaponList[2].transform.parent = riflePos.transform;
                    weaponList[2].transform.position = riflePos.position;
                    weaponList[2].transform.rotation = riflePos.rotation;
                    weaponList[2].SetActive(true);                    
                    break;
            }

        }

        switch (weaponSelector)
        {
            case 0:
                leftHandIK.weight = 0f;
                rightHandIK.weight = 0f;
                break;
            case 1:

                break;
            case 2:
                leftHandTarget.position = pistolIKLeftHandPos.position;
                leftHandTarget.rotation = pistolIKLeftHandPos.rotation;
                rightHandTarget.position = pistolIKRightHandPos.position;
                rightHandTarget.rotation = pistolIKRightHandPos.rotation;
                leftHandIK.weight = 0.9f;
                rightHandIK.weight = 0.9f;
                break;
            case 3:
                leftHandTarget.position = rifleIKLeftHandPos.position;
                leftHandTarget.rotation = rifleIKLeftHandPos.rotation;
                rightHandTarget.position = rifleIKRightHandPos.position;
                rightHandTarget.rotation = rifleIKRightHandPos.rotation;
                leftHandIK.weight = 0.9f;
                rightHandIK.weight = 0.9f;
                break;
        }
        


    }

    void UnEquip()
    {
        foreach(GameObject wpn in weaponList)
        {
            wpn.transform.parent= null;
            wpn.SetActive(false);
        }
    }
}
