using UnityEngine;
using LogitechG29;
using LogitechG29.Sample.Input;

public class G29Rule : MonoBehaviour
{
    [Header("RCC МАШИНА")]
    public RCC_CarControllerV3 carController;

    [Header("LOGITECH G29")]
    public InputControllerReader g29Controller;

    [Header("НАСТРОЙКИ РУЛЯ")]
    public float steeringSensitivity = 1.0f;
    public float steeringDeadZone = 0.1f;

    // Переменные для хранения текущих состояний
    private float currentSteering = 0f;

    void Start()
    {
        // Находим компоненты если не назначены
        if (carController == null)
            carController = GetComponent<RCC_CarControllerV3>();

        if (g29Controller == null)
            g29Controller = FindObjectOfType<InputControllerReader>();

        // Подписываемся только на необходимые события (без лепестков)
        SubscribeToG29Events();

    }

    void SubscribeToG29Events()
    {
        if (g29Controller == null) return;

    }

    void Update()
    {
        if (carController == null || g29Controller == null) return;

        // ОБРАБОТКА РУЛЯ
        HandleSteering();

        // ПРИМЕНЕНИЕ УПРАВЛЕНИЯ К RCC
        ApplyControlsToRCC();
    }

    void HandleSteering()
    {
        float rawSteering = g29Controller.Steering;

        if (Mathf.Abs(rawSteering) < steeringDeadZone)
            rawSteering = 0f;

        currentSteering = rawSteering * steeringSensitivity;
        currentSteering = Mathf.Clamp(currentSteering, -1f, 1f);
    }

    void ApplyControlsToRCC()
    {
        // РУЛЕВОЕ УПРАВЛЕНИЕ
        carController.steerInput = currentSteering;

    }
}