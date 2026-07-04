using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
  public Animator anim;
  public NavMeshAgent agent;
  public Transform player;
  public float MinDist, MaxDist, MoveSpeed = 15f;

  public void Update()
  {
 
    
    transform.LookAt(player);

    if (Vector3.Distance(transform.position, player.position) >= MinDist)
    {

      transform.position += transform.forward * MoveSpeed * Time.deltaTime;



      if (Vector3.Distance(transform.position, player.position) <= MaxDist)
      {
        //Here Call any function U want Like Shoot at here or something
      }

    }

  }
}
