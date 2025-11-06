using UnityEngine;
using TMPro;


public class HPCar : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public TMP_Text counterText; // Ссылка на UI текст
    public Canvas endGame;
    public TMP_Text endText;
    private int HP = 10;

    void Start()
    {
        counterText.text = $"HP: {HP}/10";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Enemy" && other.GetComponent<CarBOOM>().alive)
        {
            HP -= 1;
            other.GetComponent<CarBOOM>().alive = false;
            UpdateCounter();
            if (HP <= 0)
            {
                endGame.gameObject.SetActive(true);
                endText.text = "Вы проиграли! Нажмите LeftShift для выхода в меню";
                Time.timeScale = 0f;
            }
        }
    }


    private void UpdateCounter()
    {
        counterText.text = $"HP: {HP}/10";
    }
}
