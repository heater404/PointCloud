using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class PointCloudCSHelper : MonoBehaviour
{
    public ComputeShader shader;
    public Material material;

    ComputeBuffer pointPosBuffer;
    ComputeBuffer pointColBuffer;
    int PointCloudPotNum { get { return PointCloud.Width * PointCloud.Height; } }
    int kernel;
    uint x, y, z;
    void Start()
    {
        kernel = shader.FindKernel("UpdatePointCloud");

        pointPosBuffer = new ComputeBuffer(PointCloudPotNum, 12);
        shader.SetBuffer(kernel, "PointPos", pointPosBuffer);
        material.SetBuffer("PointPos", pointPosBuffer);

        pointColBuffer = new ComputeBuffer(PointCloudPotNum, 16);
        shader.SetBuffer(kernel, "PointCol", pointColBuffer);
        material.SetBuffer("PointCol", pointColBuffer);

        shader.GetKernelThreadGroupSizes(kernel, out x, out y, out z);

        StartCoroutine(UpdatePointCloudData());
    }

    IEnumerator UpdatePointCloudData()
    {
        while (true)
        {
            shader.SetFloat("Time", Time.time);
            shader.Dispatch(kernel, (int)(PointCloudPotNum / (x * y * z)), 1, 1);
            yield return null;
        }
    }

    //void OnRenderObject()
    //{
    //    //material.SetPass(0);
    //    //Graphics.DrawProcedural(MeshTopology.Points, width * height);
    //}

    void OnDestroy()
    {
        pointPosBuffer.Release();
        pointPosBuffer.Dispose();

        pointColBuffer.Release();
        pointColBuffer.Dispose();
    }

    Vector3[] GetPositionsFromCsv(string path)
    {
        List<Vector3> pos = new List<Vector3>();
        using (StreamReader sr = new StreamReader(path))
        {
            while (!sr.EndOfStream)
            {
                var positions = sr.ReadLine().Split(',');

                pos.Add(new Vector3(float.Parse(positions[0]), float.Parse(positions[1]), float.Parse(positions[2])));
            }
        }

        return pos.ToArray();
    }

    private Color[] GetColors(int num)
    {
        Color[] colors = new Color[num];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = UnityEngine.Random.ColorHSV();
        }

        return colors;
    }
}
