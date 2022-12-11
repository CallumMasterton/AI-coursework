using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensors : MonoBehaviour
{   //Week 3 Agent Sensors - https://moodle.bcu.ac.uk/course/view.php?id=79511
    public static bool rayHit = false;
    public static bool rayPlayerHit = false;

    public LayerMask layerMask;
    public LayerMask playerMask;

    public enum Type
    {
        Line,
        SphereCast,
        BoxCast
    }

    public bool Hit { get; private set; }
    public bool pHit { get; private set; }
    public RaycastHit info = new RaycastHit();

    public Type senssorType = Type.Line;
    public float raycastLength = 1f;

    [Header("BoxExtent Sttings")]
    public Vector2 boxExtents = new Vector2(1f, 1f);

    [Header("Sphere Sttings")]
    public float sphereDiameter = 1f;
    public bool IHearYou = StageTwoBoss.boxSensorSwap;


    Transform cachedTransform;

    // Start is called before the first frame update
    void Start()
    {
        cachedTransform = GetComponent<Transform>();
    }

    void Update()
    {
        if (BossAI.beenHit != true) { Scan(); }

        if (StageTwoBoss.boxSensorSwap == true || StageFourBoss.boxSensorSwap == true)//Checks if the boss needs to swap raycast
        {
            Debug.Log("Heard it load and clear");
            senssorType = Type.BoxCast;
        }
        else senssorType = Type.Line;

        if (BossAI.beenHit == true)//Checks if the boss has been hit
        {
            rayPlayerHit = false;
        }
        else
        {
            if (Hit) { rayHit = true; }
            else { rayHit = false; }

            if (pHit)
            {
                rayPlayerHit = true;
                Debug.Log("Sensor hit the player");
            }
            else { rayPlayerHit = false; }
        }

    }

    public bool Scan()
    {
        pHit = false;
        Hit = false;
        Vector3 dir = cachedTransform.forward;
        switch (senssorType)//Switch to swap bettwen each of the senssor Type
        {
            case Type.Line://Sets Ray cast to a line
                if (Physics.Linecast(cachedTransform.position, cachedTransform.position + dir * raycastLength, out info, layerMask, QueryTriggerInteraction.Ignore))//Cheacks if the ray cast line hits somthing besides the player
                {
                    Hit = true;
                    Debug.Log("Hit");
                    return true;
                }
                else if (Physics.Linecast(cachedTransform.position, cachedTransform.position + dir * raycastLength, out info, playerMask, QueryTriggerInteraction.Ignore))//Cheacks if the ray cast list hits somthing the player
                {
                    pHit = true;
                    Debug.Log("Player Hit");
                    return true;
                }
                break;
            case Type.SphereCast://Sets Ray cast to a Sphere
                if (Physics.SphereCast(new Ray(cachedTransform.position, dir), sphereDiameter, out info, raycastLength, layerMask, QueryTriggerInteraction.Ignore))//Cheacks if the ray cast sphere hits somthing besides the player
                {
                    Hit = true;
                    Debug.Log("Hit");
                    return true;
                }
                break;
            case Type.BoxCast://Sets Ray cast to a Box
                if (Physics.CheckBox(this.transform.position, new Vector3(boxExtents.x, boxExtents.y, raycastLength) / 2f, this.transform.rotation, playerMask, QueryTriggerInteraction.Ignore))//Cheacks if the ray cast box hits somthing besides the player
                {
                    pHit = true;
                    Debug.Log("Hit");
                    return true;
                }
                break;

        }
        return false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        if (cachedTransform == null)
        {
            cachedTransform = GetComponent<Transform>();
        }
        if (BossAI.beenHit != true) { Scan(); }
        if (Hit) Gizmos.color = Color.red;
        Gizmos.matrix *= Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        float length = raycastLength;

        switch (senssorType)//Switch to swap bettwen each of the senssor Type drawring function
        {
            case Type.Line://This will draw a line from the origin to a set size
                if (Hit) length = Vector3.Distance(this.transform.position, info.point);
                Gizmos.DrawLine(Vector3.zero, Vector3.forward * length);//Draws line
                Gizmos.color = Color.green;
                Gizmos.DrawCube(Vector3.forward * length, new Vector3(0.02f, 0.02f, 0.02f));//Draws cube at hit point
                break;
            case Type.SphereCast://This will draw a sphere from the origin to a set size
                Gizmos.DrawWireSphere(Vector3.zero, sphereDiameter);
                if (Hit)
                {
                    Vector3 ballCenter = info.point + info.normal * sphereDiameter;
                    length = Vector3.Distance(cachedTransform.position, ballCenter);
                }
                float halfExtents = sphereDiameter;
                Gizmos.DrawLine(Vector3.up * halfExtents, Vector3.up * halfExtents + Vector3.forward * length);//Draws lines conneting to the collsion sphere
                Gizmos.DrawLine(-Vector3.up * halfExtents, -Vector3.up * halfExtents + Vector3.forward * length);
                Gizmos.DrawLine(Vector3.right * halfExtents, Vector3.right * halfExtents + Vector3.forward * length);
                Gizmos.DrawLine(-Vector3.right * halfExtents, -Vector3.right * halfExtents + Vector3.forward * length);
                Gizmos.DrawWireSphere(Vector3.zero + Vector3.forward * length, sphereDiameter);//Draws the collsion sphere
                break;
            case Type.BoxCast://This will draw a box from the origin to a set size
                Gizmos.DrawWireCube(Vector3.zero, new Vector3(boxExtents.x, boxExtents.y, raycastLength));//Draws cube around player
                break;
        }
    }

}
