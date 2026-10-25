using UnityEngine;
using TMPro;


public class Amountscript : MonoBehaviour
{
    
    public TMP_Text amount;
    public int initial_amount;
    public int temp;
    public WinLoosepanelcontroller winLoosepanelcontroller;

    // ---------- Result codes used by amoun_update(int) ----------
    public const int LoseCode = 0;      // one coin is taken away
    public const int BarCode = 1;       // +1
    public const int BellCode = 2;      // +2
    public const int CherryCode = 3;    // +3
    public const int SevenCode = 4;     // +4

    private void Start()
    {
        ResetBalance();
    }

   public void amoun_update(int x)
    {
        if (!IsValidCode(x)) return;

        temp += GetBalanceChange(x);

        if (IsWinCode(x))
        {
            winLoosepanelcontroller.show_win_pannel();
        }

        if (temp <= 0)
        {
            winLoosepanelcontroller.show_loose_pannel();
        }

        UpdateDisplay();
    }

    public void restart_game()
    {
        ResetBalance();
        winLoosepanelcontroller.hide_loose_pannel();
        Soundcontroller.Instance.button_click();
    }

    private void ResetBalance()
    {
        temp = initial_amount;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        amount.text = temp.ToString();
    }

    private static bool IsValidCode(int code)
    {
        return code >= LoseCode && code <= SevenCode;
    }

    private static bool IsWinCode(int code)
    {
        return code >= BarCode && code <= SevenCode;
    }

    private static int GetBalanceChange(int code)
    {
        return code == LoseCode ? -1 : code;
    }
}