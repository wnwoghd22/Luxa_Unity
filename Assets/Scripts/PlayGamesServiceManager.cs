using UnityEngine;
using System;
using System.Collections.Generic;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

/// <summary>
/// thanks for c0mpi1er
/// </summary>
public class PlayGamesServiceManager : MonoBehaviour
{
    public int playerScore;
    string leaderboardID = "";
    string achievementID = "CgkI7trZuckPEAIQAA";

    private Dictionary<string, string> achievementIDs = new Dictionary<string, string>
    {
        { "First Step", GPGSIds.achievement_first_step },
        { "Perfect Solution", GPGSIds.achievement_perfect_solution },
        { "Complete Level 1", GPGSIds.achievement_complete_level_1 },
        { "Conquer Level 1", GPGSIds.achievement_conquer_level_1 },
        { "Complete Level 2", GPGSIds.achievement_complete_level_2 },
        { "Conquer Level 2", GPGSIds.achievement_conquer_level_2 },
    };

    public static PlayGamesPlatform platform;

    void Start()
    {
        if (platform == null)
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = true;
            platform = PlayGamesPlatform.Activate();
        }

        Social.Active.localUser.Authenticate(success =>
        {
            if (success)
            {
                Debug.Log("Logged in successfully");
            }
            else
            {
                Debug.Log("Login Failed");
            }
        });
    }

    public void AddScoreToLeaderboard()
    {
        if (Social.Active.localUser.authenticated)
        {
            Social.ReportScore(playerScore, leaderboardID, success => { });
        }
    }

    public void ShowLeaderboard()
    {
        if (Social.Active.localUser.authenticated)
        {
            platform.ShowLeaderboardUI();
        }
    }

    public void ShowAchievements()
    {
        if (Social.Active.localUser.authenticated)
        {
            platform.ShowAchievementsUI();
        }
    }

    //for test
    public void UnlockAchievement()
    {
        if (Social.Active.localUser.authenticated)
        {
            Social.ReportProgress(achievementID, 100f, success => { });
        }
    }

    public void UnlockAchievement(string key)
    {
        if (Social.Active.localUser.authenticated)
        {
            Social.ReportProgress(achievementIDs[key], 100f, success => { });
        }
    }
}