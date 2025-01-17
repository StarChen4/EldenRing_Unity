using UnityEngine;

public class WeaponModelInstantiationSlot : MonoBehaviour
{
    public WeaponModelSlot weaponSlot;   // what slot is this (left or right or back or hips)
    public GameObject currentWeaponModel;

    public void UnloadWeapon()
    {
        if (currentWeaponModel != null)
        {
            Destroy(currentWeaponModel);
        }
    }

    public void LoadWeapon(GameObject weaponModel)
    {
        currentWeaponModel = weaponModel;
        weaponModel.transform.parent = transform;
        
        weaponModel.transform.localPosition = Vector3.zero;
        weaponModel.transform.localRotation = Quaternion.identity;
        weaponModel.transform.localScale = Vector3.one;
    }
}
