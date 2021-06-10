using UnityEngine;
using UnityEngine.AI;

public class Man : MonoBehaviour
{
    public NavMeshAgent agent;
    Camera cam;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
}
