using EraseGame;
using EraseGame.Delegates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    /// <summary>
    /// Called when an aim angle has been decided.
    /// </summary>
    public event EmptyEvent OnAimComplete;

    /// <summary>
    /// Called when the post-aim timer has completed, just before firing.
    /// </summary>
    public event EmptyEvent OnBreakTimerComplete;

    /// <summary>
    /// Called if the fire event failed (due to hitting a block or other error)
    /// </summary>
    public event EmptyEvent OnFireFailed;

    /// <summary>
    /// Called if the fire event was successful.
    /// </summary>
    public event EmptyEvent OnFireSuccess;

    /// <summary>
    /// If we should be rendering our aim line or not.
    /// </summary>
    public bool ShouldDrawAim
    {
        get
        {
            return _shouldDrawAim;
        }
        set
        {
            if (_shouldDrawAim != value)
            {
                if (value)
                {
                    _shouldDrawAim = value;
                    StartCoroutine(DrawAim());
                }
                else
                {
                    _shouldDrawAim = value;
                }
            }
        }
    }

    /// <summary>
    /// Maximum angle to aim in for both positive and negative values.
    /// </summary>
    public float AimAngleMax = 60f;

    /// <summary>
    /// Speed the targeting preview will pingpong at.
    /// </summary>
    public float AimSpeed = 10f;

    /// <summary>
    /// Target to Aim At
    /// </summary>
    public Vector2 AimTarget;

    /// <summary>
    ///  How long the aim will pingpong for.
    /// </summary>
    public float AimTime = 50f;

    /// <summary>
    /// Time to pause after an aim angle has been determined.
    /// </summary>
    public float BreakTime = 1f;

    /// <summary>
    /// Location to fire from
    /// </summary>
    public Vector2 CurrentAimLocation;

    /// <summary>
    /// Next location to fire from
    /// </summary>
    public float NextAimLocationX;

    private LineRenderer _lineRenderer;
    private ProceduralWorldScroller _procWorldScroller;
    private bool _shouldDrawAim;

    /// <summary>
    /// Starts the animing sequence and ends in firing.
    /// </summary>
    public IEnumerator Aim()
    {
        // Start rendering our line.
        ShouldDrawAim = true;

        float counter = 0f;

        // Pick our aim angle after AimTime amount of time.
        while (counter < AimTime)
        {
            counter += Time.deltaTime;
            // Ping Pongs between -AimAngleMax and Positive AimAngleMax
            var _aimAngle = Mathf.PingPong(Time.time * AimSpeed, AimAngleMax * 2f) - AimAngleMax;
            AimTarget = Quaternion.AngleAxis(_aimAngle, Vector3.forward) * Vector2.up;
            yield return null;
        }
        OnAimComplete?.Invoke();
    }

    private void Awake()
    {
        _lineRenderer = FindObjectOfType<LineRenderer>();
    }

    /// <summary>
    /// Collects the points to push to the LineRenderer
    /// </summary>
    /// <returns>Ends when ShouldDrawAim is set to false.</returns>
    private IEnumerator DrawAim()
    {
        Debug.Log("Started Drawing Aim Line");
        while (ShouldDrawAim)
        {
            RaycastHit2D ray2d;
            ray2d = Physics2D.Raycast(CurrentAimLocation, AimTarget, 2000f);
            if (ray2d)
            {
                var points = new List<Vector2> { CurrentAimLocation, ray2d.point };
                // Test for bounces if we aren't aiming straight at  block.
                if (ray2d.collider.tag != "Block")
                {
                    var dir = CurrentAimLocation.GetDirection(ray2d.point);
                    var bouncePoints = GatherBounceTargets(ray2d.point, dir);
                    points.AddRange(bouncePoints);
                }
                _lineRenderer.StartRendering(points);
            }
            yield return null;
        }
        Debug.Log("Stopped Drawing Aim Line");
        _lineRenderer.StopRendering();
    }

    /// <summary>
    /// Collects a Vector2 List of points for the line renderer to use.
    /// </summary>
    /// <returns>Vector2 List</returns>
    private List<Vector2> GatherBounceTargets(Vector2 location, Vector2 direction)
    {
        // setup for collecting bounces
        List<Vector2> points = new List<Vector2>();

        RaycastHit2D ray2d;

        do
        {
            // add start point (since we're using GL.Begin we need a pair of vertecies.
            points.Add(location);
            // flip horizontally so we ping pong back and forth across the screen.
            direction.x *= -1;
            ray2d = Physics2D.Raycast(location, direction, 2000f);
            // add our hit location, this is our second point.
            points.Add(ray2d.point);

            // get the direction so we can use it next iteration
            direction = location.GetDirection(ray2d.point);
            // save location
            location = ray2d.point;
        }
        // stop when we hit a block
        while (ray2d && ray2d.collider.tag != "Block");
        return points;
    }

    /// <summary>
    /// Traces the path and checks if we have a win or fail state.
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartFiring()
    {
        var counter = 0f;
        // Give the player some time to try and react.
        while (counter < BreakTime)
        {
            counter += Time.deltaTime;
            yield return null;
        }
        // todo: Condition this
        OnFireFailed?.Invoke();
        OnFireSuccess?.Invoke();
    }
}