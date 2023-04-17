using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    public Objects[] slots;
    public Image[] slotImage;
    public int[] slotAmount;

    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

       if(Physics.Raycast(ray, out hit, 5f ))
        {
         
            if(hit.collider.tag == "Object")
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    for (int i = 0; i < slots.Length; i++)
                    {
                        if (slots[i] == null || slots[i].name == hit.transform.GetComponent<ObjectType>().objectType.name)
                        {
                            slots[i] = hit.transform.GetComponent<ObjectType>().objectType;
                            slotAmount[i]++;
                            slotImage[i].sprite = slots[i].itemSprite;

                            Destroy(hit.transform.gameObject);
                            break;
                        }
                    }
                }
            }

        }
    }
}
