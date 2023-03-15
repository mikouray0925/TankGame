using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skybox : MonoBehaviour
{

    [SerializeField] private Material skyboxMaterial;
    
    [SerializeField] private Shader skyboxShader;
    [Range(1, 10)] public int timePerTick = 5;
    private Renderer _renderer;
    private Skybox skybox;
    private int ticktime;
    private int MaxTickTime;

    // Start is called before the first frame update
    void Start()
    {
        
        ticktime = 0;
        MaxTickTime = timePerTick * 720;
        skyboxShader = Shader.Find("Skybox/Cubemap Blend");
        if(skyboxShader == null) {
            Debug.Log("Skybox Shader not found");
        }
        else {
            Debug.Log("Skybox Shader found");
        }
        if(skyboxMaterial == null) {
            Debug.Log("Skybox Material not found");
            skyboxMaterial = new Material(skyboxShader);
            RenderSettings.skybox = skyboxMaterial;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() {
        ticktime++;
        MaxTickTime = timePerTick * 720;
        if (ticktime > MaxTickTime) {
            ticktime = 0;
        }
        if(ticktime%timePerTick==0) UpdateMaterialVariables(ticktime / timePerTick);
    }

    void UpdateMaterialVariables(int tt) {
        //daytime = 0 ~ 360
            //sunrise = 0 ~ 90 
                //Trasiition 1 ~ 0.8
            if(tt <= 90) {
                skyboxMaterial.SetFloat("_CubemapTransition", 1.0f - (tt / 450f));
            }
            //day = 90 ~ 270
                //Trasiition 0.8 ~ 0 ~ 0.8
            else if(tt <= 270) {
                skyboxMaterial.SetFloat("_CubemapTransition", (Mathf.Abs((tt - 180f) / 112.5f)));
            }
            //sunset = 270 ~ 360
                //Trasiition 0.8 ~ 1
            else if(tt <= 360) {
                skyboxMaterial.SetFloat("_CubemapTransition", 0.2f + (tt / 450f));
            }
        //nighttime = 360 ~ 720
            //moonrise = 360 ~ 450
                //Exposure 0.9 ~ 0.45
            else if(tt <= 450) {
                skyboxMaterial.SetFloat("_Exposure", 2.7f - (tt / 200f));
            }
            //night = 450 ~ 630
                //Exposure 0.45 ~ 0 ~ 0.45
            else if(tt <= 630) {
                skyboxMaterial.SetFloat("_Exposure", (Mathf.Abs((tt - 540f) / 200f)));
            }
            //moonset = 630 ~ 720
                //Exposure 0.45 ~ 0.9
            else if(tt <= 720) {
                skyboxMaterial.SetFloat("_Exposure", 0.45f + ((tt - 630f) / 200f));
            }
    }

}
