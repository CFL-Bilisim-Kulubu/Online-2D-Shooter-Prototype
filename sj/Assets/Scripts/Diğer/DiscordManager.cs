using Discord;
using UnityEngine;

public class DiscordManager : MonoBehaviour
{
    [SerializeField] private string info, state;
    private Discord.Discord discord;
    private ActivityManager activityManager;

    private void Start()
    {
        discord = new Discord.Discord(864101586900877322, (ulong)CreateFlags.Default);
        activityManager = discord.GetActivityManager();
        UpdatePresence(info, state);
    }

    private void Update() => discord.RunCallbacks();

    private void UpdatePresence(string info, string state)
    {
        activityManager.UpdateActivity(new Activity
        {
            Details = info,
            State = state,
            Assets = {
                LargeImage = "gunpo",
                LargeText = "SJ oyunun baþ kahramaný gunpo."
            }
        }, res => { });
    }
}