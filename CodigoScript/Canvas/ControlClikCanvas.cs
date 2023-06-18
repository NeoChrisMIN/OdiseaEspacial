using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControlClikCanvas : MonoBehaviour//, IPointerClickHandler //, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform Joystick;
    public float valorX;
    public float valorY;

    public EventSystem eventSystem;


   



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (eventSystem != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = eventSystem.currentInputModule.input.mousePosition;

                //Debug.Log("Posición del puntero del mouse: " + mousePosition);
                Joystick.anchoredPosition = mousePosition;

                ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, new PointerEventData(eventSystem), ExecuteEvents.pointerClickHandler);

            }
        }
    }

}
