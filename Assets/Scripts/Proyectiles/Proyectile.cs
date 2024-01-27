using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectile : MonoBehaviour
{
    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void AddForce(Vector3 direction,float force)
    {
        rb.AddForce(direction * force, ForceMode.Impulse);
    }
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
