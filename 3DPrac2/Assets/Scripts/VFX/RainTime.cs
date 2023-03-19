using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainTime : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField] private GameObject Water;
    private GameObject water;
    private ParticleSystem rain = null;
    void Start()
    {
        water = Instantiate(Water,new Vector3(130,0.0004f,130),Quaternion.identity);
        water.transform.localScale = new Vector3(3f,3f,3f);
        HideWater();

        int destroytime = Random.Range(11, 11);

        //enable water after 3s
        Invoke("showWater", 3f);
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

    }

    void SmallerRain(){
        //lower emittion rate
        StartCoroutine(AudioFadeOut());
        var Emission = rain.emission;
        Emission.rateOverTime = 100;
    }

    void PauseRain(){
        rain.Stop();
        Invoke("DestoryWater", 4.5f);
    }

    IEnumerator AudioFadeOut(){
        //fade out rain sound effect
        AudioSource soundEffect = GetComponent<AudioSource>();
        while (soundEffect.volume > 0) {
            soundEffect.volume -= Time.deltaTime / 4.5f;
            yield return null;
        }
    }
    void showWater(){
        water.SetActive(true);
    }
    void HideWater(){
        water.SetActive(false);
        
    }
    void DestoryWater(){
        Destroy(water);
    }

}
