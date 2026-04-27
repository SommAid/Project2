using UnityEngine;


public class LaptopInteraction : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Drag your World Space Canvas here")]
    public GameObject popUpWindow;

    void Awake()
    {
        // Hide window on start
        if (popUpWindow != null)
        {
            popUpWindow.SetActive(false);
        }
    }

    // A simple public method that Meta's Event Wrapper can trigger
    public void ToggleWindow()
    {
        Debug.Log("Meta SDK Triggered: Window toggled!");
        if (popUpWindow != null)
        {
            popUpWindow.SetActive(!popUpWindow.activeSelf);
            Debug.Log("Meta SDK Triggered: Window toggled!");
        }
    }
}
