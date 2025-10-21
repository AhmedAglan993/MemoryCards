using System.Collections.Generic;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class Stories
{
    public string access_token;
    public List<JsonObject> levels;
    public bool success, app_on_store;
    public int score;
    public string user_id;
    public string primary_domain;
    public string token;
    public string message;
    public int total_coins;
    public int coins;
    public int level_coins;
    public List<Codes> codes;
}

[System.Serializable]
public struct JsonObject
{
    public int level, score, stars, coins;
}
[System.Serializable]
public struct Codes
{
    public string code_id, expired_at;
    public int trips, max_users, count, coins, percentage;
}
