
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class MainWorldScript : UdonSharpBehaviour
{
    public GameObject[] m_DeactivateObjects;

    void Start()
    {
        foreach(GameObject obj in m_DeactivateObjects)
        {
            obj.SetActive(false);
        }
    }
}
