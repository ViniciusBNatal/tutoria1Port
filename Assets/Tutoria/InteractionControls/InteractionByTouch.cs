using UnityEngine;

[RequireComponent(typeof(InteractionProcessor))]
public class InteractionByTouch : MonoBehaviour
{
    [SerializeField] private RotateByTouch _rotateByTouch;
    [SerializeField] private float _clickActionWindow;
    [SerializeField] private float _interactionRange = 6f;
    private InteractionProcessor _interactionProcessor;
    private float _currentActionWindow;
    private Camera _camera;
    private Vector2 _initialContactPoint;

    private void Awake()
    {
        _interactionProcessor = GetComponent<InteractionProcessor>();
        _camera = GetComponent<Camera>();   
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch input = Input.GetTouch(0);

            if (input.phase == TouchPhase.Began)
            {
                _currentActionWindow = _clickActionWindow;
                _initialContactPoint = input.position;
            }
            else if (input.phase == TouchPhase.Ended && _currentActionWindow >= 0f)
            {
                Physics.Raycast(_camera.ScreenPointToRay(_initialContactPoint), out RaycastHit hit, _interactionRange);
                _interactionProcessor.ProcessInteraction(hit.transform);
            }
        }
        _currentActionWindow -= Time.deltaTime;
    }    
}
