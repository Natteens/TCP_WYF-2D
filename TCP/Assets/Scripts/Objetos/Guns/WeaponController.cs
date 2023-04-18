using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    public Transform weaponsParent;
    public KeyCode[] numberKeys;
    public float scrollSpeed = 0.1f;

    private int currentWeaponIndex = 0;
    private List<Transform> weapons = new List<Transform>();

    private void Start()
    {
        foreach (Transform weapon in weaponsParent)
        {
            weapon.gameObject.SetActive(false);
            weapons.Add(weapon);
        }
    }

    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Count;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            currentWeaponIndex--;
            if (currentWeaponIndex < 0)
            {
                currentWeaponIndex = weapons.Count - 1;
            }
        }

        foreach (KeyCode key in numberKeys)
        {
            if (Input.GetKeyDown(key))
            {
                int numPressed = (int)key - (int)KeyCode.Alpha0 - 1;
                if (numPressed >= 0 && numPressed < weapons.Count)
                {
                    currentWeaponIndex = numPressed;
                }
            }
        }

        for (int i = 0; i < weapons.Count; i++)
        {
            if (i == currentWeaponIndex)
            {
                weapons[i].gameObject.SetActive(true);
            }
            else
            {
                weapons[i].gameObject.SetActive(false);
            }
        }
    }
}
