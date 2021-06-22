using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skymap : MonoBehaviour
{
    [SerializeField] Material sky;

    // Start is called before the first frame update
    void Start()
    {
        RenderSettings.skybox = sky;
    }
}
