using HoloToolkit.Unity.InputModule;
using UnityEngine;


public class CubeCommands : MonoBehaviour, IInputClickHandler
{
    public void OnInputClicked(InputEventData eventData)
    {
        if (!this.GetComponent<Rigidbody>())
        {
            var rigidbody = gameObject.AddComponent<Rigidbody>();
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }
    }
}

