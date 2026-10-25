using UnityEngine;
using System.Collections;

public class WinLoosepanelcontroller : MonoBehaviour
{
    public GameObject win_pannel;
    public GameObject loose_panel;
    public RollWheel rollWheel;


    private const float DelayBeforePanel = 1.3f;
    private const float WinPanelVisibleTime = 2f;

    public void hade_all()
    {
        win_pannel.SetActive(false);
        loose_panel.SetActive(false);
    }

    public void show_win_pannel()
    {
        StopAllCoroutines();
        StartCoroutine(ShowPanel(win_pannel));
    }

    public void show_loose_pannel()
    {
        rollWheel.LockHandle();
        StartCoroutine(ShowloosePanel());
    }

    public void hide_loose_pannel()
    {
        loose_panel.SetActive(false);
        rollWheel.UnlockHandle();
    }


    private IEnumerator ShowPanel(GameObject panel)
    {
        rollWheel.LockHandle();
        yield return new WaitForSeconds(DelayBeforePanel);
        Soundcontroller.Instance.win_sound();
        hade_all();
        panel.SetActive(true);

        yield return new WaitForSeconds(WinPanelVisibleTime);

        panel.SetActive(false);
        rollWheel.UnlockHandle();
    }

    private IEnumerator ShowloosePanel()
    {
        yield return new WaitForSeconds(DelayBeforePanel);
        Soundcontroller.Instance.loose_sound();
        loose_panel.SetActive(true);
    }
}