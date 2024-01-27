using UnityEngine;

public class Door : MonoBehaviour
{
    //VARIABLES
    //Changeables
    [SerializeField] private Transform top, bottom, left, right;

    //Access
    public Transform Top { get => top; }
    public Transform Bottom { get => bottom; }
    public Transform Left { get => left; }
    public Transform Right { get => right; }
}
