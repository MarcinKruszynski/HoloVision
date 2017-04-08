using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class ThrowBall : MonoBehaviour, IInputClickHandler
{
    public float ForceMagnitude = 300f;    

    public void OnInputClicked(InputEventData eventData)
    {
        Throw();
    }

    private void Throw()
    {
        var ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        ball.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        var rigidBody = ball.AddComponent<Rigidbody>();
        rigidBody.mass = 0.5f;
        rigidBody.position = transform.position;

        var transformForward = transform.forward;
        transformForward = Quaternion.AngleAxis(-10, transform.right) * transformForward;

        rigidBody.AddForce(transformForward * ForceMagnitude);
    }
}
