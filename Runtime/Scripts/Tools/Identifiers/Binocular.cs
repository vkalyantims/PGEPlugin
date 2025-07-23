using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Binocular : MonoBehaviour
{
    [SerializeField]
    private GameObject binocularObject;
    public void Enable()
    {
        binocularObject.SetActive(true);
    }
    public void Disable()
    {
        binocularObject.SetActive(false);
    }
}
