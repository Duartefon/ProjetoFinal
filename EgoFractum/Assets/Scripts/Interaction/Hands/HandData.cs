using UnityEngine;

public class HandData : MonoBehaviour
{
    public enum Type // nao vai ser preciso achop eu
    {
        Left,
        Right
    }

    public Type handType;
    public Transform root;
    public Animator animator;
    public Transform[] fingers;

}
