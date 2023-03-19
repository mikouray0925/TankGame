using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;


public class HoverTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string message = "This is a tooltip";
    [SerializeField] public TextMeshProUGUI tipText;
    [SerializeField] public Image tipWindow;
    private RectTransform tipWindowRect;

    private float delay = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        tipText.text = "";
        tipWindow.gameObject.SetActive(false);
        tipWindowRect = tipWindow.GetComponent<RectTransform>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ShowMessageAfterDelay(delay));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        tipText.text = "";
        tipWindow.gameObject.SetActive(false);
    }

    private void ShowMessage(){
        Debug.Log("ShowMessage");
        ShowTooltip(message, Input.mousePosition);
    }

    private void ShowTooltip(string tip, Vector2 mousePos){
        tipWindow.gameObject.SetActive(true);
        tipText.text = tip;
        //tipWindowRect.sizeDelta = new Vector2(tipText.preferredWidth > 100 ? 100: tipText.preferredWidth, tipText.preferredHeight);
        //tipWindow.transform.position = new Vector2(mousePos.x , mousePos.y + 20);
    }

    private IEnumerator ShowMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowMessage();
    }
}
