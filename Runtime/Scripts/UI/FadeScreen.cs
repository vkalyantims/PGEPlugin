using System.Collections;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{

    public float fadeDuration =10f;
    public Color fadeColor;
    private Renderer rend;


    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update() 
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
           FadeOut();
        }
    }

    public void FadeOut()
    {
        if(rend == null)
        {
            rend = GetComponent<Renderer>();
        }
        Fade(0,1);
    }

    public void Fade(float alphaIn, float alphaOut)
    {
        StartCoroutine(FadeCoroutine(alphaIn, alphaOut));
    }
    public IEnumerator FadeCoroutine(float alphaIn, float alphaOut)
    {
        float timer = 0;

        while(timer<fadeDuration)
        {
            Color newColor = fadeColor;
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer/fadeDuration);
            rend.material.SetColor("_Color", newColor);

            timer += Time.deltaTime;
            yield return null;
        }
        Color newColor2 = fadeColor;
        newColor2.a = Mathf.Lerp(alphaIn, alphaOut, timer/fadeDuration);
        rend.material.SetColor("_Color", newColor2);

    }
}
