using EraseGame.Delegates;
using UnityEngine;
using UnityEngine.EventSystems;

public class BreakableBlock : MonoBehaviour, IPointerClickHandler
{
    #region Public Events

    /// <summary>
    /// Called when the block is tapped on to deal damage.
    /// </summary>
    public event BlockEvent OnDamaged;

    /// <summary>
    /// Called when this block is being destroyed.
    /// </summary>
    public event BlockEvent OnDied;

    #endregion Public Events

    #region Public Fields

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

    #endregion Public Fields

    #region Private Fields

    private SpriteRenderer _spRenderer;
    private Color _targetColor;

    #endregion Private Fields

    #region Public Methods

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

        // Clamp our health just to avoid negatives.
        Health = Mathf.Clamp(Health - value, 0f, MaxHealth);

        // Broadcast we've taken damage.
        OnDamaged?.Invoke(this);

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
        OnDied?.Invoke(this);

        Destroy(gameObject); // TODO: Some sort of death animation.
    }

    /// <summary>
    /// <para>Called when the pointer clicks on this object.</para>
    /// </summary>
    /// <param name="eventData">Current event data.</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked");
        Damage(ClickDamage);
    }

    #endregion Public Methods

    #region Private Methods

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

    #endregion Private Methods
}