using UnityEngine;
using UnityEngine.EventSystems;

// ScrollRect 위에 올려서 마우스 드래그는 막고 휠 스크롤은 통과시키는 컴포넌트
public class ScrollDragBlocker
    : MonoBehaviour,
        IInitializePotentialDragHandler,
        IBeginDragHandler,
        IDragHandler,
        IEndDragHandler
{
    public void OnInitializePotentialDrag(PointerEventData eventData) { }

    public void OnBeginDrag(PointerEventData eventData) { }

    public void OnDrag(PointerEventData eventData) { }

    public void OnEndDrag(PointerEventData eventData) { }
    // IScrollHandler 미구현 → 휠 이벤트는 부모(ScrollRect)로 버블업
}
