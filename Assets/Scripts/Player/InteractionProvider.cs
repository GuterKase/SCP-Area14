using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(InteractionMarkEnablerDisabler))]
public class InteractionProvider : MonoBehaviour
{
    [SerializeField] private float _maxInteractionDistance;
    [SerializeField] private float _interactionSphereRadious;
    [SerializeField] private float _delayAfterInteraction;

    [Inject] private readonly RayProvider _rayProvider;
    [Inject] private readonly InventoryEnablerDisabler _inventoryEnablerDisabler;

    private IInteractable _interactable;
    private bool _isDelayGoing;
    private WaitForSeconds _timeoutAfterInteraction;

    public Action OnPlayerFindUnInteractable { get; set; }
    public Action<Collider> OnPlayerFindInteractable { get; set; }

    private void Start()
    {
        _inventoryEnablerDisabler.OnInventoryEnabledDisabled += SetActiveState;
        _timeoutAfterInteraction = new WaitForSeconds(_delayAfterInteraction);
    }

    private void SetActiveState()
    {
        enabled = !enabled;
    }

    private void Update()
    {
        if (_isDelayGoing) { return; }

        Collider raycastHit = GetRayCastHit();

        if (raycastHit == null)
        {
            OnPlayerFindUnInteractable?.Invoke();
            return;
        }

        OnPlayerFindInteractable?.Invoke(raycastHit);

        Interact();
    }

    private Collider GetRayCastHit()
    {
        bool raycasted = Physics.SphereCast(_rayProvider.ProvideRay(),
                                            _interactionSphereRadious,
                                            out RaycastHit raycastHit,
                                            _maxInteractionDistance);

        Collider collider = raycastHit.collider;

        return raycasted && collider.TryGetComponent(out _interactable) ? collider : null;
    }

    private void Interact()
    {
        if (!Input.GetButtonDown("Interaction")) { return; }

        StartCoroutine(StartInteractionDelay());
        _interactable.Interact();
    }

    private IEnumerator StartInteractionDelay()
    {
        OnPlayerFindUnInteractable?.Invoke();
        _isDelayGoing = true;

        yield return _timeoutAfterInteraction;

        _isDelayGoing = false;
    }

    private void OnDestroy()
    {
        _inventoryEnablerDisabler.OnInventoryEnabledDisabled -= SetActiveState;
    }
}