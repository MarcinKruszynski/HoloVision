using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Renderer))]
public class ItemKeywords : MonoBehaviour, ISpeechHandler
{
    public void OnShoot()
    {
        if (!this.GetComponent<Rigidbody>())
        {
            var rigidbody = this.gameObject.AddComponent<Rigidbody>();
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }
    }

    public void OnSpeechKeywordRecognized(SpeechKeywordRecognizedEventData eventData)
    {
        switch (eventData.RecognizedText.ToLower())
        {
            case "shoot down":
                OnShoot();
                break;
        }
    }
}
