using EraseGame;
using EraseGame.Delegates;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AttackController))]
public class ProceduralWorldScroller : MonoBehaviour
{
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

    private EventHub _eventHub => EventHub.GetEventHub();
    private List<HorizontalBar> _activeBars;
    private AttackController _attackController;

    /// <summary>
    /// Spawns the next horizontal bar
    /// </summary>
    public void SpawnNextBar(Vector3 position)
    {
        var newBar = Instantiate(HorizontalBarPrefab, position, Quaternion.identity, transform);
        _eventHub.InvokeOnBarSpawned(this, newBar);
        _activeBars.Add(newBar);
    }

    private void Awake()
    {
        _attackController = GetComponent<AttackController>();
        _activeBars = new List<HorizontalBar>();
        SetupInitialStage();
    }

    private void OnBlockDied(BreakableBlock block)
    {
        // save our next aiming x.
        _attackController.NextAimLocationX = block.transform.position.x;
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
        _eventHub.InvokeOnScrollComplete(this, _activeBars.Last());
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
        _attackController.CurrentAimLocation = destroyedBlockLocation;
        // start the game essentially.
        // TODO move this to some sort of state machine later.
        StartCoroutine(_attackController.Aim());
    }
}