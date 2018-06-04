using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PendulumSettings : MonoBehaviour
{
    public Slider LengthSlider;
    public Slider MassSlider;

    public Rigidbody SphereRB;
    public Transform NiddleTransform;



    private void OnEnable()
    {
        LengthSlider.value = NiddleTransform.localScale.y;
        MassSlider.value = SphereRB.mass;
    }


    public void ApplyChanges()
    {
        NiddleTransform.localScale = new Vector3(0.1f, LengthSlider.value, 0.1f);
        SphereRB.mass = MassSlider.value;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameObject.SetActive(false);
    }

}
