using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Function for giving enemies a FOV
/// </summary>
public class EnemySight : MonoBehaviour {

    /// <summary>
    /// How far the enemy can view
    /// </summary>
    public float viewRadius;

    /// <summary>
    /// The FOV of enemy
    /// </summary>
    public float viewAngle;

    /// <summary>
    /// Any walls/obstacles
    /// </summary>
    public LayerMask obstacleMask;

    /// <summary>
    /// Bool determining if the player is seen by enemy
    /// </summary>
    public bool isSeen;

    Mesh viewMesh;
    public MeshFilter viewMeshFilter;


    private void Start()
    {
        viewMesh = new Mesh();
        viewMeshFilter.mesh = viewMesh;
    }

    private void LateUpdate()
    {
        DrawFOV();
        FindPlayer();
    }

    /// <summary>
    /// Function determines if the player is seen by enemy
    /// </summary>
    void FindPlayer()
    {
        int rayCount = Mathf.RoundToInt(viewAngle);
        float stepAngleSize = viewAngle / rayCount;

        isSeen = false;

        for (int j = 0; j < rayCount; j++)
        {
            //Going to raycast every angleStep to see if player is seen
            float angleSteps = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * j;
            if (HitPlayer(angleSteps))
            {
                isSeen = true;
            }
        }

    }
    
    /// <summary>
    /// Draws the FOV of the enemy
    /// </summary>
    void DrawFOV()
    {
        int rayCount = Mathf.RoundToInt(viewAngle);
        float stepAngleSize = viewAngle / rayCount;
        List<Vector3> viewPoints = new List<Vector3>();
        for (int i = 0; i <= rayCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            viewPoints.Add(ViewCast(angle));
        }
        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];
        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
            if(i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    /// <summary>
    /// Helper function of DrawFOV(), determines how far each ray goes when drawing FOV
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    Vector3 ViewCast(float angle)
    {
        Vector3 direction = DirectionFromAngle(angle);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, viewRadius, obstacleMask)) 
        {
            return hit.point;
        }       
        else
        {
            return transform.position + direction * viewRadius;
        }
    }

    /// <summary>
    /// Helper function of FindPlayer(), determines if player is within enemy FOV
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    bool HitPlayer(float angle)
    {
        Vector3 direction = DirectionFromAngle(angle);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, viewRadius))
        {
            if (hit.collider.tag == "Player")
                return true;
        }
        return false;
    }

    /// <summary>
    /// Helper function, gets average direction from angle
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    Vector3 DirectionFromAngle(float angle)
    {
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
}