using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleGateR : MonoBehaviour
{
    Vector3 final;

    private void Awake()
    {
        final = new Vector3(this.transform.position.x + 3f, this.transform.position.y, this.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        HandleGate();
    }

    void HandleGate()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            while (Vector3.Distance(this.transform.position, final) > 0.1f)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, final, Time.deltaTime);
            }
        }
    }
}
