using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PointCloud : MonoBehaviour, IDragHandler
{
    public const int Width = 640, Height = 480;

    private Vector3[] positions;
    private int[] indices;
    private Color[] colors;
    private Mesh mesh;
    // Use this for initialization
    void Start()
    {
        var totalPointNum = Width * Height;
        positions = new Vector3[totalPointNum];
        indices = new int[totalPointNum];
        colors = new Color[totalPointNum];
        for (int i = 0; i < totalPointNum; i++)
        {
            positions[i] = new Vector3(0, 0, 0);
            indices[i] = i;
            colors[i] = Color.red;
        }
        mesh = GetComponent<MeshFilter>().mesh;
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.vertices = positions;
        mesh.colors = colors;
        mesh.SetIndices(indices, MeshTopology.Points, 0);
        mesh.MarkDynamic();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            this.transform.Rotate(eventData.delta.y, eventData.delta.x, 0);
        }
    }
}
