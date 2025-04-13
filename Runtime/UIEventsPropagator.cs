using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace JohaToolkit.UnityEngine.UI
{
    public class UIEventPropagator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IDropHandler,
        IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, ISelectHandler, IDeselectHandler,
        IScrollHandler, IMoveHandler, IUpdateSelectedHandler, IInitializePotentialDragHandler, IBeginDragHandler,
        IEndDragHandler, ISubmitHandler, ICancelHandler
    {
        [Flags]
        private enum UIEventType
        {
            PointerEnter = 1,
            PointerExit = 2,
            Drag = 4,
            Drop = 8,
            PointerDown = 16,
            PointerUp = 32,
            PointerClick = 64,
            Select = 128,
            Deselect = 256,
            Scroll = 512,
            Move = 1024,
            UpdateSelected = 2048,
            InitializePotentialDrag = 4096,
            BeginDrag = 8192,
            EndDrag = 16384,
            Submit = 32768,
            Cancel = 65536
        }
        
        [Header("WARNING: Interrupts Standard event propagation!")]
        [Header("INFO: Propagation doesn't jumps over empty gameObjects!")]
        
        [SerializeField] private UIEventType propagate;
        [SerializeField, Tooltip("Normally, Events are caught and are not propagated. (e.g. 'OnSubmit' on Buttons) Enable 'OnSubmit' here to propagate the event anyways")] private UIEventType propagateAlways;
        
        private bool IsUIEventTypeSelected(UIEventType uiEventType) => (uiEventType & propagate) == uiEventType;

        private bool IsUIEventTypeMarkedAsPropagateAlways(UIEventType uiEventType) => (uiEventType & propagateAlways) == uiEventType;
        
        private void Execute<T>(UIEventType eventType, Action<T> action)
        {
            if (!IsUIEventTypeSelected(eventType))
                return;
            transform.parent.TryGetComponent(out T parentHandler);
            // if this object Recieves the event, we don't want to propagate the event
            if(IsUIEventTypeMarkedAsPropagateAlways(eventType) || !IsInterceptedByThisObjects<T>())
                action?.Invoke(parentHandler);
        }

        private bool IsInterceptedByThisObjects<T>()
        {
            return GetComponents<T>().Length >= 2;
        }

        public void OnPointerEnter(PointerEventData eventData) => Execute<IPointerEnterHandler>(UIEventType.PointerEnter, parentHandler => parentHandler.OnPointerEnter(eventData));
        
        public void OnPointerExit(PointerEventData eventData) => Execute<IPointerExitHandler>(UIEventType.PointerExit, parentHandler => parentHandler.OnPointerExit(eventData));
        
        public void OnDrag(PointerEventData eventData) => Execute<IDragHandler>(UIEventType.Drag, parentHandler => parentHandler.OnDrag(eventData));
        
        public void OnDrop(PointerEventData eventData) => Execute<IDropHandler>(UIEventType.Drop, parentHandler => parentHandler.OnDrop(eventData));
        
        public void OnPointerDown(PointerEventData eventData) => Execute<IPointerDownHandler>(UIEventType.PointerDown, parentHandler => parentHandler.OnPointerDown(eventData));
        
        public void OnPointerUp(PointerEventData eventData) => Execute<IPointerUpHandler>(UIEventType.PointerUp, parentHandler => parentHandler.OnPointerUp(eventData));
        
        public void OnPointerClick(PointerEventData eventData) => Execute<IPointerClickHandler>(UIEventType.PointerClick, parentHandler => parentHandler.OnPointerClick(eventData));
        
        public void OnSelect(BaseEventData eventData) => Execute<ISelectHandler>(UIEventType.Select, parentHandler => parentHandler.OnSelect(eventData));
        
        public void OnDeselect(BaseEventData eventData) => Execute<IDeselectHandler>(UIEventType.Deselect, parentHandler => parentHandler.OnDeselect(eventData));
        
        public void OnScroll(PointerEventData eventData) => Execute<IScrollHandler>(UIEventType.Scroll, parentHandler => parentHandler.OnScroll(eventData));
        
        public void OnMove(AxisEventData eventData) => Execute<IMoveHandler>(UIEventType.Move, parentHandler => parentHandler.OnMove(eventData));
        
        public void OnUpdateSelected(BaseEventData eventData) => Execute<IUpdateSelectedHandler>(UIEventType.UpdateSelected, parentHandler => parentHandler.OnUpdateSelected(eventData));
        
        public void OnInitializePotentialDrag(PointerEventData eventData) => Execute<IInitializePotentialDragHandler>(UIEventType.InitializePotentialDrag, parentHandler => parentHandler.OnInitializePotentialDrag(eventData));
        
        public void OnBeginDrag(PointerEventData eventData) => Execute<IBeginDragHandler>(UIEventType.BeginDrag, parentHandler => parentHandler.OnBeginDrag(eventData));
        
        public void OnEndDrag(PointerEventData eventData) => Execute<IEndDragHandler>(UIEventType.EndDrag, parentHandler => parentHandler.OnEndDrag(eventData));
        
        public void OnSubmit(BaseEventData eventData) => Execute<ISubmitHandler>(UIEventType.Submit, parentHandler => parentHandler.OnSubmit(eventData));
        
        public void OnCancel(BaseEventData eventData) => Execute<ICancelHandler>(UIEventType.Cancel, parentHandler => parentHandler.OnCancel(eventData));
    }
}
