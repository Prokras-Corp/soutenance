using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLadder : MonoBehaviour
{
    [SerializeField] GameObject ladder;
    [SerializeField] GameObject button;
    BoxCollider boxc;

    private void OnTriggerEnter(Collider other)
    {
        boxc = button.GetComponent<BoxCollider>();
        boxc.transform.position = new Vector3(boxc.transform.position.x, boxc.transform.position.y - 60f, boxc.transform.position.z);
        button.transform.position = new Vector3(button.transform.position.x, button.transform.position.y - 0.60f, button.transform.position.z);
        ladder.SetActive(true);
    }
}
