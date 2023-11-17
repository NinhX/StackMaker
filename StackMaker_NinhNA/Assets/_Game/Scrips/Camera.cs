using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camere : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 vt;


    void LateUpdate()
    {
        transform.position = player.position + vt;
    }
}
