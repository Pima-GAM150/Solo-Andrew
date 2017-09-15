using EraseGame;
using EraseGame.Delegates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using static LineRenderer;

public class AttackController : MonoBehaviour
{
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

    private EventHub _eventHub => EventHub.GetEventHub();

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
        _eventHub.InvokeOnAimComplete(this);
    }

    private void Start()
    {
        _lineRenderer = FindObjectOfType<LineRenderer>();
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        _eventHub.OnScrollComplete += EventOnScrollComplete;
        _eventHub.OnAimComplete += EventOnAimComplete;
        _eventHub.OnBlockBreak += EventBlockBreak;
    }

    private void EventBlockBreak(BreakableBlock block)
    {
        NextAimLocationX = block.transform.position.x;
    }

    private void EventOnAimComplete()
    {
        StartCoroutine(StartFiring());
    }

    private void EventOnScrollComplete(HorizontalBar bar)
    {
        if (bar != null)
        {
            SpeedUp();
            CurrentAimLocation.x = NextAimLocationX;
        }

        StartCoroutine(Aim());
    }

    private void SpeedUp()
    {
        AimSpeed *= 1.2f;
        AimTime *= 0.95f;
        BreakTime = Mathf.Pow(BreakTime, 0.9f) + 0.5f; // our minimum will be 1.5f
    }

    /// <summary>
    /// Collects the points to push to the LineRenderer
    /// </summary>
    /// <returns>Ends when ShouldDrawAim is set to false.</returns>
    private IEnumerator DrawAim()
    {
        while (ShouldDrawAim)
        {
            RaycastHit2D ray2d;
            ray2d = Physics2D.Raycast(CurrentAimLocation, AimTarget, 2000f);
            if (ray2d)
            {
                var points = new List<Vector2> { CurrentAimLocation, ray2d.point };
                // Test for bounces if we aren't aiming straight at  block.
                if (ray2d.collider.tag != "Block" && ray2d.collider.tag != "EndCollider")
                {
                    var dir = CurrentAimLocation.GetDirection(ray2d.point);
                    var bouncePoints = GatherBounceTargets(ray2d.point, dir);
                    points.AddRange(bouncePoints);
                }
                _lineRenderer.StartRendering(points);
            }
            yield return null;
        }
        _lineRenderer.StopRendering();
    }

    private bool DrawFire(LineStyle style)
    {
        _lineRenderer.CurrentStyle = style;
        RaycastHit2D ray2d;
        ray2d = Physics2D.Raycast(CurrentAimLocation, AimTarget, 2000f);
        bool hitEnd = false;
        if (ray2d)
        {
            var points = new List<Vector2> { CurrentAimLocation, ray2d.point };
            // Test for bounces if we aren't aiming straight at  block.

            bool isHittingBoxBottom = ray2d.collider.tag == "Block" && ray2d.normal == Vector2.down;

            // if we arent hitting the bottom of a block
            if (!isHittingBoxBottom)
            {
                // and we arent hitting the end
                if (ray2d.collider.tag != "EndCollider")
                {
                    // check for bounces
                    var dir = CurrentAimLocation.GetDirection(ray2d.point);
                    var bouncePoints = GatherBounceTargets(ray2d.point, dir, "EndCollider");
                    points.AddRange(bouncePoints);

                    var endRay = Physics2D.Raycast(points.Last() + Vector2.down, Vector2.up, 100f);
                    Debug.DrawLine(points.Last() + Vector2.down, (points.Last() + Vector2.down) + (Vector2.up * 100f), Color.black);
                    hitEnd = (endRay && endRay.collider.tag == "EndCollider");
                }
                // only possible if we've passed through a block.
                else hitEnd = true;
            }
            _lineRenderer.StartRendering(points);
        }
        return hitEnd;
    }

    /// <summary>
    /// Collects a Vector2 List of points for the line renderer to use.
    /// </summary>
    /// <returns>Vector2 List</returns>
    private List<Vector2> GatherBounceTargets(Vector2 location, Vector2 direction, string targetTag = "Block", int bounces = 6)
    {
        // setup for collecting bounces
        List<Vector2> points = new List<Vector2>();

        RaycastHit2D ray2d;
        int curBounces = 0;
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
            curBounces++;
        }
        // stop when we hit a block
        while (ray2d && ray2d.collider.tag != targetTag && curBounces < bounces);
        return points;
    }

    /// <summary>
    /// Traces the path and checks if we have a win or fail state.
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartFiring()
    {
        Debug.Log($"[{Time.time}] Stopping Draw Aim");
        ShouldDrawAim = false;
        var counter = 0f;

        while (counter < BreakTime)
        {
            DrawFire(LineStyle.Test);
            counter += Time.deltaTime;
            yield return null;
        }

        _eventHub.InvokeOnBreakTimerComplete(this);
        counter = 0f;
        // test if we have a hit

        var success = DrawFire(LineStyle.Test);
        while (counter < BreakTime)
        {
            DrawFire(LineStyle.Test);
            counter += Time.deltaTime;
            yield return null;
        }

        if (success)
        {
            DrawFire(LineStyle.Valid);
            yield return new WaitForSeconds(1f);
            _eventHub.InvokeOnFireSuccess(this);
        }
        else
        {
            DrawFire(LineStyle.Invalid);
            yield return new WaitForSeconds(1f);
            _eventHub.InvokeOnFireFailed(this);
        }
        _lineRenderer.StopRendering();
    }
}