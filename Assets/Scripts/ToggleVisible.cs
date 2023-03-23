using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleVisible : MonoBehaviour
{
    private float startAlpha;
    private float currentAlpha;
    private float alphaTarget;

    private bool visibleBool;

    private IEnumerator FadeCoroutine;
    public Toggle healthyLegShowToggle;
   	private Renderer MeshRenderer;
    GameObject thisone;

    // Use this for initialization
    void Start()
    {
        //Starting alpha
        startAlpha = 0.0f;
        alphaTarget = 1.0f;
        currentAlpha = 0;
        visibleBool = false;
		
		MeshRenderer = GetComponent<Renderer>();
        MeshRenderer.material.color = new Color(1, 1, 1, 0);
        MeshRenderer.enabled = false;
        //thisone = this.gameObject;
        //thisone.SetActive(false);

        healthyLegShowToggle.onValueChanged.AddListener(delegate {
            ToggleVisBool();
        });
    }

    // Update is called once per frame
    void Update()
    {




    }


    public void ToggleVisBool(){

        visibleBool = healthyLegShowToggle.isOn;
		
		CallFadeFunc(visibleBool);

    }
	
	
	
	
	
	public void CallFadeFunc(bool FadeIn) {
        
	
		if (FadeCoroutine != null) {
            StopCoroutine(FadeCoroutine);
        }

        //pass parameters here
        FadeCoroutine = FadeFunction(1.0f, FadeIn);

        StartCoroutine(FadeCoroutine);
    }

    IEnumerator FadeFunction(float timer, bool FadeIn)
    {
        //thisone.SetActive(true);
        //MeshRenderer.enabled = !MeshRenderer.enabled;

        if(FadeIn == true) {
            MeshRenderer.enabled = true;
        }

        Color StartColor = MeshRenderer.material.color;
        Color EndColor = (FadeIn) ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0);
        float t = 0.0f;

        while (t <= timer)
        {
          

            MeshRenderer.material.color = Color.Lerp(StartColor, EndColor, t / timer);

            t += Time.deltaTime;
            yield return null;
        }

        MeshRenderer.material.color = EndColor;

        if (FadeIn == false)
        {
            MeshRenderer.enabled = false;
        }


        FadeCoroutine = null;
    }
	




}
