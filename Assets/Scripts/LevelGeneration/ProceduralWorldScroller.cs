using EraseGame;
using EraseGame.Delegates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralWorldScroller : MonoBehaviour
{
    #region Public Events

    /// <summary>
    /// Called when an aim angle has been decided.
    /// </summary>
    public event EmptyEvent OnAimComplete;

    /// <summary>
    /// Called when a new horizontal bar is instantiated.
    /// </summary>
    public event BarEvent OnBarSpawned;

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

    #endregion Public Events

    #region Public Fields

    /// <summary>
    /// Maximum angle to aim in for both positive and negative values.
    /// </summary>
    public float AimAngleMax = 60f;

    /// <summary>
    /// Location to fire from
    /// </summary>
    public Vector2 AimLocation;

    /// <summary>
    /// Speed the targeting preview will pingpong at.
    /// </summary>
    public float AimSpeed = 10f;

    /// <summary>
    ///  How long the aim will pingpong for.
    /// </summary>
    public float AimTime = 50f;

    /// <summary>
    /// Time to pause after an aim angle has been determined.
    /// </summary>
    public float BreakTime = 1f;

    /// <summary>
    ///  Prefab to spawn when moving to the next level.
    /// </summary>
    public HorizontalBar HorizontalBarPrefab;

    /// <summary>
    /// Distance from Y = 0 the next horizontal bar will be spawned.
    /// </summary>
    public float InitialSpawnDistance = 6f;

    /// <summary>
    /// The speed at which the screen scrolls during transitions.
    /// </summary>
    public float ScrollSpeed = 10f;

    /// <summary>
    /// if we should be drawing our targeting preview or not.
    /// </summary>
    public bool ShouldDrawAim;

    #endregion Public Fields

    #region Private Fields

    private List<HorizontalBar> _activeBars;
    private float _aimAngle;
    private LineRenderer _lineRenderer;
    private float _nextAimLocationX;

    #endregion Private Fields

    #region Public Methods

    /// <summary>
    /// Spawns the next horizontal bar
    /// </summary>
    public void SpawnNextBar(Vector3 position)
    {
        var newBar = Instantiate(HorizontalBarPrefab, position, Quaternion.identity, transform);
        OnBarSpawned?.Invoke(newBar);
        _activeBars.Add(newBar);
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Starts the animing sequence and ends in firing.
    /// </summary>
    private IEnumerator Aim()
    {
        // Start rendering our line.
        ShouldDrawAim = true;

        float counter = 0f;

        // Pick our aim angle after AimTime amount of time.
        while (counter < AimTime)
        {
            counter += Time.deltaTime;
            // Ping Pongs between -AimAngleMax and Positive AimAngleMax
            _aimAngle = Mathf.PingPong(Time.time * AimSpeed, AimAngleMax * 2f) - AimAngleMax;
            yield return null;
        }
    }

    private void Awake()
    {
        _lineRenderer = FindObjectOfType<LineRenderer>();
        _activeBars = new List<HorizontalBar>();
        SetupInitialStage();
    }

    private void DrawAim(Vector2 target)
    {
        RaycastHit2D ray2d;
        ray2d = Physics2D.Raycast(AimLocation, target, 2000f);
        if (ray2d)
        {
            var points = new List<Vector2> { AimLocation, ray2d.point };
            // Test for bounces if we aren't aiming straight at  block.
            if (ray2d.collider.tag != "Block")
            {
                var dir = AimLocation.GetDirection(ray2d.point);
                var bouncePoints = GatherBounceTargets(ray2d.point, dir);
                points.AddRange(bouncePoints);
            }
            _lineRenderer.StartRendering(points);
        }
        else
        {
            // No ray, dont render.
            _lineRenderer.StopRendering();
        }
    }

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

    private void OnBlockDied(BreakableBlock block)
    {
        // save our next aiming x.
        _nextAimLocationX = block.transform.position.x;
    }

    private IEnumerator ScrollBars()
    {
        // move all bars down until our bottom-most bar is at the bottom.
        while (_activeBars[0].transform.position.y > 0)
        {
            _activeBars.ForEach(bar => bar.transform.position -= Time.deltaTime * ScrollSpeed * Vector3.down);
            yield return null;
        }
        // toss out our old bottom since it's now off screen.
        _activeBars.RemoveAt(0);
        // set our aim location to the next aim location we got when the block died.
        AimLocation.x = _nextAimLocationX;
    }

    private void SetupInitialStage()
    {
        // spawn our first bar at our location (should be 0,0)
        SpawnNextBar(transform.position);
        // get the location of the randomly destroyed hole
        var destroyedBlockLocation = _activeBars[0].DestroyRandomBlock().position;
        // spawn the second bar at the initial spawn distance
        SpawnNextBar(transform.position + (Vector3.up * InitialSpawnDistance));
        // offset it so we're aiming out the top of a block.
        destroyedBlockLocation.y += 0.5f;
        // set our aim location to the destroyed blocks location.
        AimLocation = destroyedBlockLocation;
        // start the game essentially.
    }

    private IEnumerator StartFiring()
    {
        var counter = 0f;
        // Give the player some time to try and react.
        while (counter < BreakTime)
        {
            counter += Time.deltaTime;
            yield return null;
        }
        // Hide aiming line
        ShouldDrawAim = false;
        _lineRenderer.StopRendering();
    }

    private void Update()
    {
        if (ShouldDrawAim)
            DrawAim(Quaternion.AngleAxis(_aimAngle, Vector3.forward) * Vector2.up);
    }

    #endregion Private Methods
}