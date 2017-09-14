using EraseGame;
using EraseGame.Delegates;
using UnityEngine;
using UnityEngine.EventSystems;

public class BreakableBlock : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// Determines if this block can be damaged or not.
    /// </summary>
    [HideInInspector]
    public bool CanDamage = true;

    /// <summary>
    /// How much damage a click deals to this block.
    /// </summary>
    public float ClickDamage = 1f;

    /// <summary>
    /// How much heal this block has.
    /// </summary>
    public float Health = 4f;

    /// <summary>
    /// Set to Health on awake.
    /// </summary>
    [HideInInspector]
    public float MaxHealth;

    private EventHub _eventHub => EventHub.GetEventHub();
    private SpriteRenderer _spRenderer;
    private Color _targetColor;

    /// <summary>
    /// Sets the color of this block.
    /// </summary>
    /// <param name="color">color to set to.</param>
    public void ChangeColor(Color color)
    {
        _targetColor = color;
    }

    /// <summary>
    /// Applies damage to this block
    /// </summary>
    /// <param name="value">amount of damage</param>
    public void Damage(float value = 1f)
    {
        // Do nothing if we can't be damaged.
        if (!CanDamage)
            return;
        Debug.Log($"Applying Damage to {name}");
        // Clamp our health just to avoid negatives.
        Health = Mathf.Clamp(Health - value, 0f, MaxHealth);

        // Broadcast we've taken damage.
        _eventHub.InvokeOnBlockDamaged(this, this);

        // die if we're at 0 health.
        if (Health <= 0)
        {
            Kill();
        }
    }

    /// <summary>
    /// Kills this block.
    /// </summary>
    public void Kill()
    {
        _eventHub.InvokeOnBlockBreak(this, this);

        Destroy(gameObject); // TODO: Some sort of death animation.
    }

    /// <summary>
    /// <para>Called when the pointer clicks on this object.</para>
    /// </summary>
    /// <param name="eventData">Current event data.</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        Damage(ClickDamage);
    }

    private void Start()
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