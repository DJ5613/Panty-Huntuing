using UnityEngine;
using LogitechG29.Sample.Input;
// Не нужно using RCC

public class G29Input : MonoBehaviour
{
    public RCC_CarControllerV3 car; // RCC машина
    public InputControllerReader inputControllerReader; // Ссылка на компонент G29

    void Update()
    {
        if (car == null || inputControllerReader == null)
            return;

        // Руль — управление поворотом
        car.steerInput = inputControllerReader.Steering;

        // Педали
        car.throttleInput = inputControllerReader.Throttle;
        car.brakeInput = inputControllerReader.Brake;
        car.handbrakeInput = inputControllerReader.Handbrake;

        // Коробка передач
        if (inputControllerReader.NorthButton)
            car.GearShiftUp();
        if (inputControllerReader.SouthButton)
            car.GearShiftDown();
    }
}
