using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public List<GameObject> TargetObjects = new(); // The stacks the camera can transition between
    private Transform _target; // Current target object for the camera
    [SerializeField]
    private float _distance = 25.0f; // Initial distance from the target
    [SerializeField]
    private float _xSpeed = 120.0f; // Rotation speed around the y-axis
    [SerializeField]
    private float _ySpeed = 120.0f; // Rotation speed around the x-axis

    [SerializeField]
    private float _yMinLimit = -20f; // Minimum vertical angle relative to the target
    [SerializeField]
    private float _yMaxLimit = 80f; // Maximum vertical angle relative to the target

    [SerializeField]
    private float _distanceMin = 5f; // Minimum zoom distance
    [SerializeField]
    private float _distanceMax = 45f; // Maximum zoom distance
    [SerializeField]
    private float _transitionSpeed = 25.0f; // Speed of the transition between targets

    private Vector3 _targetPosition; // Target position for camera transition
    private Quaternion _targetRotation; // Target rotation for camera transition

    private float _x = 0.0f; // Horizontal angle
    private float _y = 0.0f; // Vertical angle

    private int _currentViewIndex = 1; // Index of the current view target
    private bool _isTransitioning = false; // Flag to check if the camera is transitioning

    // Use this for initialization
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        _x = angles.y;
        _y = angles.x;
    }

    private void Update()
    {
        if (TargetObjects.Count > _currentViewIndex)
            _target = TargetObjects[_currentViewIndex].transform;

        // Check for right mouse click to trigger transition
        if (Input.GetMouseButtonDown(1))
        {
            TransitionToNextObject();
        }
    }

 
    void LateUpdate()
    {
        if (_target && !_isTransitioning) //Make sure there is a target and do nothing if its currently transitioning to the next target
        {
            if (Input.GetMouseButton(0))
            {
                _x += Input.GetAxis("Mouse X") * _xSpeed * _distance * 0.02f;
                _y -= Input.GetAxis("Mouse Y") * _ySpeed * 0.02f;

                _y = ClampAngle(_y, _yMinLimit, _yMaxLimit);
            }

            _distance = Mathf.Clamp(_distance - Input.GetAxis("Mouse ScrollWheel") * 5, _distanceMin, _distanceMax);

            Quaternion rotation = Quaternion.Euler(_y, _x, 0);
            Vector3 negDistance = new Vector3(0.0f, 0.0f, -_distance);
            Vector3 position = rotation * negDistance + _target.position;

            transform.rotation = rotation;
            transform.position = position;
        }
    }

    //Keep the angle within a range so it doesnt flip upside down
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }


    public void TransitionToNextObject()
    {
        if (TargetObjects.Count == 0) return; // If no views, do nothing

        Vector3 currentOffset = transform.position - _target.gameObject.transform.position;

        _currentViewIndex = (_currentViewIndex + 1) % TargetObjects.Count;

        _target = TargetObjects[_currentViewIndex].transform; // Change the current target


        // Calculate the target position and rotation
        _targetPosition = TargetObjects[_currentViewIndex].transform.position + currentOffset;
        _targetRotation = transform.rotation;

        // Transition the camera towards the target position and rotation
        StartCoroutine(MoveToNextTarget());
    }

    private IEnumerator MoveToNextTarget()
    {
        _isTransitioning = true;
        while (Vector3.Distance(transform.position, _targetPosition) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * _transitionSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, _targetRotation, Time.deltaTime * _transitionSpeed);
            yield return null;
        }

        // Move to the next target
        _isTransitioning = false;
    }



}
