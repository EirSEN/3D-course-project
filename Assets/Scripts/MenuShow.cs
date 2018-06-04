using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuShow : MonoBehaviour
{
    bool isPlayerinRange = false;
    public Image _panel;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        isPlayerinRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isPlayerinRange = false;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Action"))
        {
            _panel.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
