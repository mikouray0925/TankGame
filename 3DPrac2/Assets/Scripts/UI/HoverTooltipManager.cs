using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


//reference: https://www.youtube.com/watch?v=FVqtmTWd8Zk
public class HoverTooltipManager : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI tipText;
    [SerializeField] public RectTransform tipWindow;
    public static FastAction<string,Vector2> OnHoverEnter;
    public static FastAction OnHoverExit;
    // Start is called before the first frame update
    public void Show(){
        gameObject.SetActive(true);
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        OnHoverEnter.Add(ShowTooltip);
        OnHoverExit.Add(HideTooltip);

    }
    public void Hide(){
        OnHoverEnter.Remove(ShowTooltip);
        OnHoverExit.Remove(HideTooltip);
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
    void Start(){
        tipText.text = "";
        tipWindow.gameObject.SetActive(false);
    }

    private void ShowTooltip(string tip, Vector2 mousePos){
        tipText.text = tip;
        tipWindow.gameObject.SetActive(true);
        tipWindow.sizeDelta = new Vector2(tipText.preferredWidth > 200 ? 200: tipText.preferredWidth, tipText.preferredHeight);
        tipWindow.transform.position = new Vector2(mousePos.x +tipWindow.sizeDelta.x * 2 , mousePos.y );
    }

    private void HideTooltip(){
        tipText.text = "";
        tipWindow.gameObject.SetActive(false);
    }
}
