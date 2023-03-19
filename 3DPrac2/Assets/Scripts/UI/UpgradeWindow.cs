using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UpgradeWindow : MonoBehaviour
{
    // Start is called before the first frame update

    private Button HealthUpgrade;
    private Button DamageUpgrade;
    private Button CooldownUpgrade;
    private GameObject Player;
    private GameObject Tank;

    private int healthUpgradeCount = 0;
    private int damageUpgradeCount = 0;
    private int cooldownUpgradeCount = 0;
    public void Show(){
        gameObject.SetActive(true);
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Player = GameObject.Find("Player");
        Tank = GameObject.Find("Tank");

        HealthUpgrade = GameObject.Find("Life Upgrade").GetComponent<Button>();;
        if(HealthUpgrade != null){
            Debug.Log("Found Health Upgrade");
        }
        DamageUpgrade = GameObject.Find("Gear Upgrade").GetComponent<Button>();;
        if(DamageUpgrade != null){
            Debug.Log("Found Damage Upgrade");
        }
        CooldownUpgrade = GameObject.Find("Cooling Upgrade").GetComponent<Button>();
        if(CooldownUpgrade != null){
            Debug.Log("Found Cooldown Upgrade");
        }

        HealthUpgrade.onClick.AddListener(delegate{
            healthUpgradeCount++;
            Tank.GetComponent<TankHealth>().ScaleHealth((healthUpgradeCount*0.2f + 1f)  * 20);
            Tank.GetComponent<TankHealth>().Heal();
            Hide();
        });

        DamageUpgrade.onClick.AddListener(delegate{
            damageUpgradeCount++;
            Player.GetComponent<TurretController>().damageMultiplier = damageUpgradeCount*0.1f + 1f;
            Hide();
        });

        CooldownUpgrade.onClick.AddListener(delegate{
            cooldownUpgradeCount++;
            Player.GetComponent<TurretController>().fireCDMultiplier = 1f - cooldownUpgradeCount*0.05f;
            Hide();
        });
    }

    public void Hide(){
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
