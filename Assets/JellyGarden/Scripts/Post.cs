using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;
public class Post : MonoBehaviour
{
    public WWW a;
    public static Post THIS;
    string access_token;
    public static float startTime;
    float endTime;
    // Use this for initialization
    void Awake()
    {
        THIS = this;
    }
    public WWW GET(string url)
    {
        WWW www = new WWW(url);
        StartCoroutine(WaitForRequest(www));
        return www;
    }
    public WWW POST(string url, Dictionary<string, string> post)
    {
        WWWForm form = new WWWForm();
        foreach (KeyValuePair<string, string> post_arg in post)
        {
            form.AddField(post_arg.Key, post_arg.Value);
        }
        WWW www = new WWW(url, form);
        StartCoroutine(WaitForRequest(www));
        return www;
    }
    private IEnumerator WaitForRequest(WWW www)
    {
        yield return www;
        // check for errors
        if (www.error == null)
        {
            Stories myObject = new Stories();
            myObject = JsonUtility.FromJson<Stories>(www.text);
            Debug.Log("WWW Ok!: " + www.text);
            Debug.Log("loginparameters: " + myObject);
            //PlayerPrefs.DeleteAll();
            PlayerPrefs.SetString("Response", www.text);
            Debug.Log(PlayerPrefs.GetString("Response"));
            access_token = myObject.access_token;
            int score = myObject.score;
            string token = myObject.token;
            string primary_domain = myObject.primary_domain;
            string user_id = myObject.user_id;
            Debug.Log("Success: " + myObject.success);
            Debug.Log("Token: " + myObject.token);
          
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
        while (!www.isDone)
        {

        }
    }
}