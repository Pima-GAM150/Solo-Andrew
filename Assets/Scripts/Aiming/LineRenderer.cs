using System.Linq;
using UnityEngine;

public class LineRenderer : MonoBehaviour
{
    private Vector2[] _points;
    private bool _shouldRender;

    public void StartRendering(Vector2[] points)
    {
        _points = points;
        _shouldRender = true;
    }

    public void StopRendering()
    {
        _shouldRender = false;
    }

    private void OnDrawGizmos()
    {
        if (_shouldRender && _points.Any())
            RenderLine();
    }

    private void OnPostRender()
    {
        if (_shouldRender && _points.Any())
            RenderLine();
    }

    private void RenderLine()
    {
        //GL.LoadOrtho();
        GL.Begin(GL.LINES);
        GL.Color(Color.red);
        foreach (var point in _points)
        {
            GL.Vertex(point);
        }
        //GL.PopMatrix();
        GL.End();
    }
}