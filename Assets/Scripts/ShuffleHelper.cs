using System.Collections.Generic;
using UnityEngine;

public static class ShuffleUtility
{
    public static List<T> Shuffle<T>(in List<T> sourceList)
    {
        if (sourceList == null || sourceList.Count == 0)
            return new List<T>();

        List<T> shuffled = new List<T>(sourceList);

        // Fisher-Yates shuffle algorithm
        // https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
        for (int i = shuffled.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            T temp = shuffled[i];
            shuffled[i] = shuffled[randomIndex];
            shuffled[randomIndex] = temp;
        }

        return shuffled;
    }
    public static List<T> SpotifyShuffle<T>(in List<T> sourceList, T lastPlayed = default(T))
    {
        if (sourceList == null || sourceList.Count == 0)
            return new List<T>();

        List<T> shuffled = Shuffle<T>(sourceList);

        // Spotify-style trick: if the first item in new shuffle matches the last played
        if (lastPlayed != null && shuffled.Count > 1 && EqualityComparer<T>.Default.Equals(shuffled[0], lastPlayed))
        {
            // Find a different item to swap with
            for (int i = 1; i < shuffled.Count; i++)
            {
                if (!EqualityComparer<T>.Default.Equals(shuffled[i], lastPlayed))
                {
                    // Swap first element with this one
                    T temp = shuffled[0];
                    shuffled[0] = shuffled[i];
                    shuffled[i] = temp;
                    break;
                }
            }
        }

        return shuffled;
    }
}