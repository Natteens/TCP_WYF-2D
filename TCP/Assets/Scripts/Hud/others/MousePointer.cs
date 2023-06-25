using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointer : MonoBehaviour
{
    public Transform pointer;

    void Awake()
    {
        Cursor.visible = false;
    }
    void Update()
    {
        InGameMouse();
    }

    void InGameMouse()
    {
        Vector3 mouse = Input.mousePosition;
        mouse.z = 0.5f;

        pointer.position = Camera.main.ScreenToWorldPoint(mouse);

        if (Input.GetMouseButtonDown(0) && Cursor.visible == true)
        {
            Cursor.visible = false;
        }
    }
}
