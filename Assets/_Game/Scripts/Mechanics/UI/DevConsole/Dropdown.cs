using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;





public class Dropdown : MonoBehaviour
{

    public UnityEvent disableDropdown;
    public UnityEvent enableDropdown;

    private bool childrenActive = false;
    


    public void ToggleChildren()
    {
        childrenActive = !childrenActive;
        if (childrenActive)
        {
            enableDropdown.Invoke();
        }
        else
        {
            disableDropdown.Invoke();
        }
    }

}
