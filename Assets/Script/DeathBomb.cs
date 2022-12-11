using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DeathBomb : MonoBehaviour
{
    GameObject player;
    Rigidbody rb;

    public float radius = 2.0F;
    public float power = 40.0F;

    bool theEnd;

    // Start is called before the first frame update
    void Start()
    {
        power = 40.0F;
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        transform.LookAt(player.transform);//Looks at direction of player
        theEnd = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (!theEnd) { transform.Translate(Vector3.forward * 1f); }//Moves forward
        else
        {
            Explode();
        }
    }

    void OnTriggerEnter(Collider col)
    {
        theEnd = true;
        Debug.Log("Boom");
        this.GetComponent<MeshRenderer>().enabled = false;//Turns of the Rendere
        this.GetComponent<Collider>().enabled = false;//Turns of the collider
    }

    public void Explode()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);//Fills sphere colliders
        foreach (Collider hit in colliders)//Cheacks each collider
        {
            if (hit.gameObject.CompareTag("Player"))
            {
                Debug.Log("The Player shall be missed");
            }
            else if (hit.gameObject.CompareTag("enemy"))
            {
                Rigidbody erb = hit.GetComponent<Rigidbody>();
                erb.constraints = RigidbodyConstraints.None;

                if (hit.GetComponent<AiCrowd>()) { hit.GetComponent<AiCrowd>().enabled = false; }//Turns of the Ai Script 
                if (hit.GetComponent<NavMeshAgent>()) { hit.GetComponent<NavMeshAgent>().enabled = false; }//Turns of the NavMeshAgent 
                erb.isKinematic = false;//Turns off Kinematic

                if (erb != null)
                    erb.AddExplosionForce(power, explosionPos, radius, 3.0F);//Adds explosion force

                Destroy(hit.gameObject, 1);
            }
        }

        Destroy(gameObject, 1);
    }
}
