using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using EraseGame.Delegates;

public class BreakableBlock : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// Called when the block is tapped on to deal damage.
    /// </summary>
    public event BlockEvent OnDamaged;

    /// <summary>
    /// Called when this block is being destroyed.
    /// </summary>
    public event BlockEvent OnDied;

    public bool CanDamage = true;
    public float ClickDamage = 1f;
    public float Health = 4f;

    [HideInInspector]
    public float MaxHealth;

    private SpriteRenderer _spRenderer;
    private Color _targetColor;

    public void ChangeColor(Color color)
    {
        _targetColor = color;
    }

    public void Damage(float value = 1f)
    {
        if (!CanDamage)
            return;

        Health = Mathf.Clamp(Health - value, 0f, MaxHealth);
        OnDamaged?.Invoke(this);

        if (Health <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        OnDied?.Invoke(this);

        Destroy(gameObject); // TODO: Some sort of death animation.
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked");
        Damage(ClickDamage);
    }

    private void Awake()
    {
        MaxHealth = Health;
        _spRenderer = GetComponent<SpriteRenderer>();
        _targetColor = _spRenderer.color;
    }

    private void Update()
    {
        // If the alpha isn't equal or the r channel isn't equal.
        if (!Mathf.Approximately(_spRenderer.color.a, _targetColor.a) ||
             !Mathf.Approximately(_spRenderer.color.r, _targetColor.r))
        {
            _spRenderer.color = Color.Lerp(_spRenderer.color, _targetColor, Time.deltaTime * 10f);
        }
    }
}