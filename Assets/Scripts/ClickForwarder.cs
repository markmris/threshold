using UnityEngine;

public class ClickForwarder : MonoBehaviour
{
    public GeneratorController generatorController;

    public void OnMouseDown()
    {
        generatorController.HandleClick(gameObject);
    }
}
