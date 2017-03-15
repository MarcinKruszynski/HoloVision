using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemGlobalKeywords : MonoBehaviour, ISpeechHandler
{
    private Vector3[] cachedChildPositions;
    private Quaternion[] cachedChildRotations;
    private Renderer[] childRenderers;

    private void Awake()
    {
        childRenderers = GetComponentsInChildren<Renderer>();
        if (childRenderers != null && childRenderers.Length > 0)
        {
            cachedChildPositions = new Vector3[childRenderers.Length];
            cachedChildRotations = new Quaternion[childRenderers.Length];

            for (int i = 0; i < childRenderers.Length; i++)
            {
                cachedChildPositions[i] = childRenderers[i].transform.localPosition;
                cachedChildRotations[i] = childRenderers[i].transform.rotation;
            }
        }
    }

    public void OnSpeechKeywordRecognized(SpeechKeywordRecognizedEventData eventData)
    {
        switch (eventData.RecognizedText.ToLower())
        {
            case "reset all":
                if (childRenderers != null && childRenderers.Length > 0 &&
                    cachedChildPositions != null && cachedChildPositions.Length == childRenderers.Length &&
                    cachedChildRotations != null && cachedChildRotations.Length == childRenderers.Length)
                {
                    for (int i = 0; i < childRenderers.Length; i++)
                    {
                        var rigidbody = childRenderers[i].GetComponent<Rigidbody>();
                        if (rigidbody != null)
                        {
                            DestroyImmediate(rigidbody);
                        }

                        childRenderers[i].transform.localPosition = cachedChildPositions[i];
                        childRenderers[i].transform.rotation = cachedChildRotations[i];
                    }
                }
                break;
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < childRenderers.Length; i++)
        {
            DestroyImmediate(childRenderers[i]);
        }
    }
}
