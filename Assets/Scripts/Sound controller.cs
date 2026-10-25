using UnityEngine;

public class Soundcontroller : MonoBehaviour
{
    public static Soundcontroller Instance;

    public AudioSource button_click_source;
    public AudioSource handel_source;
    public AudioSource start_spin_source;
    public AudioSource win_source;
    public AudioSource loose_source;
    public AudioSource number_stop_source;

    public AudioClip button_click_clip;
    public AudioClip handel_clip;
    public AudioClip start_spin_clip;
    public AudioClip win_clip;
    public AudioClip loose_clip;
    public AudioClip number_stop_clip;

    private void Awake()
    {
        Instance = this;
    }

    public void button_click()
    {
        button_click_source.PlayOneShot(button_click_clip);
    }

    public void handel_sound()
    {
        handel_source.PlayOneShot(handel_clip);
    }

    public void start_spin_sound()
    {
        start_spin_source.PlayOneShot(start_spin_clip);
    }

    public void stop_spin_sound()
    {
        start_spin_source.Stop();
    }

    public void win_sound()
    {
        win_source.PlayOneShot(win_clip);
    }

    public void loose_sound()
    {
        loose_source.PlayOneShot(loose_clip);
    }

    public void number_stop_sound()
    {
        number_stop_source.PlayOneShot(number_stop_clip);
    }
}