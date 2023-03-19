using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainTime : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField] private GameObject Water;
    private GameObject water;
    private ParticleSystem rain = null;
    private AudioSource soundEffect;
    [SerializeField] private Material waterMaterial;
    void Start()
    {
        Debug.Log("RainTime Start");
        soundEffect = GetComponent<AudioSource>();
        water = Instantiate(Water,new Vector3(130,0,130),Quaternion.identity);
        water.transform.localScale = new Vector3(3f,3f,3f);
        HideWater();

        int destroytime = Random.Range(30, 120);

        //enable water after 3s
        StartCoroutine("showWater");
        //Make rain smaller after destoy time
        rain = GetComponent<ParticleSystem>();
        Invoke("SmallerRain", destroytime);
        Invoke("PauseRain", destroytime + 4.5f);
        Invoke("HideWater", destroytime + 8f);

        Destroy(gameObject, destroytime + 10f);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale == 0) soundEffect.Pause();
        else soundEffect.UnPause();
    }

    void SmallerRain(){
        //lower emittion rate
        StartCoroutine(AudioFadeOut());
        StartCoroutine(WaterFadeOut());
        var Emission = rain.emission;
        Emission.rateOverTime = 100;
    }

    void PauseRain(){
        rain.Stop();
        Invoke("DestoryWater", 4.5f);
    }

    IEnumerator AudioFadeOut(){
        //fade out rain sound effect
        
        while (soundEffect.volume > 0) {
            soundEffect.volume -= Time.deltaTime / 4.5f;
            yield return null;
        }
    }
    IEnumerator showWater(){
        yield return new WaitForSeconds(3f);
        water.SetActive(true);
        water.transform.position = new Vector3(130,0.0004f,130);
        //move up 
        while (waterMaterial.GetFloat("_AlphaMultiplier") < 0.1f){
            waterMaterial.SetFloat("_AlphaMultiplier", waterMaterial.GetFloat("_AlphaMultiplier") + Time.deltaTime / 80f);
            yield return null;
        }
        
    }
    IEnumerator WaterFadeOut(){
        //move up 
        while (waterMaterial.GetFloat("_AlphaMultiplier") > 0f){
            waterMaterial.SetFloat("_AlphaMultiplier", waterMaterial.GetFloat("_AlphaMultiplier") - Time.deltaTime / 45f);
            yield return null;
        }
        
    }
    void HideWater(){
        water.SetActive(false);
        waterMaterial.SetFloat("_AlphaMultiplier", 0f);
    }
    void DestoryWater(){
        Destroy(water);
    }

}
