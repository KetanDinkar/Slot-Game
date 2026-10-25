using UnityEngine;
using UnityEngine.UI;

public class Buttoncontroller : MonoBehaviour
{
    public GameObject Rule_pannel;

    public Image Instruction_pannel;
    public Sprite instruction_1;
    public Sprite instruction_2;

    public GameObject right_button;
    public GameObject left_button;

    public GameObject instruction_button;
    public RollWheel rollWheel;
    public WinLoosepanelcontroller winLoosepanelcontroller;

    public GameObject start_pannel;

    private void Start()
    {
        start_pannel.SetActive(true);
        instruction_button.SetActive(false);
        winLoosepanelcontroller.hade_all();
    }

    public void start_button_click()
    {
        start_pannel.SetActive(false);
        Rule_pannel.SetActive(true);
        ShowPage(instruction_1);
        left_button.SetActive(false);
        button_click_sound_play();
    }

    public void right_button_click()
    {
        ShowPage(instruction_2);
        SetNavigationButtons(leftVisible: true, rightVisible: false);
        button_click_sound_play();
    }

    public void left_button_click()
    {
        ShowPage(instruction_1);
        SetNavigationButtons(leftVisible: false, rightVisible: true);
        button_click_sound_play();
    }

    public void rule_pannel_close()
    {
        Rule_pannel.SetActive(false);
        instruction_button.SetActive(true);
        rollWheel.UnlockHandle();
        button_click_sound_play();
    }

    public void instruction_button_click()
    {
        Rule_pannel.SetActive(true);
        ShowPage(instruction_1);
        SetNavigationButtons(leftVisible: false, rightVisible: true);
        instruction_button.SetActive(false);
        button_click_sound_play();
    }

    public void ExitGame()
    {
        button_click_sound_play();
        Application.Quit();

    }


    private void ShowPage(Sprite page)
    {
        Instruction_pannel.sprite = page;
    }

    private void SetNavigationButtons(bool leftVisible, bool rightVisible)
    {
        left_button.SetActive(leftVisible);
        right_button.SetActive(rightVisible);
    }


    void button_click_sound_play()
    {
        Soundcontroller.Instance.button_click();
    }
}