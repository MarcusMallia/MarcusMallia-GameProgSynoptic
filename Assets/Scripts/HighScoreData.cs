[System.Serializable]
public class HighScoreData
{
    public float bestTime;

    public float BestTime
    {
        get => bestTime;
        private set => bestTime = value;
    }

    public HighScoreData(float time)
    {
        BestTime = time;
    }
}
