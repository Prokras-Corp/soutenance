using UnityEngine;

public class ButtonBoss : MonoBehaviour
{
    [SerializeField] GameObject button;
    public bool isActive = false;

    private void OnTriggerEnter(Collider other)
    {
        button.transform.position = new Vector3(button.transform.position.x, button.transform.position.y - 0.60f, button.transform.position.z);
        isActive = true;
    }
}
