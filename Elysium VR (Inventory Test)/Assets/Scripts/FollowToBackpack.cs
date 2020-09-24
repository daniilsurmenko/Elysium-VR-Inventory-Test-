using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowToBackpack : MonoBehaviour
{
    [SerializeField] private Transform backpackTransfor = null;
    private Vector3 offSet = new Vector3(0, 5.5f, 0);
    void Update()
    {
        gameObject.transform.position = backpackTransfor.position + offSet;
    }
}
