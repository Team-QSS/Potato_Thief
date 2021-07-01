using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.TerrainAPI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using KJG;

public class JoystickControl : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private Image _joystickBackgroundImage;
    private Image _joystickImage;
    private Vector2 _inputVector;
    
    // Start is called before the first frame update
    void Start()
    {
        _joystickBackgroundImage = GetComponent<Image>();
        _joystickImage = transform.GetChild(0).GetComponent<Image>();
    }

    public void OnDrag(PointerEventData ped)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_joystickBackgroundImage.rectTransform,
            ped.position, ped.pressEventCamera, out pos))
        {
            pos.x = (pos.x / _joystickBackgroundImage.rectTransform.sizeDelta.x);
            pos.y = (pos.y / _joystickBackgroundImage.rectTransform.sizeDelta.y);
            
            _inputVector = new Vector2(pos.x * 2, pos.y * 2 );
            _inputVector = (_inputVector.magnitude > 1.0f) ? _inputVector.normalized : _inputVector;
            
            _joystickImage.rectTransform.anchoredPosition = new Vector2(_inputVector.x * (_joystickBackgroundImage.rectTransform.sizeDelta.x / 3), 
                                                                    _inputVector.y * (_joystickBackgroundImage.rectTransform.sizeDelta.y / 3));
            Debug.Log(_inputVector.x);
        }
    }

    public void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _inputVector = Vector2.zero;
        _joystickImage.rectTransform.anchoredPosition = Vector2.zero;
    }

    public float GetHorizontalValue()
    {
        return _inputVector.x;
    }
    
    public float GetVerticalValue()
    {
        return _inputVector.y;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
