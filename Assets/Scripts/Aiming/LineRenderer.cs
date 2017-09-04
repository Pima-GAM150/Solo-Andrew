using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using EraseGame.Structs.Utility;

public class LineRenderer : MonoBehaviour
{
    #region Public Enums

    public enum LineStyle
    {
        Preview,
        Test,
        Invalid,
        Valid
    }

    #endregion Public Enums

    #region Public Fields + Properties

    /// <summary>
    /// Which color pallette should be used.
    /// </summary>
    public LineStyle CurrentStyle;

    /// <summary>
    /// How thick the line should be drawn.
    /// </summary>
    public float LineThickness = 0.02f;

    public ColorPair PreviewColors;
    public ColorPair TestColors;
    public ColorPair ValidColors;
    public ColorPair InvalidColors;

    #endregion Public Fields + Properties

    #region Private Fields + Properties

    private Material _lineMaterial;

    private List<Vector2> _points;

    private bool _shouldRender;

    #endregion Private Fields + Properties

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

    // Redirect Awake to setup material.
    private void Awake() => SetupMaterial();

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
        _lineMaterial.SetPass(0);
        GL.Begin(GL.QUADS);
        var colorPair = GetCurrentStyle();
        // sets the vertex colors based on its distance from being the end point.
        for (int i = 0; i < _points.Count; i++)
        {
            // fixed point at 3 (4 points) so our line doesn't suddenly swap colors as it bounces.
            var percent = i / 3;

            var color = Color.Lerp(colorPair.PrimaryColor, colorPair.SecondaryColor, percent);
            GL.Color(color);
            GL.Vertex(_points[i] + (Vector2.left * LineThickness));
            GL.Vertex(_points[i] + (Vector2.right * LineThickness));
        }
        GL.PopMatrix();
        GL.End();
    }

    private ColorPair GetCurrentStyle()
    {
        switch (CurrentStyle)
        {
            case LineStyle.Preview:
                return PreviewColors;

            default:
            case LineStyle.Test:
                return TestColors;

            case LineStyle.Invalid:
                return InvalidColors;

            case LineStyle.Valid:
                return ValidColors;
        }
    }

    #endregion Private Methods
}