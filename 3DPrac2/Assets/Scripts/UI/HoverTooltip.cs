using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class HoverTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string message = "This is a tooltip";
    private float delay = 0.5f;
    // Start is called before the first frame update
    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ShowMessageAfterDelay(delay));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        HoverTooltipManager.OnHoverExit.Call();
    }

    private void ShowMessage(){
        HoverTooltipManager.OnHoverEnter.Call(message,Input.mousePosition);
    }

    private IEnumerator ShowMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowMessage();
    }
}
