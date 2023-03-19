using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] bool alive;
    [SerializeField] float maxHealth;
    [SerializeField] float currentHealth;

    public float maxHp {
        get {
            return maxHealth;
        }
        private set {
            maxHealth = value;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        }
    }

    public float hp {
        get {
            return currentHealth;
        }
        private set {
            if (!alive) return;

            value = Mathf.Clamp(value, 0f, maxHealth);
            if (value > currentHealth) {
                OnHealthIncrease();
            }
            if (value < currentHealth) {
                OnHealthDecrease();
            }  

            currentHealth = value;
            if (currentHealth == 0) {
                alive = false;
                OnHealthBecomeZero();
            }
        }
    }

    public bool IsAlive() {
        return alive;
    }

    public void TakeDamage(float damageVal, Vector3 damageDir) {
        OnTakingDamage(damageVal, damageDir, out float finalDamageVal);
        hp -= finalDamageVal;
    }

    public void ScaleHealth(float multiplier) {
        maxHealth     *= multiplier;
        currentHealth *= multiplier;
    }
    public void Heal() {
        hp = maxHp;
    }

    protected virtual void OnHealthIncrease() {}
    protected virtual void OnHealthDecrease() {}
    protected virtual void OnHealthBecomeZero() {}
    protected virtual void OnTakingDamage(float damageVal, Vector3 damageDir, out float finalDamageVal) {
        finalDamageVal = damageVal;
    }
}
