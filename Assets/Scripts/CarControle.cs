using LogitechG29.Sample.Input;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Input Controller")]
    public InputControllerReader inputControllerReader;

    [Header("Car Physics Settings")]
    public float maxSteeringAngle = 30f;
    public float steeringSpeed = 2f;
    public float maxMotorTorque = 1000f;     // ������������ �������� ���������
    public float maxBrakeTorque = 2000f;     // ������������ ���� ����������
    public float dragCoefficient = 0.3f;     // ������������� �������
    public float rollingResistance = 0.1f;   // ������������� �������
    public float idleEngineBrake = 0.2f;     // ���������� ���������� ��� ���������� ����

    [Header("Wheel References")]
    public Transform frontLeftWheel;
    public Transform frontRightWheel;
    public Transform rearLeftWheel;
    public Transform rearRightWheel;

    private float currentSteeringAngle;
    private Rigidbody carRigidbody;
    private float currentSpeed;
    private float currentMotorTorque;
    private float currentBrakeTorque;

    void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();

        // ��������� Rigidbody ��� ����� ������������� ���������
        if (carRigidbody != null)
        {
            carRigidbody.centerOfMass = new Vector3(0f, -0.5f, 0f); // �������� ����� ���� ��� ������������
        }

        if (inputControllerReader != null)
        {
            inputControllerReader.SetDebugMode(true);
        }
        else
        {
            Debug.LogError("InputControllerReader �� �������� � ����������!");
        }


        // НАСТРОЙКИ ДЛЯ СТАБИЛЬНОСТИ
        carRigidbody.centerOfMass = new Vector3(0f, -0.5f, 0f); // Низкий центр масс
        carRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        carRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        // Ограничение вращения для предотвращения опрокидывания
        carRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void FixedUpdate()
    {
        HandlePhysics();
        HandleSteering();
        UpdateWheelVisuals();
    }

    void HandlePhysics()
    {
        if (inputControllerReader == null) return;

        // �������� �������� �������
        float throttle = inputControllerReader.Throttle;  // ��� (0 �� 1)
        float brake = inputControllerReader.Brake;        // ������ (0 �� 1)

        // ������������ ������� ��������
        currentSpeed = carRigidbody.linearVelocity.magnitude;

        // �����: ������������ �������� ������
        currentMotorTorque = throttle * maxMotorTorque;

        // ����������: �������� ���������� �������
        currentBrakeTorque = brake * maxBrakeTorque;

        // ���������� ����������: ���� ��� ������� � �� ����� ������
        if (throttle <= 0.1f && brake <= 0.1f && currentSpeed > 0.1f)
        {
            float engineBrakeForce = idleEngineBrake * maxMotorTorque;
            currentBrakeTorque += engineBrakeForce;
        }

        // ���������� ���
        ApplyForces();
        ApplyResistance();
    }

    void ApplyForces()
    {
        // ��������� ������������ ���� ������ �����
        if (currentMotorTorque > 0)
        {
            Vector3 engineForce = transform.forward * currentMotorTorque * Time.fixedDeltaTime;
            carRigidbody.AddForce(engineForce, ForceMode.Force);
        }

        // ��������� ��������� ����
        if (currentBrakeTorque > 0 && currentSpeed > 0.1f)
        {
            // �������� �������������� �������� ����������� ��������
            Vector3 brakeDirection = -carRigidbody.linearVelocity.normalized;
            Vector3 brakeForce = brakeDirection * currentBrakeTorque * Time.fixedDeltaTime;

            // ������������ ����������, ����� �� ����� �����
            if (Vector3.Dot(brakeForce, carRigidbody.linearVelocity) < 0)
            {
                carRigidbody.AddForce(brakeForce, ForceMode.Force);
            }
        }
    }

    void ApplyResistance()
    {
        // ������������� ������� (��������������� �������� ��������)
        float airDrag = dragCoefficient * currentSpeed * currentSpeed;
        Vector3 dragForce = -carRigidbody.linearVelocity.normalized * airDrag * Time.fixedDeltaTime;
        carRigidbody.AddForce(dragForce, ForceMode.Force);

        // ������������� ������� (���������� ��� ��������)
        if (currentSpeed > 0.1f)
        {
            float rollingDrag = rollingResistance * maxMotorTorque;
            Vector3 rollingForce = -carRigidbody.linearVelocity.normalized * rollingDrag * Time.fixedDeltaTime;
            carRigidbody.AddForce(rollingForce, ForceMode.Force);
        }

        // �������������� ��������� ��� ����� ����� ��������
        if (currentSpeed < 0.5f && inputControllerReader.Throttle < 0.1f)
        {
            carRigidbody.linearVelocity *= 0.9f; // ������� ���������
        }
    }

    void HandleSteering()
    {
        if (inputControllerReader == null) return;

        float steeringInput = inputControllerReader.Steering;

        // ��������� �������� ��� �������� (�� ������� �������� ������� ������)
        float speedFactor = Mathf.Clamp01(1f - currentSpeed / 20f);
        float effectiveSteeringAngle = steeringInput * maxSteeringAngle * speedFactor;

        currentSteeringAngle = Mathf.Lerp(
            currentSteeringAngle,
            effectiveSteeringAngle,
            Time.fixedDeltaTime * steeringSpeed
        );

        // ��������� ������� ����� ������
        if (currentSpeed > 0.1f)
        {
            float turnFactor = currentSpeed * 0.5f; // ����������� �������� � ����������� �� ��������
            carRigidbody.MoveRotation(carRigidbody.rotation *
                Quaternion.Euler(0f, currentSteeringAngle * turnFactor * Time.fixedDeltaTime, 0f));
        }
    }

    void UpdateWheelVisuals()
    {
        // ПОВОРОТ ПЕРЕДНИХ КОЛЁС
        if (frontLeftWheel != null)
            frontLeftWheel.localRotation = Quaternion.Euler(GetWheelRotation(frontLeftWheel), currentSteeringAngle, 0f);

        if (frontRightWheel != null)
            frontRightWheel.localRotation = Quaternion.Euler(GetWheelRotation(frontRightWheel), currentSteeringAngle, 0f);

        // ЗАДНИЕ КОЛЁСА - только вращение
        if (rearLeftWheel != null)
            rearLeftWheel.localRotation = Quaternion.Euler(GetWheelRotation(rearLeftWheel), 0f, 0f);

        if (rearRightWheel != null)
            rearRightWheel.localRotation = Quaternion.Euler(GetWheelRotation(rearRightWheel), 0f, 0f);
    }
    private float GetWheelRotation(Transform wheel)
    {
        // Вращение колёс на основе пройденного расстояния
        float distance = currentSpeed * Time.time * 360f;
        return distance % 360f;
    }
    void RotateWheel(Transform wheel, float rotation)
    {
        if (wheel != null)
        {
            wheel.Rotate(rotation, 0f, 0f);
        }
    }

    void Update()
    {
        // ���������� ����������
        if (Time.frameCount % 30 == 0) // ������� ��� � 30 ������ ����� �� �������� �������
        {
            Debug.Log($"��������: {currentSpeed:F1} ��/�, " +
                     $"���: {inputControllerReader.Throttle:F2}, " +
                     $"������: {inputControllerReader.Brake:F2}, " +
                     $"����: {inputControllerReader.Steering:F2}");
        }
    }

    // ��������������� �������� ��� UI
    public float GetCurrentSpeed()
    {
        return currentSpeed * 3.6f; // ����������� � ��/�
    }

    public float GetMotorPower()
    {
        return currentMotorTorque / maxMotorTorque;
    }

    public float GetBrakePower()
    {
        return currentBrakeTorque / maxBrakeTorque;
    }
}