using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowBoneDirection : MonoBehaviour
{
    bool HasGetChildren;
    List<Transform> allChildrenTra = new List<Transform> ();
    void OnDrawGizmosSelected ()
    {
        if (!HasGetChildren)
        {
            List<GameObject> allChildren = GetAllChildren.GetAll (gameObject);
            foreach (GameObject g in allChildren)
            {
                allChildrenTra.Add (g.transform);
            }
            HasGetChildren = true;
        }
        foreach (Transform t in allChildrenTra)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine (t.position, t.position + t.right);
            Gizmos.color = Color.green;
            Gizmos.DrawLine (t.position, t.position + t.up);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine (t.position, t.position + t.forward);
        }
    }
}