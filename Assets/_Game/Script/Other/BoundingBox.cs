using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingBox : MonoBehaviour
{
    [SerializeField] private float buffer;
    [SerializeField] private Camera cam;

    [SerializeField] private List<Collider2D> colliderList;

    private void Start()
    {
        cam = Camera.main;

        var (center, size) = CaculateOrthoSize();
        cam.transform.position = center;
        cam.orthographicSize = size;
    }

    private (Vector3 center, float size) CaculateOrthoSize()
    {
        var bounds = new Bounds();

        foreach (var col in colliderList)
        {
            bounds.Encapsulate(col.bounds);
        }

        bounds.Expand(buffer);

        var vertical = bounds.size.y;
        var horizontal = bounds.size.x * cam.pixelHeight / cam.pixelWidth;

        var size = Mathf.Max(horizontal, vertical) * 0.5f;
        var center = bounds.center + new Vector3(0f, 0f, -10f);

        return (center, size);
    }

    private void OnDrawGizmosSelected()
    {
        colliderList.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            colliderList.Add(transform.GetChild(i).GetComponent<Collider2D>());
        }
    }
}
