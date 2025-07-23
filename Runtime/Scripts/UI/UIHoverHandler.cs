using Oculus.Interaction;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class UIHoverHandler : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerClickHandler
{
    [Tooltip("How long (in seconds) to dwell before triggering.")]
    public float dwellTime = 1f;

    [Tooltip("Image used as the radial progress indicator (type=Filled).")]
    public Image fillImage;

    private Toggle _toggle;
    private Button _button;
    private Coroutine _dwellRoutine;

    [SerializeField]
    private bool ignoreNextEnter = false, canKeepOnHover = false;
    private PointerEventData _lastEventData;
    private bool _isHovering = false;
    void Awake()
    {
        if (fillImage != null)
        {
            fillImage.raycastTarget = false;
            fillImage.fillAmount = 0;
        }

        _toggle = GetComponent<Toggle>();
        _button = GetComponent<Button>();

        // Initialize fill based on toggle state (buttons start empty)
        if (_toggle != null)
            ApplyFill(_toggle.isOn);
        else
            ApplyFill(false);

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _isHovering = true;
        _lastEventData = eventData;
        if (ignoreNextEnter)
        {
            ignoreNextEnter = false;
            if(!canKeepOnHover)
                return;
        }
        if (_dwellRoutine == null)
            _dwellRoutine = StartCoroutine(DwellRoutine());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isHovering = false;
        CancelDwell();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Prevent immediately re-entering hover
        ignoreNextEnter = true;
        CancelDwell();
        // If this is a toggle, let Unity handle flipping it and then reset fills
        if (_toggle != null)
            StartCoroutine(DelayedGroupUpdate());
        // If it's a button, its own onClick will fire automatically—
        // we just need to reset our fill next frame:
        else if (_button != null)
            StartCoroutine(DelayedFillReset());

        //if (canKeepOnHover)
        //{
        //    StartCoroutine(ReenterIfStillHovered());
        //}
    }
    private IEnumerator ReenterIfStillHovered()
    {
        yield return null; // wait one frame so fill resets or toggle state updates

        // Make sure our ignore flag is clear
        ignoreNextEnter = false;

        if (_isHovering)
        {
            // Fire our OnPointerEnter again
            OnPointerEnter(_lastEventData);
        }
    }
    private IEnumerator DwellRoutine()
    {
        // small delay before starting to give click events priority
        yield return new WaitForSeconds(0.5f);

        float start = fillImage?.fillAmount ?? 0f;
        // for toggles, we invert; for buttons we always go to full
        float end = (_toggle != null)
            ? (_toggle.isOn ? 0f : 1f)
            : 1f;

        float elapsed = 0f;
        while (elapsed < dwellTime)
        {
            elapsed += Time.deltaTime;
            if (fillImage != null)
                fillImage.fillAmount = Mathf.Lerp(start, end, elapsed / dwellTime);
            yield return null;
        }

        // finalize the fill
        if (fillImage != null)
            fillImage.fillAmount = end;

        // trigger the control
        if (_toggle != null)
        {
            _toggle.isOn = !_toggle.isOn;
            ResetGroupFills();
        }
        else if (_button != null)
        {
            _button.onClick.Invoke();
            // immediately snap back to empty
            ApplyFill(false);
        }

        _dwellRoutine = null;

        if (canKeepOnHover)
        {
            print("Exceute reenter");
            yield return null;
            StartCoroutine(ReenterIfStillHovered());
        }
    }

    private IEnumerator DelayedGroupUpdate()
    {
        yield return null; // wait one frame
        ResetGroupFills();
    }

    private IEnumerator DelayedFillReset()
    {
        yield return null;
        ApplyFill(false);
    }

    private void ResetGroupFills()
    {
        if (_toggle?.group == null) return;

        foreach (var tog in _toggle.group.GetComponentsInChildren<Toggle>())
        {
            var dwell = tog.GetComponent<UIHoverHandler>();
            if (dwell != null)
                dwell.ApplyFill(tog.isOn);
        }
    }

    private void CancelDwell()
    {
        if (_dwellRoutine != null)
        {
            StopCoroutine(_dwellRoutine);
            _dwellRoutine = null;
        }
        // snap back to the real state
        if (_toggle != null)
            ApplyFill(_toggle.isOn);
        else
            ApplyFill(false);
    }

    /// <summary>
    /// Sets fillImage.fillAmount to 1 if on (toggle) or zero otherwise.
    /// </summary>
    public void ApplyFill(bool isOn)
    {
        if (fillImage != null)
            fillImage.fillAmount = isOn ? 1f : 0f;
    }
}
