using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToMaze : MonoBehaviour
{
    CharacterController player;

    [SerializeField] GameObject boss;
    [SerializeField] GameObject maze;
    [SerializeField] GameObject trigger;
    [SerializeField] GameObject trigger2;

    [SerializeField] GameObject day;
    [SerializeField] GameObject night;

    [SerializeField] Material skybox1;

    public void OnTriggerEnter(Collider other)
    {
        boss.SetActive(false);

        day.SetActive(false);
        night.SetActive(true);

        RenderSettings.skybox = skybox1;

        trigger.SetActive(false);
        trigger2.SetActive(true);
        maze.SetActive(true);

        player = other.GetComponent<CharacterController>();
        player.enabled = false;
        player.transform.position = new Vector3(138, -20, -18);
        player.enabled = true;
    }
}
