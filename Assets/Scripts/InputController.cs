using UnityEngine;

public class InputController
   : MonoBehaviour
{

    [SerializeField]
    private GameController _gameController;
    private float _doubleClickTime = 0.5f; // Time in seconds to detect double click
    private float _lastClickTime;
    private RaycastHit _hitInfo;

    [SerializeField]
    private Color _highlightColor = Color.yellow; // The color to use for highlighting
    private GameObject _highlightedObject = null;
    private Color _originalColor;


    private float _clickTimeThreshold = 0.2f; // Time threshold for a click
    private bool _isClickStarted = false;
    private float _clickStartTime;


    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        if (Physics.Raycast(ray, out _hitInfo))
        {
            if (_highlightedObject != _hitInfo.collider.gameObject)
            {
                ResetHighlight();
                _highlightedObject = _hitInfo.collider.gameObject;

                //Check if it's a Jenga Block
                if (_highlightedObject.GetComponent<JengaBlockObject>() != null)
                {
                    _originalColor = GetRendererColor(_highlightedObject);
                    SetRendererColor(_highlightedObject, _highlightColor);
                }
            }
        }
        else
        {
            ResetHighlight();
        }



        if (Input.GetMouseButtonDown(0)) // When left mouse button is pressed
        {
            _isClickStarted = true;
            _clickStartTime = Time.time;
        }

        if (_isClickStarted && Input.GetMouseButtonUp(0)) // When left mouse button is released
        {
            if (Time.time - _clickStartTime <= _clickTimeThreshold)
            {
                if (_highlightedObject != null && _highlightedObject.GetComponent<JengaBlockObject>() != null)
                {
                    _gameController.ShowBlockData(_highlightedObject.GetComponent<JengaBlockObject>());
                }
                else
                {
                    _gameController.ShowBlockData(null);
                }
            }

            _isClickStarted = false;
        }
    }

    void ResetHighlight()
    {
        if (_highlightedObject != null)
        {
            SetRendererColor(_highlightedObject, _originalColor);
            _highlightedObject = null;
        }
    }

    Color GetRendererColor(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            return renderer.material.color;
        }
        return Color.white; // Default color if no renderer is found
    }

    void SetRendererColor(GameObject obj, Color color)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = color;
        }
    }


    void UpdateUI(JengaBlockObject block)
    {
        _gameController.UIOverlay.SetUITextFromBlock(block.JengaBlock);
    }




}
