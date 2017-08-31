using EraseGame.Delegates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralWorldScroller : MonoBehaviour
{
    /// <summary>
    /// Called when a new horizontal bar is instantiated.
    /// </summary>
    public event BarSpawn OnBarSpawned;

    public Vector2 AimLocation;
    public float AimSpeed = 10f;
    public float AimTime = 50f;
    public HorizontalBar HorizontalBarPrefab;
    public float InitialSpawnDistance = 6f;
    public Material LineMaterial;

    /// <summary>
    /// The speed at which the screen scrolls during transitions.
    /// </summary>
    public float ScrollSpeed = 10f;

    public bool ShouldDrawAim;
    private List<HorizontalBar> _activeBars;
    private float _aimAngle;
    private LineRenderer _lineRenderer;

    public void SpawnNextBar()
    {
        var newBar = Instantiate(HorizontalBarPrefab, transform, false);
        OnBarSpawned?.Invoke(newBar);
        _activeBars.Add(newBar);
        StartCoroutine(ScrollBars());
    }

    private IEnumerator AimAndFire()
    {
        ShouldDrawAim = true;

        float counter = 0f;
        float randomAimSpeed = Random.Range(AimSpeed - 5f, AimSpeed + 5f);

        while (counter < AimTime)
        {
            counter += Time.deltaTime;
            _aimAngle = Mathf.PingPong(Time.time * randomAimSpeed, 120f) - 60f;
            yield return null;
        }
        _lineRenderer.StopRendering();
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
        if (ray2d.collider.tag == "Block")
        {
            _lineRenderer.StartRendering(new Vector2[] { AimLocation, ray2d.point });
        }
        else
        {
            _lineRenderer.StopRendering();
        }
    }

    private IEnumerator ScrollBars()
    {
        while (_activeBars[0].transform.position.y > 0)
        {
            _activeBars.ForEach(bar => bar.transform.position -= Time.deltaTime * ScrollSpeed * Vector3.down);
            yield return null;
        }
    }

    private void SetupInitialStage()
    {
        var firstBar = Instantiate(HorizontalBarPrefab, transform, false);
        var destroyedBlockLocation = firstBar.DestroyRandomBlock().position;
        var secondBar = Instantiate(HorizontalBarPrefab, new Vector3(0f, InitialSpawnDistance, 0f), Quaternion.identity, transform);
        destroyedBlockLocation.y += 0.5f;
        AimLocation = destroyedBlockLocation;
        StartCoroutine(AimAndFire());
    }

    private void Update()
    {
        if (ShouldDrawAim)
            DrawAim(Quaternion.AngleAxis(_aimAngle, Vector3.forward) * Vector2.up);
    }
}