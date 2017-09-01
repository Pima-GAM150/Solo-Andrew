using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LineRenderer : MonoBehaviour
{
    #region Public Fields

    /// <summary>
    /// Color at the end of the line.
    /// </summary>
    public Color EndColor;

    /// <summary>
    /// Color at the start of the line
    /// </summary>
    public Color StartColor;

    #endregion Public Fields

    #region Private Fields

    private Material _lineMaterial;
    private List<Vector2> _points;
    private bool _shouldRender;

    #endregion Private Fields

    #region Public Methods

    public void SetupMaterial()
    {
        // 100% copy pasted right from unity docs.
        // Unity has a built-in shader that is useful for drawing
        // simple colored things.
        Shader shader = Shader.Find("Hidden/Internal-Colored");
        _lineMaterial = new Material(shader)
        {
            hideFlags = HideFlags.HideAndDontSave
        };
        // Turn on alpha blending
        _lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        _lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        // Turn backface culling off
        _lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
        // Turn off depth writes
        _lineMaterial.SetInt("_ZWrite", 0);
    }

    /// <summary>
    /// Starts drawing lines OnPostRender
    /// </summary>
    /// <param name="points">vertecies of the line to draw Must be an even number!</param>
    public void StartRendering(List<Vector2> points)
    {
        _points = points;
        _shouldRender = true;
    }

    /// <summary>
    /// Stops this from drawing lines.
    /// </summary>
    public void StopRendering()
    {
        _shouldRender = false;
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Used to visualize in scene view.
    /// </summary>
    private void OnDrawGizmos()
    {
        if (_shouldRender && _points.Any())
            RenderLine();
    }

    /// <summary>
    /// Draws the line.
    /// </summary>
    private void OnPostRender()
    {
        if (_shouldRender && _points.Any())
            RenderLine();
    }

    /// <summary>
    /// Renders GL lines based on the points list.
    /// </summary>
    private void RenderLine()
    {
        // idk the purpose of any of this but its required
        GL.PushMatrix();
        GL.LoadOrtho();
        SetupMaterial();
        _lineMaterial.SetPass(0);
        GL.Begin(GL.LINES);

        // sets the vertex colors based on its distance from being the end point.
        for (int i = 0; i < _points.Count; i++)
        {
            var percent = i / (_points.Count - 1);
            var color = Color.Lerp(StartColor, EndColor, percent);
            GL.Color(color);
            GL.Vertex(_points[i]);
        }
        GL.PopMatrix();
        GL.End();
    }

    #endregion Private Methods
}