using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaSlice : MonoBehaviour
{
    float effectDuration = 0.5f;
    int numberOfRounds = 3;
    List<GameObject> slices = new List<GameObject>();
    Color[] originalColors;

    void Awake()
    {
        foreach (Transform slice in transform)
        {
            slices.Add(slice.gameObject);
        }

        GetSliceMaterials();
    }

    void GetSliceMaterials()
    {
        var sliceMaterials = slices[0].GetComponent<MeshRenderer>().materials;
        originalColors = new Color[slices[0].GetComponent<MeshRenderer>().materials.Length];

        originalColors = new Color[sliceMaterials.Length];
        for (int i = 0; i < sliceMaterials.Length; i++)
        {
            originalColors[i] = sliceMaterials[i].GetColor("_Color");
        }
    }

    public IEnumerator TakeASlice()
    {
        for (int round = 0; round < numberOfRounds; round++)
        {
            foreach (GameObject slice in slices)
            {
                HighlightSlice(slice);

                yield return new WaitForSeconds(effectDuration);

                ResetSliceColor(slice);
            }
        }
        StartCoroutine(DeactivateRandomSlice());
    }

    void HighlightSlice(GameObject slice)
    {
        var sliceMaterials = slice.GetComponent<MeshRenderer>().materials;

        foreach (var material in sliceMaterials)
        {
            material.SetFloat("_Metallic", 0.4f);
        }
    }

    IEnumerator HighlightSlice(GameObject slice, string hexColor, float duration)
    {
        var sliceMaterials = slice.GetComponent<MeshRenderer>().materials;

        Color startColor = sliceMaterials[0].GetColor("_Color");
        Color endColor;

        ColorUtility.TryParseHtmlString(hexColor, out endColor);

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            for (int i = 0; i < sliceMaterials.Length; i++)
            {
                sliceMaterials[i].SetColor("_Color", Color.Lerp(startColor, endColor, elapsedTime / duration));
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    void ResetSliceColor(GameObject slice)
    {
        var sliceMaterials = slice.GetComponent<MeshRenderer>().materials;

        foreach (var material in sliceMaterials)
        {
            material.SetFloat("_Metallic", 0f);
        }
    }

    void ResetSliceColor(GameObject slice, bool emptyVar)
    {
        var sliceMaterials = slice.GetComponent<MeshRenderer>().materials;

        for (int i = 0; i < sliceMaterials.Length; i++)
        {
            sliceMaterials[i].SetColor("_Color", originalColors[i]);
        }
    }

    IEnumerator DeactivateRandomSlice()
    {
        int randomIndex = Random.Range(0, transform.childCount);
        Transform randomSlice = transform.GetChild(randomIndex);

        yield return StartCoroutine(HighlightSlice(randomSlice.gameObject, "#BA0A0A", 5f));

        var renderer = randomSlice.GetComponent<Renderer>();
        renderer.enabled = false;

        yield return new WaitForSeconds(3f);

        ResetSliceColor(randomSlice.gameObject, true);
        renderer.enabled = true;
    }
}
