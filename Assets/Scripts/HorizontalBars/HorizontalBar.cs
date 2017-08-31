using System.Collections.Generic;
using System.Linq;
using EraseGame.Delegates;
using UnityEngine;

public class HorizontalBar : MonoBehaviour
{
    public event BlockEvent OnBlockDied;

    public event ColorEvent OnUpdateColor;

    public List<BreakableBlock> Blocks;

    private bool _canDamage = true;
    private float _lastHealthPercent = 1f;

    public void Awake()
    {
        Blocks = GetComponentsInChildren<BreakableBlock>().ToList();
        Blocks.ForEach(block =>
       {
           block.OnDamaged += BlockDamaged;
           block.OnDied += BlockDied;
           OnUpdateColor += block.ChangeColor;
       });
    }

    public Transform DestroyRandomBlock()
    {
        var block = Blocks[Random.Range(0, Blocks.Count - 1)];
        var returnTransform = block.transform;
        block.Damage(block.MaxHealth);
        return returnTransform;
    }

    private void BlockDamaged(BreakableBlock block)
    {
        if (!_canDamage)
            return;

        OnUpdateColor -= block.ChangeColor;
        var healthPercent = block.Health / block.MaxHealth;

        if (healthPercent > 0f)
            block.ChangeColor(Color.Lerp(Color.white, new Color(1f, 1f, 1f, 0f), healthPercent));

        // only update colors if we're getting darker.
        if (_lastHealthPercent > healthPercent)
            UpdateBlockColors(healthPercent);

        _lastHealthPercent = healthPercent;
    }

    private void BlockDied(BreakableBlock block)
    {
        UpdateBlockColors(0f);
        _canDamage = false;
        Blocks.ForEach(b => { b.CanDamage = _canDamage; });
        OnBlockDied?.Invoke(block);
    }

    private void UpdateBlockColors(float healthPercent)
    {
        //broadcast the new color to all the blocks.
        OnUpdateColor?.Invoke(Color.Lerp(Color.black, Color.white, healthPercent));
    }
}