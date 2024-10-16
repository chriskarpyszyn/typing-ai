public sealed class Score
{
    private static Score instance = null;
    private static readonly object padlock = new object();

    private float elapsedTime = 0f;
    

    Score()
    {

    }

    public static Score Instance
    {
        get
        {
            if (instance == null)
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Score();
                    }
                }
            }
            return instance;
        }
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    public string GetElapsedTimeString()
    {
        return Score.Instance.GetElapsedTime().ToString("F1");
    }

    public void IncreaseElapsedTime(float t)
    {
        elapsedTime += t;
    }

    public void SetElapsedTime(float t)
    {
        elapsedTime = t;
    }
}