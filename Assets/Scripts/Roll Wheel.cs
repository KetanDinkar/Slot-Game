using System.Collections;
using UnityEngine;

public class RollWheel : MonoBehaviour
{
    public Transform[] leftside;
    public Transform[] rightside;
    public Transform[] midside;

    
    public GameObject[] wheel_object;

    public float minSpinTime = 1.5f;
    public float maxSpinTime = 3.5f;
    public float shiftInterval = 0.08f;

    private bool isSpinning = false;

    public Amountscript amountscript;
    [HideInInspector] public bool stop_handel = false;

    public bool cheat = false;

    // ---------- Constants ----------
    private const int TopSlot = 0;
    private const int MiddleSlot = 1;
    private const int BottomSlot = 2;

    private const float SlowdownChance = 0.7f;  
    private const float MaxSlowdown = 0.4f;      

    private const string TagSeven = "7";
    private const string TagCherry = "cherry";
    private const string TagBell = "bell";
    private const string TagBar = "bar";

    private const int NoPayout = -1;


    public bool IsHandleLocked
    {
        get { return stop_handel; }
    }

    public void LockHandle()
    {
        stop_handel = true;
    }

    public void UnlockHandle()
    {
        stop_handel = false;
    }

    private void Start()
    {
        HideOriginalWheelObjects();

        InitializeSide(leftside);
        InitializeSide(midside);
        InitializeSide(rightside);

        LockHandle();
    }

    // ---------- Spinning ----------

    public void rotate_the_wheel()
    {
        if (isSpinning) return;
        StartCoroutine(SpinRoutine());
    }

    private IEnumerator SpinRoutine()
    {
        isSpinning = true;
        Soundcontroller.Instance.start_spin_sound();
        Transform[][] reels = { leftside, midside, rightside };
        ReelSpinTimer[] timers = CreateTimers(reels.Length);

        while (!AllDone(timers))
        {
            float dt = Time.deltaTime;

            for (int i = 0; i < reels.Length; i++)
            {
                if (timers[i].Advance(dt, shiftInterval))
                {
                    ShiftSide(reels[i]);
                }
            }

            yield return null;
        }

        isSpinning = false;
        Soundcontroller.Instance.stop_spin_sound();
        CheckWinCondition();
    }

    private ReelSpinTimer[] CreateTimers(int count)
    {
        float[] durations = new float[count];
        for (int i = 0; i < count; i++)
        {
            durations[i] = Random.Range(minSpinTime, maxSpinTime);
        }

        ReelSpinTimer[] timers = new ReelSpinTimer[count];
        for (int i = 0; i < count; i++)
        {
            float slowdown = Random.value < SlowdownChance ? Random.Range(0f, MaxSlowdown) : 0f;
            timers[i] = new ReelSpinTimer(durations[i], slowdown);
        }
        return timers;
    }

    private static bool AllDone(ReelSpinTimer[] timers)
    {
        foreach (ReelSpinTimer timer in timers)
        {
            if (!timer.IsDone) return false;
        }
        return true;
    }

    // ---------- Reel contents ----------

    private void HideOriginalWheelObjects()
    {
        foreach (GameObject obj in wheel_object)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }

    private void InitializeSide(Transform[] side)
    {
        for (int i = 0; i < side.Length; i++)
        {
            if (wheel_object.Length > 0 && side[i] != null)
            {
                ClearSlot(side[i]);
                SpawnRandomSymbol(side[i]);
            }
        }
    }

    private void ShiftSide(Transform[] side)
    {
        // Destroy bottom object
        if (side[BottomSlot].childCount > 0)
        {
            Destroy(side[BottomSlot].GetChild(0).gameObject);
        }

        // Middle goes to bottom, top goes to middle
        MoveSymbol(side[MiddleSlot], side[BottomSlot]);
        MoveSymbol(side[TopSlot], side[MiddleSlot]);

        // New random symbol at the top
        if (wheel_object.Length > 0)
        {
            SpawnRandomSymbol(side[TopSlot]);
        }
    }

    private void MoveSymbol(Transform from, Transform to)
    {
        if (from.childCount > 0)
        {
            Transform movingObj = from.GetChild(0);
            movingObj.SetParent(to);
            movingObj.position = to.position;
        }
    }

    private void ClearSlot(Transform slot)
    {
        foreach (Transform child in slot)
        {
            Destroy(child.gameObject);
        }
    }

    private void SpawnRandomSymbol(Transform slot)
    {
        int randIndex = Random.Range(0, wheel_object.Length);
        SpawnSymbol(wheel_object[randIndex], slot);
    }

    private void SpawnSymbol(GameObject symbol, Transform slot)
    {
        GameObject spawned = Instantiate(symbol, slot.position, Quaternion.identity, slot);
        spawned.SetActive(true);
    }

    private void SetMiddleSymbol(Transform[] side, GameObject symbol)
    {
        if (side[MiddleSlot].childCount > 0)
        {
            Destroy(side[MiddleSlot].GetChild(0).gameObject);
        }

        SpawnSymbol(symbol, side[MiddleSlot]);
    }

    private string GetMiddleTag(Transform[] side)
    {
        if (side[MiddleSlot].childCount > 0)
        {
            return side[MiddleSlot].GetChild(0).tag;
        }
        return "";
    }

    public void cheat_apply_or_not()
    {
        float r = Random.Range(0f, 1f);
        if (r <=0.13) { cheat = true; }
        else { cheat = false; }
    }
    // ---------- Result ----------

    private void CheckWinCondition()
    {
        string leftTag = GetMiddleTag(leftside);
        string midTag = GetMiddleTag(midside);
        string rightTag = GetMiddleTag(rightside);

        if (cheat)
        {
           
            GameObject luckySymbol = wheel_object[Random.Range(0, wheel_object.Length)];

            SetMiddleSymbol(leftside, luckySymbol);
            SetMiddleSymbol(midside, luckySymbol);
            SetMiddleSymbol(rightside, luckySymbol);

            leftTag = midTag = rightTag = luckySymbol.tag;
        }

        if (AllMatch(leftTag, midTag, rightTag))
        {
            HandleWin(leftTag);
        }
        else
        {
            HandleLoss(leftTag, midTag, rightTag);
        }

        cheat = false;
    }

    private static bool AllMatch(string leftTag, string midTag, string rightTag)
    {
        return leftTag == midTag && midTag == rightTag && !string.IsNullOrEmpty(leftTag);
    }

    private void HandleWin(string tag)
    {
        Debug.Log("YOU WIN! All middle tags match: " + tag);

        int payout = GetPayoutCode(tag);
        if (payout != NoPayout)
        {
            amountscript.amoun_update(payout);
        }
    }

    private void HandleLoss(string leftTag, string midTag, string rightTag)
    {
        Debug.Log("YOU LOSE! -> Left: " + leftTag +
                  " | Mid: " + midTag + " | Right: " + rightTag);

        UnlockHandle();
        amountscript.amoun_update(Amountscript.LoseCode);
    }

    private static int GetPayoutCode(string tag)
    {
        switch (tag)
        {
            case TagSeven: return Amountscript.SevenCode;
            case TagCherry: return Amountscript.CherryCode;
            case TagBell: return Amountscript.BellCode;
            case TagBar: return Amountscript.BarCode;
            default: return NoPayout;
        }
    }

    private class ReelSpinTimer
    {
        private readonly float duration;
        private readonly float slowdownPortion;
        private float elapsed;
        private float sinceLastShift;

        public bool IsDone { get; private set; }

        public ReelSpinTimer(float duration, float slowdownPortion)
        {
            this.duration = duration;
            this.slowdownPortion = slowdownPortion;
        }

        public bool Advance(float deltaTime, float baseInterval)
        {
            if (IsDone) return false;

            elapsed += deltaTime;
            sinceLastShift += deltaTime;

            float interval = GetShiftInterval(elapsed / duration, baseInterval);
            bool shouldShift = sinceLastShift >= interval && elapsed < duration;

            if (shouldShift)
            {
                sinceLastShift = 0f;
            }

            if (elapsed >= duration)
            {
                Soundcontroller.Instance.number_stop_sound();
                IsDone = true;
                
            }

            return shouldShift;
        }

        private float GetShiftInterval(float progress, float baseInterval)
        {
            if (slowdownPortion <= 0f || progress <= 1f - slowdownPortion)
                return baseInterval;

            float t = Mathf.InverseLerp(1f - slowdownPortion, 1f, progress);
            float slowdown = 1f - Mathf.SmoothStep(0f, 1f, t);

            return baseInterval / Mathf.Max(slowdown, 0.01f);
        }

        
    }
}