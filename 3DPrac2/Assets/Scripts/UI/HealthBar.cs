using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Health health;
    [SerializeField] Image fill;
    [SerializeField] Gradient gradient;
    Slider slider;

    void Awake() {
        slider = GetComponent<Slider>();
    }

    void LateUpdate() {
        slider.maxValue = health.maxHp;
        slider.value = health.hp;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
