using System.Collections.Generic;
using UnityEngine;


public static class ConeCastExtension
{

    public static Vector3[] GenerateConePoints(float radius, float increment)
    {
        List<Vector3> points = new List<Vector3>();
        points.Add(Vector3.zero);

        float step = increment;

        while(step <= radius)
        {
            points.Add(new Vector3(0, step, 0));
            points.Add(new Vector3(0, -step, 0));
            points.Add(new Vector3(step, 0, 0));
            points.Add(new Vector3(-step, 0, 0));

            points.Add(new Vector3(-step, step, 0)); 
            points.Add(new Vector3(step, step, 0));
            points.Add(new Vector3(-step, -step, 0));
            points.Add(new Vector3(step, -step, 0));

            step += increment;
        }

        return points.ToArray();
    }
    public static bool ConeCast(Vector3 origin, float maxRadius, Vector3 direction, float maxDistance, float angleInDegrees, ref RaycastHit hit, int whatIsGrappleable)
    {

        //Vector3 origin = cam.position;
        //float maxRadius = 1f;
        //Vector3 direction = cam.forward;
        bool hitSomething = false;

        float closestPoint = 0;

        Debug.DrawRay(origin, direction * maxDistance, Color.red);

        if(Physics.Raycast(origin, direction, out hit, maxDistance, whatIsGrappleable, QueryTriggerInteraction.Ignore))
        {
            return true;
        }
        
        
        RaycastHit[] sphereCastHits = Physics.SphereCastAll(origin, maxRadius, direction, maxDistance, whatIsGrappleable);
        List<RaycastHit> coneCastHitList = new List<RaycastHit>();

        for(int segment = 0; segment < 6; segment++)
        {
            sphereCastHits = Physics.SphereCastAll(origin, segment * maxRadius/6, direction, maxDistance, whatIsGrappleable);
            coneCastHitList = new List<RaycastHit>();
            if (sphereCastHits.Length > 0)
            {
                for (int i = 0; i < sphereCastHits.Length; i++)
                {
                    Vector3 hitPoint = sphereCastHits[i].point;
                    float hitDistance = sphereCastHits[i].distance;
                    Vector3 directionToHit = hitPoint - origin;
                    float angleToHit = Mathf.Abs(Vector3.Angle(direction, directionToHit));

                    if (angleToHit <= angleInDegrees)
                    {
                        closestPoint = hitDistance;
                        hit = sphereCastHits[i];
                        angleInDegrees = angleToHit;
                        hitSomething = true;
                    }
                }
            }
            if (hitSomething)
            {
                return true;
            }
        }



        return hitSomething;
    }

    public static bool ConeCastPoints(Vector3 origin, float maxRadius, Vector3 direction, float maxDistance, float angleInDegrees, Vector3[] points, ref RaycastHit hit, int whatIsGrappleable)
    {

        //Vector3 origin = cam.position;
        //float maxRadius = 1f;
        //Vector3 direction = cam.forward;
        bool hitSomething = false;

        float closestPoint = 0;

        Debug.DrawRay(origin, direction * maxDistance, Color.red);

        if (Physics.Raycast(origin, direction, out hit, maxDistance, whatIsGrappleable, QueryTriggerInteraction.Ignore))
        {
            return true;
        }

        Vector3 scaledDirection = (maxDistance * direction);
        RaycastHit[] sphereCastHits; 
        List<RaycastHit> coneCastHitList;
        foreach(Vector3 point in points)
        {
            Vector3 newDirection = (scaledDirection + point);
            //Debug.Log("Direction = "+direction+"; scaledDirection: " + scaledDirection + " + Point: " + point + "= " + (newDirection));
            sphereCastHits = Physics.SphereCastAll(origin, maxRadius, newDirection, maxDistance, whatIsGrappleable);
            Debug.DrawRay(origin, (newDirection) * maxDistance, Color.yellow);
            coneCastHitList = new List<RaycastHit>();

            if (Physics.SphereCast(origin, maxRadius, newDirection, out hit, maxDistance, whatIsGrappleable, QueryTriggerInteraction.Ignore))
            {
                return true;
                Vector3 hitPoint = hit.point;
                float hitDistance = hit.distance;
                Vector3 directionToHit = hitPoint - origin;
                float angleToHit = Mathf.Abs(Vector3.Angle(direction, directionToHit));

                if (hitDistance <= closestPoint)
                {
                    closestPoint = hitDistance;
                    angleInDegrees = angleToHit;
                    hitSomething = true;
                }
                
            }

            
        }

        return hitSomething;
    }
}
