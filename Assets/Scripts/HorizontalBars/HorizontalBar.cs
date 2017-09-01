using EraseGame.Delegates;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HorizontalBar : MonoBehaviour
{
    #region Public Events

    /// <summary>
    /// calls whenever a block is finally destroyed.
    /// </summary>
    public event BlockEvent OnBlockDied;

    /// <summary>
    /// called when the color is changing for non-broken blocks.
    /// </summary>
    public event ColorEvent OnUpdateColor;

    #endregion Public Events

    #region Public Fields

    /// <summary>
    /// List of the children blocks.
    /// </summary>
    public List<BreakableBlock> Blocks;

    #endregion Public Fields

    #region Private Fields

    private bool _canDamage;

    private float _lastHealthPercent = 1f;

    #endregion Private Fields

    #region Public Properties

    /// <summary>
    /// If the player is able to tap to deal damage to this bar or not.
    /// </summary>
    public bool CanDamage
    {
        get
        {
            return _canDamage;
        }
        set
        {
            Blocks.ForEach(b => b.CanDamage = value);
            _canDamage = value;
        }
    }

    #endregion Public Properties

    #region Public Methods

    public void Awake()
    {
        Blocks = GetComponentsInChildren<BreakableBlock>().ToList();
        // Subscribe to block events and subscribe the block to some of our events.
        Blocks.ForEach(block =>
       {
           block.CanDamage = CanDamage;
           block.OnDamaged += BlockDamaged;
           block.OnDied += BlockDied;
           OnUpdateColor += block.ChangeColor;
       });
    }

    /// <summary>
    /// Destroys a random block on this bar.
    /// </summary>
    /// <returns>the transform of the destroyed block</returns>
    public Transform DestroyRandomBlock()
    {
        CanDamage = true;
        var block = Blocks[Random.Range(0, Blocks.Count - 1)];
        var returnTransform = block.transform;
        block.Damage(block.MaxHealth);
        CanDamage = false;
        return returnTransform;
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Called when a block takes damage.
    /// </summary>
    /// <param name="block">the block damaged.</param>
    private void BlockDamaged(BreakableBlock block)
    {
        //exit if we can't deal damage.
        if (!CanDamage)
            return;

        // unsubscribe this block since it will have a damaged color.
        OnUpdateColor -= block.ChangeColor;

        var healthPercent = block.Health / block.MaxHealth;

        // Only bother coloring the block if it's not dead.
        if (healthPercent > 0f)
            block.ChangeColor(Color.Lerp(new Color(1f, 1f, 1f, 0f), Color.white, healthPercent));

        // only update colors if we're getting darker.
        if (_lastHealthPercent > healthPercent)
            UpdateBlockColors(healthPercent);

        // store for later.
        _lastHealthPercent = healthPercent;
    }

    /// <summary>
    /// Called when a block attached to this bar dies.
    /// </summary>
    /// <param name="block">the block that is going to be destroyed</param>
    private void BlockDied(BreakableBlock block)
    {
        UpdateBlockColors(0f);
        CanDamage = false;
        Blocks.ForEach(b => { b.CanDamage = CanDamage; });
        OnBlockDied?.Invoke(block);
    }

    /// <summary>
    /// Sets the color of blocks subscribed to OnUpdateColor
    /// </summary>
    /// <param name="healthPercent">0 to 1 percentage to lerp between black and white.</param>
    private void UpdateBlockColors(float healthPercent)
    {
        //broadcast the new color to all the blocks.
        OnUpdateColor?.Invoke(Color.Lerp(Color.black, Color.white, healthPercent));
    }

    #endregion Private Methods
}