using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RoysPlayer
{
    public class PlayerCamera : MonoBehaviour, IDragHandler//, IPointerDownHandler, IPointerUpHandler,IBeginDragHandler
    {
        public Vector2 sens = Vector2.one * 0.2f;
        public float cameraRotHorizonLock = 90;
        public float cameraRot;
        public PlayerCameraTransform cameraPos;
        public Transform cameraBody;
        public Scrollbar sensBar;
        public Text sensBarText;
        public bool lockCamera;
        private void Start()
        {
            float _sens = 0.3f;
            if (PlayerPrefs.HasKey("sensivity"))
                _sens = PlayerPrefs.GetFloat("sensivity");
            SetSensivity(_sens);
        }
        public void SetSensivity(float s)
        {
            sens = new Vector2(s, s);
            sensBar.value = s;
            sensBarText.text = s.ToString().Substring(0, 3);
            PlayerPrefs.SetFloat("sensivity", s);
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (lockCamera)
                return;
            cameraBody.Rotate(new Vector3(0, eventData.delta.x * sens.y, 0));
            float r = Mathf.Abs(eventData.delta.y) * sens.x;
            if (eventData.delta.y < 0 && cameraRot < cameraRotHorizonLock - r)
            {
                cameraRot += r;
            }
            if (eventData.delta.y > 0 && cameraRot > -cameraRotHorizonLock + r)
            {
                cameraRot -= r;
            }
            cameraPos.x = cameraRot;
            //focusPos = eventData.position;
        }
        //bool isFocusingBlock;
        //[Tooltip("Size in pixels")] public float screenFocusDistance = 30;
        //[Tooltip("Time to interact with the unit (60 = 1 sec).")] public int placeTime = 10;
        //public Vector2 focusPos;
        //public Vector2 lastFocusPos;
        //int focusBlockTime;
        //void FixedUpdate()
        //{
        //    if (isFocusingBlock)
        //    {
        //        if (focusBlockTime < placeTime)
        //        {
        //            focusBlockTime++;
        //            if (Vector2.Distance(lastFocusPos, focusPos) > screenFocusDistance)
        //            {//FAIL DRAGGING
        //                lastFocusPos = focusPos;
        //                isFocusingBlock = false;
        //            }
        //        }
        //        else
        //        { //DRAG
        //            _OnDragEvent(focusPos);
        //            if (!isDragging)
        //            {
        //                isDragging = true;
        //                _OnStartDragEvent();
        //            }
        //        }
        //    }
        //}
        //public void OnPointerDown(PointerEventData eventData)
        //{
        //    lastFocusPos = focusPos  = eventData.position; 
        //    focusBlockTime = 0;
        //    isFocusingBlock  = true;
        //}
        //public void OnPointerUp(PointerEventData eventData)
        //{
        //    if (focusBlockTime < placeTime && isFocusingBlock)
        //    {//CLICK
        //        _OnClickEvent();
        //    }
        //    isFocusingBlock = false;
        //    _OnEndDragEvent();
        //    isDragging = false;
        //}

        //public void OnBeginDrag(PointerEventData eventData)
        //{
        //    _OnStartDragEvent();
        //}
        //bool isDragging;
        //public static CameraEvent _OnStartDragEvent;
        //public static CameraEvent _OnEndDragEvent;
        //public static MouseEvent _OnDragEvent;
        //public static CameraEvent _OnClickEvent;
        //public delegate void CameraEvent(); 
        //public delegate void MouseEvent(Vector2 mousePos);
    }
}