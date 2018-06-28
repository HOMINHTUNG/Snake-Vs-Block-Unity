using Assets.Blockz_vs_Ballz.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using UnityEditor;
using System.IO;
using System.Collections;

public class ShopController : MonoBehaviour
{
    private List<SkinObject> listSkin = new List<SkinObject>();

    private int PageCount;
    public Image mImageSkin;
    public Text mNameSkin;
    public Text mCostSkin;
    public Image mNextSkin;
    public Image mPreSkin;
    public Text mYourCoin;

    public CanvasGroup MessageDialog;
    public Text TextMess;

    public static Sprite HeadSkinEquid;
    public static Sprite BodySkinEquid;
    public static Boolean ChangeSkin = false;
    public static int MONEY;
    public static String SKIN;

    public AudioSource MusicBackgroundOut;
    public AudioClip SoundBackgroundOut;

    public AudioSource MusicButton;
    public AudioClip SoundButton;

    public AudioSource MusicButtonEquid;
    public AudioClip SoundButtonEquid;

    public AudioSource MusicButtonBuy;
    public AudioClip SoundButtonBuy;

    public TextAsset FileSKIN;
    public Image PreSkin
    {
        get { return mPreSkin; }
        set { mPreSkin = value; }
    }
    public Image NextSkin
    {
        get { return mNextSkin; }
        set { mNextSkin = value; }
    }
    public Text CostSkin
    {
        get { return mCostSkin; }
        set { mCostSkin = value; }
    }
    public Text NameSkin
    {
        get { return mNameSkin; }
        set { mNameSkin = value; }
    }
    public Image ImageSkin
    {
        get { return mImageSkin; }
        set { mImageSkin = value; }
    }


    // Use this for initialization
    void Start()
    {
        MONEY = PlayerPrefs.GetInt("MONEY");
        print("MONEY SHOP: " + MONEY);
        //SKIN = ReadStringSkin();
        SKIN = PlayerPrefs.GetString("SKIN");
        print("SKIN SHOP 13-6: " + SKIN);
        initSkin();

        PageCount = 0;
        SlideSkin();
        updateShop();
        updateUI(PageCount);
        

        MusicButtonEquid.clip = SoundButtonEquid;
        MusicButtonBuy.clip = SoundButtonBuy;
        MusicButton.clip = SoundButton;
        MusicBackgroundOut.clip = SoundBackgroundOut;
        MusicBackgroundOut.Play();

        DisableCG(MessageDialog);
    }

    // Update is called once per frame
    void Update()
    {
        updateShop();
        updateUI(PageCount);
        
    }

    public void ExitShop()
    {
        DisableCG(MessageDialog);

        MusicButton.PlayOneShot(SoundButton);
        SceneManager.LoadScene("Main");
    }

    public void SetView()
    {

    }

    public void initSkin()
    {

        listSkin.Add(new SkinObject("Ugly Duck", 0, "duck_img", "duck_head", "duck_body", 0, 0));
        listSkin.Add(new SkinObject("Christmas Snake", 50, "noel_img", "noel_head", "noel_body", 0, 0));
        listSkin.Add(new SkinObject("Minion", 100, "minion_img", "minion_head", "minion_body", 0, 0));
        listSkin.Add(new SkinObject("Monkey D.Luffy", 1000, "luffy_img", "luffy_head", "luffy_body", 0, 0));
        listSkin.Add(new SkinObject("Chang’e", 4000, "luna_img", "luna_head", "luna_body", 0, 0));
        listSkin.Add(new SkinObject("Pikachu", 6000, "pikachu_img", "pikachu_head", "pikachu_body", 0, 0));

    }

    public void NextSkinShop()
    {
        MusicButton.PlayOneShot(SoundButton);
        print("Page " + PageCount);
        if (PageCount == listSkin.Count - 1)
        {
            // NextSkin.enabled = false;
        }
        else
        {
            //    PreSkin.enabled = true;
            PageCount++;
        }
        SlideSkin();
    }

    public void PreSkinShop()
    {
        MusicButton.PlayOneShot(SoundButton);
        if (PageCount > 0)
        {
            //    NextSkin.enabled = true;
            PageCount--;
        }
        else
        {
            PageCount = 0;
            //      PreSkin.enabled = false;
        }
        print("Page " + PageCount);
        SlideSkin();
    }

    private void SlideSkin()
    {
        try
        {
            updateShop();
            updateUI(PageCount);
        }
        catch (Exception e)
        {
            print(e.Message.ToString());
        }


        if (PageCount == 0)
        {
            PreSkin.enabled = false;
            NextSkin.enabled = true;
        }

        if (PageCount == listSkin.Count - 1)
        {
            NextSkin.enabled = false;
            PreSkin.enabled = true;
        }
        if (PageCount > 0 && PageCount < listSkin.Count - 1)
        {
            NextSkin.enabled = true;
            PreSkin.enabled = true;
        }
        DisableCG(MessageDialog);
    }

    public void OnClickCostSkin()
    {
        MusicButton.PlayOneShot(SoundButton);
        print("SKIN BEFORE: " + SKIN);
        String mSkin = "";
        if (CostSkin.text.Equals("EQUID"))
        {
            MusicButtonEquid.Play();
            int indexSkinOld = indexEquidSkinOld();

            listSkin[indexSkinOld].StatusEquid = 0;
            listSkin[PageCount].StatusEquid = 1;

            mSkin = PageCount.ToString() + " " + listSkin[PageCount].StatusBuy + " " + listSkin[PageCount].StatusEquid;
            print("STATUS SKIN NEW: " + mSkin);
            checkMySkin(mSkin);

            String mSkinOld = indexSkinOld.ToString() + " " + listSkin[indexSkinOld].StatusBuy + " " + listSkin[indexSkinOld].StatusEquid;
            print("STATUS SKIN OLD: " + mSkinOld);
            checkMySkin(mSkinOld);
            PlayerPrefs.SetString("SKIN", SKIN);
            updateShop();
            updateUI(PageCount);

            ChangeSkin = true;
            BodySkinEquid = Resources.Load<Sprite>(listSkin[PageCount].BodySkin);
            HeadSkinEquid = Resources.Load<Sprite>(listSkin[PageCount].HeadSkin);
        }

        else if (!CostSkin.text.Equals("UNEQUID") && !CostSkin.text.Equals("EQUID"))
        {
            if (MONEY >= listSkin[PageCount].Cost)
            {
                MusicButtonBuy.Play();
                listSkin[PageCount].StatusBuy = 1;
                listSkin[PageCount].StatusEquid = 0;
                MONEY -= listSkin[PageCount].Cost;
                print("MONEY When BUY: " + MONEY);

                mSkin = PageCount.ToString() + " " + listSkin[PageCount].StatusBuy + " " + listSkin[PageCount].StatusEquid;
                print("SKIN When BUY: " + mSkin);
                checkMySkin(mSkin);
                PlayerPrefs.SetString("SKIN", SKIN);
                updateShop();
                updateUI(PageCount);

                print("Mua Skin thành công!");
                TextMess.text = "Purchase Success!";
                EnableCG(MessageDialog);
            }
            else
            {
                print("Không đủ tiền mua!");
                TextMess.text = "Not Enough Money!";
                EnableCG(MessageDialog);
            }
        }
        print("SKIN AFTER: " + SKIN);

        PlayerPrefs.SetInt("MONEY", MONEY);
    }

    public void checkMySkin(String mSKIN)
    {
        String[] Skin = mSKIN.Split(' ');
        Boolean checkExit = false;

        if (SKIN.Contains("-"))
        {
            print("SKIN: " + "Nhieu");
            String[] listMySkin = SKIN.Split('-');
            for (int i = 0; i < listMySkin.Length; i++)
            {
                print("SKIN PLIT[" + i + "]: " + listMySkin[i]);
                print("listMySkin[i].Substring(0, 1): " + listMySkin[i].Substring(0, 1));
                if (listMySkin[i].Substring(0, 1).Equals(Skin[0]))
                {
                    checkExit = true;
                    string tempSkin = SKIN;

                    print("SKIN -1 : " + listMySkin[i]);
                    print("SKIN -2 : " + mSKIN);
                    SKIN = SKIN.Replace(listMySkin[i], mSKIN);
                    print("SKIN WHEN REPLACE: " + SKIN);
                }
            }
            if (!checkExit)
            {
                print("checkExit: " + false);
                SKIN += "-" + mSKIN;
            }
            print("SKIN NEW REPLACE: " + SKIN);
        }
        else
        {
            print("SKIN: " + "Mot");
            print("SKIN.Substring(0, 1): " + SKIN.Substring(0, 1));
            if (SKIN.Substring(0, 1).Equals(Skin[0]))
            {
                print("SKIN -1 : " + SKIN);
                print("SKIN -2 : " + mSKIN);
                SKIN = SKIN.Replace(SKIN, mSKIN);
                print("SKIN WHEN REPLACE: " + SKIN);
            }
            else
            {
                SKIN += "-" + mSKIN;
            }
        }
        
    }

    public int indexEquidSkinOld()
    {
        for (int i = 0; i < listSkin.Count; i++)
        {
            if (listSkin[i].StatusEquid == 1)
            {
                return i;
            }
        }
        return 0;
    }

    public void updateShop()
    {
        String mSkin = PlayerPrefs.GetString("SKIN");
        mYourCoin.text = "Your coin: " + PlayerPrefs.GetInt("MONEY").ToString() + "$";
        if (mSkin.Contains("-"))
        {
            String[] listMySkin = SKIN.Split('-');
            for (int i = 0; i < listMySkin.Length; i++)
            {
                updateListSkin(listMySkin[i]);
            }
        }
        else
        {
            updateListSkin(SKIN);
        }
    }

    public void updateUI(int index)
    {
        ImageSkin.sprite = Resources.Load<Sprite>(listSkin[index].ImageSkin);
        NameSkin.text = listSkin[index].NameSkin;

        if (listSkin[index].StatusBuy == 0)
        {
            CostSkin.text = listSkin[index].Cost.ToString() + "&";
        }
        else
        {
            if (listSkin[index].StatusEquid == 0)
            {
                CostSkin.text = "EQUID";
            }
            else
            {
                CostSkin.text = "UNEQUID";
            }
        }
    }

    public void updateListSkin(String mSKIN)
    {
        String[] Skin = mSKIN.Split(' ');

        int IndexSkin = int.Parse(Skin[0]);
        listSkin[IndexSkin].StatusBuy = int.Parse(Skin[1]);
        listSkin[IndexSkin].StatusEquid = int.Parse(Skin[2]);
    }
    public void OnClickButtonMess()
    {
        DisableCG(MessageDialog);
    }
    //public void ShowRewardedAd()
    //{
    //    if (Advertisement.IsReady("rewardedVideo"))
    //    {
    //        var options = new ShowOptions { resultCallback = HandleShowResult };
    //        Advertisement.Show("rewardedVideo", options);
    //    }
    //}
    //private void HandleShowResult(ShowResult result)
    //{
    //    switch (result)
    //    {
    //        case ShowResult.Finished:
    //            Debug.Log("The ad was successfully shown.");
    //            MONEY += 20;
    //            PlayerPrefs.SetInt("MONEY", MONEY);
    //            RewardPanel.SetActive(true);
    //            SceneManager.LoadScene(0);
    //            break;
    //        case ShowResult.Skipped:
    //            Debug.Log("The ad was skipped before reaching the end.");
    //            break;
    //        case ShowResult.Failed:
    //            Debug.LogError("The ad failed to be shown.");
    //            break;
    //    }
    //}



    //// 400x400 px window will apear in the center of the screen.
    //private Rect windowRect = new Rect((Screen.width / 2) - 260,
    //                            (Screen.height / 2) + 220,
    //                            350,
    //                            200);

    //public void OnGUI()
    //{
    //    if (showBuy != 0)
    //    {
    //        var centeredStyle = GUI.skin.GetStyle("Label");
    //        centeredStyle.fontSize = 20;
    //        windowRect = GUI.Window(0, windowRect, DialogWindow, "");
    //    }
    //}
    ////Application.Quit();
    ////        showBuy = 0;
    //// This is the actual window.
    //public void DialogWindow(int windowID)
    //{
    //    GUI.color = Color.green;
    //    var centeredStyle = GUI.skin.GetStyle("Label");
    //    centeredStyle.fontSize = 30;
    //    switch (showBuy)
    //    {
    //        case 1:
    //            GUI.Label(new Rect(30, 40, windowRect.width, windowRect.height), "Mua Skin thành công!", centeredStyle);
    //            if (GUI.Button(new Rect(150, 100, windowRect.width, windowRect.height), "OK", centeredStyle))
    //            {
    //                showBuy = 0;
    //            }
    //            break;
    //        case 2:
    //            GUI.Label(new Rect(30, 40, windowRect.width, windowRect.height), "Không đủ Coin!", centeredStyle);
    //            if (GUI.Button(new Rect(150, 100, windowRect.width, windowRect.height), "OK", centeredStyle))
    //            {
    //                showBuy = 0;
    //            }
    //            break;
    //    }

    //}

    public void EnableCG(CanvasGroup cg)
    {
        cg.alpha = 1;
        cg.blocksRaycasts = true;
        cg.interactable = true;
    }

    public void DisableCG(CanvasGroup cg)
    {
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }


    //public void WriteStringSkin(string mSkin)
    //{
        
    //    System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/Skin");
    //    String path = Application.persistentDataPath + "/Skin" + "/SKIN.txt";
    //    try
    //    {
    //        System.IO.File.WriteAllText(path, mSkin);
    //    }
    //    catch (FileNotFoundException e)
    //    {
    //        Debug.Log("WriteStringSkin: "+ e);
    //        System.IO.File.Create(path);
    //        WriteStringSkin("0 1 1");
    //        System.IO.File.WriteAllText(path, mSkin);
    //    }
    //    catch (IOException ex)
    //    {
            
    //        DisableCG(MessageDialog);
    //        Debug.Log("WriteStringSkin: " + ex);
    //        WriteStringSkin("0 1 1");
    //        SceneManager.LoadScene("Shop");
    //    }
        
    //}
    
    //public String ReadStringSkin()
    //{
    //    //string path = @"Assets\Blockz vs Ballz\Resources\SKIN.txt";
    //    //string Skin = "";
    //    ////Read the text from directly from the test.txt file
    //    //StreamReader reader = new StreamReader(path);
    //    //Skin = FileSKIN.text;
    //    //reader.Close();
    //    System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/Skin");
    //    String path = Application.persistentDataPath + "/Skin" + "/SKIN.txt";
    //    String readFile = "";
    //    try
    //    {
    //        readFile = System.IO.File.ReadAllText(path);
    //    }
    //    catch (FileNotFoundException e)
    //    {
    //        Debug.Log("ReadStringSkin: " + e);
    //        System.IO.File.Create(path);
    //        readFile = System.IO.File.ReadAllText(path);
    //    }
    //    catch (IOException ex)
    //    {

    //        DisableCG(MessageDialog);
    //        Debug.Log("WriteStringSkin: " + ex);
    //        WriteStringSkin("0 1 1");
    //        SceneManager.LoadScene("Shop");
    //    }
    //    Debug.Log("ReadFile: " + readFile);
    //     return readFile;
    //    //var ReadFile = Resources.Load("SKIN") as TextAsset;
    //}
}
