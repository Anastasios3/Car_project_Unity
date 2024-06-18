using UnityEngine;

public class CarController : MonoBehaviour
{
    public WheelCollider[] wheels;
    public Transform[] wheelMeshes;
    public float motorTorque = 150f;
    public float brakeTorque = 300f;
    public float maxSteerAngle = 30f;
    public float maxBrakeTorque = 500f;
    public Transform centerOfMass;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (centerOfMass != null)
        {
            rb.centerOfMass = centerOfMass.localPosition;
        }

        if (wheels.Length != wheelMeshes.Length)
        {
            Debug.LogError("Number of WheelColliders does not match number of WheelMeshes.");
        }
    }

    void Update()
    {
        HandleSteering();
        HandleMotor();
        UpdateWheelMeshes();
    }

    private void HandleSteering()
    {
        float steer = Input.GetAxis("Horizontal") * maxSteerAngle;
        wheels[0].steerAngle = steer;
        wheels[1].steerAngle = steer;
    }

    private void HandleMotor()
    {
        float motor = Input.GetAxis("Vertical") * motorTorque;
        float brake = Input.GetKey(KeyCode.Space) ? maxBrakeTorque : 0f;

        for (int i = 0; i < wheels.Length; i++)
        {
            if (i >= 2) // Rear wheels
            {
                wheels[i].motorTorque = motor;
            }
            wheels[i].brakeTorque = brake;
        }
    }

    private void UpdateWheelMeshes()
    {
        for (int i = 0; i < wheels.Length; i++)
        {
            UpdateWheelMesh(wheels[i], wheelMeshes[i]);
        }
    }

    private void UpdateWheelMesh(WheelCollider collider, Transform mesh)
    {
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);
        mesh.position = position;
        mesh.rotation = rotation;
    }
}
