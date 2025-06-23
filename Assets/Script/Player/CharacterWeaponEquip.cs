using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Tilemaps;

public class CharacterWeaponEquip : MonoBehaviour
{
    AudioSource _audioSource;
    [SerializeField] Animator anim;
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] private Collider knifeCollider;

    [SerializeField] private List<GameObject> weaponList;
    [SerializeField] private Transform currentWeaponPos;

    [Header("Bag")]
    [SerializeField] private Transform weaponInBag;
    
    [Header("Weapon Positions")]
    [SerializeField] private Transform knifePos;
    [SerializeField] private Transform pistolPos;
    [SerializeField] private Transform riflePos;
    [SerializeField] private Transform grenadePos;

    [Header("Grenade settings")]
    [SerializeField] private Transform throwPosition; // reference to the throw position transform
    [SerializeField] private Vector3 throwDirection = new Vector3(0, 1, 0); // direction of the throw
    [SerializeField] private GameObject grenadePrefab;

    [Header("Grenade force")]
    [SerializeField] private float throwForce = 1.0f; // force applied to throw the grenade
    [SerializeField] private float maxForce = 2.0f; // maximum force applied to throw the grenade

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
    
    [Header("Audio section")]
    [SerializeField] AudioClip _knifeDraw;
    [SerializeField] AudioClip _pistolCock;
    [SerializeField] AudioClip _rifleLeverAction;
    [SerializeField] AudioClip _medKitBandage;

    [Header("UI section")]
    [SerializeField] HoverOver _hoverOverToolbar;

    private bool isCharging = false; // flag to check if player is charging the throw
    private float chargeTime = 0f; // time player has been gharging the throw

    int weaponSelector = 0;
    int weapontTemp = 0;
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        playerHealth= GetComponent<PlayerHealth>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Item item = InventoryManager.instance.GetSelectedItem(false);
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            weaponSelector = 0;
            InventoryManager.instance.ChangeSelectedSlot(-1);
        }

        if (item == null)
            weaponSelector = 0;
        if(item != null)
        {
            if (item.type == Item.ItemType.Knife)
                weaponSelector = 1;
            else if (item.type == Item.ItemType.Pistol)
                weaponSelector = 2;
            else if (item.type == Item.ItemType.Rifle)
                weaponSelector = 3;
            else if (item.type == Item.ItemType.MedKit)
                weaponSelector = 4;
            else if (item.type == Item.ItemType.Grenade)
                weaponSelector = 5;
        }
            

        if(weaponSelector != weapontTemp)
        {
            weapontTemp = weaponSelector;
            switch (weaponSelector)
            {
                case 0:
                case 4:
                    anim.SetBool("IsPistolEquip", false);
                    anim.SetBool("IsRifleEquip", false);
                    anim.SetBool("IsKnifeEquip", false);
                    anim.SetBool("IsGrenadeEquip", false);
                    UnEquip();


                    break;
                case 1:
                    anim.SetBool("IsPistolEquip", false);
                    anim.SetBool("IsRifleEquip", false);
                    anim.SetBool("IsKnifeEquip", true);
                    anim.SetBool("IsGrenadeEquip", false);
                    UnEquip();

                    weaponList[0].transform.parent = knifePos.transform;
                    weaponList[0].transform.position = knifePos.position;
                    weaponList[0].transform.rotation = knifePos.rotation;
                    weaponList[0].SetActive(true);
                    _audioSource.PlayOneShot(_knifeDraw, 0.5f);
                    break;
                case 2:
                    anim.SetBool("IsPistolEquip", true);
                    anim.SetBool("IsRifleEquip", false);
                    anim.SetBool("IsKnifeEquip", false);
                    anim.SetBool("IsGrenadeEquip", false);
                    UnEquip();

                    weaponList[1].transform.parent = pistolPos.transform;
                    weaponList[1].transform.position = pistolPos.position;
                    weaponList[1].transform.rotation = pistolPos.rotation;
                    weaponList[1].SetActive(true);
                    _audioSource.PlayOneShot(_pistolCock, 0.5f);
                    break;
                case 3:
                    anim.SetBool("IsPistolEquip", false);
                    anim.SetBool("IsRifleEquip", true);
                    anim.SetBool("IsKnifeEquip", false);
                    anim.SetBool("IsGrenadeEquip", false);
                    UnEquip();

                    weaponList[2].transform.parent = riflePos.transform;
                    weaponList[2].transform.position = riflePos.position;
                    weaponList[2].transform.rotation = riflePos.rotation;
                    weaponList[2].SetActive(true);
                    _audioSource.PlayOneShot(_rifleLeverAction, 0.5f);
                    break;
                case 5:
                    anim.SetBool("IsPistolEquip", false);
                    anim.SetBool("IsRifleEquip", false);
                    anim.SetBool("IsKnifeEquip", false);
                    anim.SetBool("IsGrenadeEquip", true);
                    UnEquip();

                    weaponList[3].transform.parent = grenadePos.transform;
                    weaponList[3].transform.position = grenadePos.position;
                    weaponList[3].transform.rotation = grenadePos.rotation;
                    weaponList[3].SetActive(true);
                    // play audio clip for grenade?
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
                leftHandIK.weight = 0f;
                rightHandIK.weight = 0f;
                if (Input.GetMouseButtonDown(0) && (!_hoverOverToolbar.IsOverElement()))
                {
                    anim.SetTrigger("Stab");
                }
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
            case 4:
                leftHandIK.weight = 0f;
                rightHandIK.weight = 0f;
                if (Input.GetMouseButtonDown(0) && item.actionType == Item.ActionType.Heal && (!_hoverOverToolbar.IsOverElement()))
                {
                    _audioSource.PlayOneShot(_medKitBandage, 0.5f);
                    item = InventoryManager.instance.GetSelectedItem(true);
                    playerHealth.ReceiveHealth(item.countHealth);

                }
                break;
            case 5:
                leftHandIK.weight = 0f;
                rightHandIK.weight = 0f;
                if (Input.GetMouseButtonDown(0) && item.actionType == Item.ActionType.Throw && (!_hoverOverToolbar.IsOverElement()))
                {
                    StartThrowing();
                }
                if (isCharging)
                {
                    ChargeThrow();
                }
                if (Input.GetMouseButtonUp(0) && item.actionType == Item.ActionType.Throw && (!_hoverOverToolbar.IsOverElement()))
                {
                    anim.SetTrigger("ThrowGrenade");
                    item = InventoryManager.instance.GetSelectedItem(true);
                    ReleaseThrow();
                }
                break;
        }
        


    }

    public void UnEquip()
    {
        foreach(GameObject wpn in weaponList)
        {
            //wpn.transform.parent = null;
            
            wpn.SetActive(false);
        }
    }

    public void EnableKnifeCollider()
    {
        knifeCollider.enabled = true;
    }

    public void DisableKnifeCollider()
    {
        knifeCollider.enabled = false;
    }

    private void OnDestroy()
    {
        weaponSelector = 0;
        UnEquip();
    }

    void StartThrowing()
    {
        // pull pin sound

        isCharging = true;
        chargeTime = 0f;

        //Trajectory line
    }

    void ChargeThrow()
    {
        chargeTime += Time.deltaTime;

        //trajectory line velocity
    }

    void ReleaseThrow()
    {
        //play animation

        isCharging = false;
        CalculateForce();

        //hide line
    }

    void CalculateForce()
    {
        ThrowGrenade(Mathf.Min(chargeTime * throwForce, maxForce));
    }

    void ThrowGrenade(float force)
    {
        Vector3 spawnPosition = throwPosition.position;
        GameObject grenade = Instantiate(grenadePrefab, spawnPosition, gameObject.transform.rotation);

        Rigidbody rb = grenade.GetComponent<Rigidbody>();

        Vector3 finalThrowDirection = (gameObject.transform.forward + throwDirection).normalized;
        rb.AddForce(finalThrowDirection * force, ForceMode.VelocityChange);

        //Throwing sound
    }

    // show Trajectory
}
