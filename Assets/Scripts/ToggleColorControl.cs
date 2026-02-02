using UnityEngine;
using UnityEngine.UI;

public class ToggleColorControl : MonoBehaviour
{
    public Image backgroundImage;
    public Toggle toggle;

    void Awake()
    {
        SwitchColor(toggle.isOn);

        toggle.onValueChanged.AddListener(SwitchColor);
    }

    public void SwitchColor(bool isOn)
    {
        backgroundImage.color = isOn ? Color.green : Color.red;
    }
}
