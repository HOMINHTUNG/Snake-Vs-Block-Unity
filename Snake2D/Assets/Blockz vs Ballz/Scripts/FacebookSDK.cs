using Facebook.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FacebookSDK : MonoBehaviour
{
    void Awake()
    {
        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
    }

    public void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    public void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }
    public void ShareCallback(IShareResult result)
    {
        if (result.Cancelled || !String.IsNullOrEmpty(result.Error))
        {
            Debug.Log("ShareLink Error: " + result.Error);
        }
        else if (!String.IsNullOrEmpty(result.PostId))
        {
            // Print post identifier of the shared content
            Debug.Log(result.PostId);
        }
        else
        {
            // Share succeeded without postID
            Debug.Log("ShareLink success!");
        }
    }
   
    
    public void LoginWithFB()
    {
        var perms = new List<string>() { "public_profile,email" };
        FB.LogInWithReadPermissions(perms, AuthCallback);


    }
    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            // AccessToken class will have session details
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            // Print current access token's User ID
            Debug.Log(aToken.UserId);
            // Print current access token's granted permissions
            foreach (string perm in aToken.Permissions)
            {
                Debug.Log(perm);
            }
        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }

    void Start()
    {
        FB.Init(OnInit);
    }

    private void OnInit()
    {
        LoginToFB();
    }

    public void LoginToFB()
    {
        FB.LogInWithPublishPermissions(new List<string>() { "publish_actions" }, LoginResult);
    }

    private void LoginResult(IResult result)
    {
        //TODO:do what you want
    }

    public IEnumerator ShareWithFB(String path)
    {
        
        WWW www = new WWW("file://" + "emulated/0/Android/data/com.hominhtung.snakevsblock/files/ScreenShot" + @"/DiedScreenShot.png");
        FB.ShareLink(
        contentURL: new Uri(www.url),
        contentTitle: "CHIA SẺ ĐIỂM",
        contentDescription: "Điểm bạn quá cao hahaha!",
        callback: ShareCallback);

        yield return null;
    }

    public void clickShareFB()
    {
        FB.ShareLink(
        contentURL: new Uri("https://ibb.co/dxnaOT"),
        contentTitle: "CHIA SẺ ĐIỂM",
        contentDescription: "Điểm bạn quá cao!",
        callback: ShareCallback);
    }

    private IEnumerator share3()
    {
        string path = "";
        yield return new WaitForEndOfFrame();

        Texture2D screenImage = new Texture2D(Screen.width, Screen.height);

        //Get Image from screen
        screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenImage.Apply();

        //Convert to png
        byte[] imageBytes = screenImage.EncodeToPNG();


        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/ScreenShot");
        path = Application.persistentDataPath + "/ScreenShot" + "/DiedScreenShot.png";
        System.IO.File.WriteAllBytes(path, imageBytes);



        StartCoroutine(ShareWithFB(path));
    }



    public IEnumerator ShareFB(String path)
    {
        WWW www = new WWW("file://" + path);
        FB.ShareLink(
        contentURL: new Uri(www.url),
        contentTitle: "CHIA SẺ ĐIỂM",
        contentDescription: "Điểm bạn quá cao hahaha!",
        callback: ShareCallback);

        yield return null;
    }
    private IEnumerator share2()
    {
        string path = "";
        yield return new WaitForEndOfFrame();

        Texture2D screenImage = new Texture2D(Screen.width, Screen.height);

        //Get Image from screen
        screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenImage.Apply();

        //Convert to png
        byte[] imageBytes = screenImage.EncodeToPNG();


        System.IO.Directory.CreateDirectory(Application.temporaryCachePath + "/ScreenShot");
        path = Application.temporaryCachePath + "/ScreenShot" + "/DiedScreenShot.png";
        System.IO.File.WriteAllBytes(path, imageBytes);


        StartCoroutine(ShareFB(path));
    }




    public void ShareFB3()
    {
        LoginWithFB();
        if (FB.IsLoggedIn)
        {
            StartCoroutine(share3());
        }
    }
    public void ShareFB2()
    {
        LoginWithFB();
        if (FB.IsLoggedIn)
        {
            StartCoroutine(share2());
        }
    }
    public void ShareFB1()
    {
      
            StartCoroutine(share1());
        
    }
    public IEnumerator ShareResultWithFB(String path)
    {
        string url = string.Format("file://{0}", path);
        WWW www = new WWW(url);
        yield return www;

        Debug.Log("www: "+ www.url);
        FB.ShareLink(
        contentURL: new Uri(www.url),
        contentTitle: "CHIA SẺ ĐIỂM",
        contentDescription: "Điểm bạn quá cao hahaha!",
        callback: ShareCallback);

        yield return null;
    }
    private IEnumerator share1()
    {
        string path = "";
        yield return new WaitForEndOfFrame();

        Texture2D screenImage = new Texture2D(Screen.width, Screen.height);
        
        //Get Image from screen
        screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenImage.Apply();

        //Convert to png
        byte[] imageBytes = screenImage.EncodeToPNG();

        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/ScreenShot");
        path = Application.persistentDataPath + "/ScreenShot" + "/DiedScreenShot.png";
        System.IO.File.WriteAllBytes(path, imageBytes);

        StartCoroutine(ShareResultWithFB(path));
    }

    public void takeScreenShotAndShare()
    {
        StartCoroutine(takeScreenshotAndSave());
    }

    private IEnumerator takeScreenshotAndSave()
    {
        string path = "";
        yield return new WaitForEndOfFrame();

        Texture2D screenImage = new Texture2D(Screen.width, Screen.height);

        //Get Image from screen
        screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenImage.Apply();

        //Convert to png
        byte[] imageBytes = screenImage.EncodeToPNG();


        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/GameOverScreenShot");
        path = Application.persistentDataPath + "/GameOverScreenShot" + "/DiedScreenShot.png";
        System.IO.File.WriteAllBytes(path, imageBytes);

        StartCoroutine(shareScreenshot(path));
    }

    private IEnumerator shareScreenshot(string destination)
    {
        string ShareSubject = "Picture Share";
        string shareLink = "Test Link";
        string textToShare = "Text To share";

        Debug.Log(destination);


        if (!Application.isEditor)
        {

            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://"+destination);
            Debug.Log("shareScreenshot : " + "file:/" + destination);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), textToShare + shareLink);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), ShareSubject);
            intentObject.Call<AndroidJavaObject>("setType", "image/*");
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
            currentActivity.Call("startActivity", intentObject);
        }
        yield return null;
    }

    private static void ShareImageWithTextOnAndroid(string message, string imageFilePath)
    {
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
        intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
        AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
        AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + imageFilePath);
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), message);

        intentObject.Call<AndroidJavaObject>("setType", "image/jpeg");
        AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

        currentActivity.Call("startActivity", intentObject);
    }

    private static void ShareImageAndroid(string imageFilePath)
    {
        AndroidJavaClass classPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject objActivity = classPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaClass classMedia = new AndroidJavaClass("android.media.MediaScannerConnection");
        classMedia.CallStatic("scanFile", new object[4] { objActivity,
        new string[] { imageFilePath },
        new string[] { "image/png" },
        null  });
    }


    public void Share()
    {
        LoginWithFB();
        if (FB.IsLoggedIn)
        {
            StartCoroutine(ShareImageShot());
        }
    }

    IEnumerator ShareImageShot()
    {
      
        yield return new WaitForEndOfFrame();
        Texture2D screenTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);

        screenTexture.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0);

        screenTexture.Apply();

        byte[] dataToSave = screenTexture.EncodeToPNG();

        string destination = Path.Combine(Application.persistentDataPath, "ScreenShotGame");

        File.WriteAllBytes(destination, dataToSave);

        var wwwForm = new WWWForm();
        wwwForm.AddBinaryData("image", dataToSave, "InteractiveConsole.png");
        
        FB.API("me/photos",HttpMethod.POST, Callback, wwwForm);

    }
    
    private string ApiQuery = "";
    void Callback(IResult result)
    {
        if (result.Error != null)
            Debug.Log("Error Response:\n" + result.Error);
        else if (!ApiQuery.Contains("/picture"))
            Debug.Log("Success Response:\n" + result.ToString());
        else
        {
            Debug.Log("Success Response:\n");
        }
    }
}
