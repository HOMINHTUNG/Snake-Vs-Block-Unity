using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{

    [Header("Managers")]
    public GameController GC;

    [Header("Some Snake Variables & Storing")]
    public List<Transform> BodyParts = new List<Transform>();
    public float minDistance = 0.25f;
    public int initialAmount;
    public float speed = 1;
    public float rotationSpeed = 50;
    public float LerpTimeX;
    public float LerpTimeY;

    [Header("Snake Head Prefab")]
    public GameObject BodyPrefab;

    [Header("Snake Sprite")]
    public Sprite HeadSprite;
    public Sprite BodySprite;
    public String SKIN;


    [Header("Parts Text Amount Management")]
    public TextMesh PartsAmountTextMesh;

    [Header("Private Fields")]
    private float distance;
    private Vector3 refVelocity;

    private Transform curBodyPart;
    private Transform prevBodyPart;

    private bool firstPart;

    [Header("Mouse Control Variables")]
    Vector2 mousePreviousPos;
    Vector2 mouseCurrentPos;

    [Header("Particle System Management")]
    public ParticleSystem SnakeParticle;

    public AudioSource MusicDead;
    public AudioClip SoundDead;

    // Use this for initialization
    void Start()
    {
        if (ShopController.ChangeSkin)
        {
            HeadSprite = ShopController.HeadSkinEquid;
            BodySprite = ShopController.BodySkinEquid;
        }
        else
        {
            SKIN = PlayerPrefs.GetString("SKIN");
            if(SKIN == "" || SKIN == null)
            {
                SKIN = "0 1 1";
                PlayerPrefs.SetString("SKIN", SKIN);
            }
            switch (updateSkin(SKIN))
            {
                case 0:
                    BodySprite = Resources.Load<Sprite>("duck_body");
                    HeadSprite = Resources.Load<Sprite>("duck_head");
                    break;
                case 1:
                    BodySprite = Resources.Load<Sprite>("noel_body");
                    HeadSprite = Resources.Load<Sprite>("noel_head");
                    break;
                case 2:
                    BodySprite = Resources.Load<Sprite>("minion_body");
                    HeadSprite = Resources.Load<Sprite>("minion_head");
                    break;
                case 3:
                    BodySprite = Resources.Load<Sprite>("luffy_body");
                    HeadSprite = Resources.Load<Sprite>("luffy_head");
                    break;
                case 4:
                    BodySprite = Resources.Load<Sprite>("luna_body");
                    HeadSprite = Resources.Load<Sprite>("luna_head");
                    break;
                case 5:
                    BodySprite = Resources.Load<Sprite>("pikachu_body");
                    HeadSprite = Resources.Load<Sprite>("pikachu_head");
                    break;
            }

        }

        MusicDead.clip = SoundDead;

        //At the beginning, if it's the first part, spawn it at (0, -3, 0)
        firstPart = true;

        //Add the initial BodyParts
        for (int i = 0; i < initialAmount; i++)
        {
            Invoke("AddBodyPart", 0.1f);


            //Use invoke to avoid a weird bug where the snake goes down at the beginning.
        }


    }

    public void SpawnBodyParts()
    {
        firstPart = true;

        //Add the initial BodyParts
        for (int i = 0; i < initialAmount; i++)
        {
            //Use invoke to avoid a weird bug where the snake goes down at the beginning.
            Invoke("AddBodyPart", 0.1f);
        }
    }

    // Update is called once per frame
    void Update()
    {

        //We constantly move if the game is on
        if (GameController.gameState == GameController.GameState.GAME)
        {
            Move();

            //Check if no more snake parts
            if (BodyParts.Count == 0)
            {
                MusicDead.Play();
                GC.SetGameover();
            }

        }

        //Update the Parts Amount text
        if (PartsAmountTextMesh != null)
        {
            PartsAmountTextMesh.text = transform.childCount + "";
            Debug.Log("PartsAmountTextMesh.text: " + PartsAmountTextMesh.text);
        }



    }

    public void Move()
    {
        float curSpeed = speed;

        //Always move the body Up
        if (BodyParts.Count > 0)
            BodyParts[0].Translate(Vector2.up * curSpeed * Time.smoothDeltaTime);


        //check if we are still on screen
        float maxX = Camera.main.orthographicSize * Screen.width / Screen.height;

        if (BodyParts.Count > 0)
        {
            if (BodyParts[0].position.x > maxX) //Right pos
            {
                BodyParts[0].position = new Vector3(maxX - 0.01f, BodyParts[0].position.y, BodyParts[0].position.z);
            }
            else if (BodyParts[0].position.x < -maxX) //Left pos
            {
                BodyParts[0].position = new Vector3(-maxX + 0.01f, BodyParts[0].position.y, BodyParts[0].position.z);
            }
        }

        //Move the snake on the Horizontal Axis with mouse control
        if (Input.GetMouseButtonDown(0))
        {
            mousePreviousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0))
        {
            //check if he is still in screen

            if (BodyParts.Count > 0 && Mathf.Abs(BodyParts[0].position.x) < maxX)
            {
                mouseCurrentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                float deltaMousePos = Mathf.Abs(mousePreviousPos.x - mouseCurrentPos.x);
                float sign = Mathf.Sign(mousePreviousPos.x - mouseCurrentPos.x);


                //BodyParts[0].Translate(Vector3.left * rotationSpeed * Time.deltaTime * deltaMousePos * sign);
                BodyParts[0].GetComponent<Rigidbody2D>().AddForce
                    (Vector2.right * rotationSpeed * deltaMousePos * -sign);

                mousePreviousPos = mouseCurrentPos;
            }
            else if (BodyParts.Count > 0 && BodyParts[0].position.x > maxX) //Right pos
            {
                BodyParts[0].position = new Vector3(maxX - 0.01f, BodyParts[0].position.y, BodyParts[0].position.z);
            }
            else if (BodyParts.Count > 0 && BodyParts[0].position.x < maxX) //Left pos
            {
                BodyParts[0].position = new Vector3(-maxX + 0.01f, BodyParts[0].position.y, BodyParts[0].position.z);
            }
        }


        //Move the other body parts depending on the Head, that's why we start the loop at 1
        for (int i = 1; i < BodyParts.Count; i++)
        {
            curBodyPart = BodyParts[i];
            prevBodyPart = BodyParts[i - 1];

            distance = Vector3.Distance(prevBodyPart.position, curBodyPart.position);

            Vector3 newPos = prevBodyPart.position;

            newPos.z = BodyParts[0].position.z;

            //Try 2 Lerps, one on the x pos and one on the Y
            Vector3 pos = curBodyPart.position;

            pos.x = Mathf.Lerp(pos.x, newPos.x, LerpTimeX);
            pos.y = Mathf.Lerp(pos.y, newPos.y, LerpTimeY);

            curBodyPart.position = pos;


        }
    }

    public void AddBodyPart()
    {
        Transform newPart;

        if (firstPart)
        {
            newPart = (Instantiate(BodyPrefab, new Vector3(0, 0, 0),
                       Quaternion.identity) as GameObject).transform;
            newPart.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

            //Disable the collision with snake

            // Set this part as the parent of the Text Mesh
            PartsAmountTextMesh.transform.parent = newPart;

            //Place it correctly
            PartsAmountTextMesh.transform.position = newPart.position +
                new Vector3(0, 0.5f, 0);

            firstPart = false;
        }
        else
        {

            newPart = (Instantiate(BodyPrefab, BodyParts[BodyParts.Count - 1].position,
          BodyParts[BodyParts.Count - 1].rotation) as GameObject).transform;
            newPart.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

        }

        newPart.SetParent(transform);

        BodyParts.Add(newPart);
        BodyParts[0].GetComponent<SpriteRenderer>().sprite = HeadSprite;
        BodyParts[BodyParts.Count - 1].GetComponent<SpriteRenderer>().sprite = BodySprite;


    }
    public int updateSkin(String mSKIN)
    {
        int Index = 0;
        if (mSKIN.Contains("-"))
        {
            String[] Skin = mSKIN.Split('-');

            for (int i = 0; i < Skin.Length; i++)
            {
                String[] mSkin = Skin[i].Split(' ');
                if (mSkin[2].Equals("1"))
                {
                    Index = int.Parse(mSkin[0]);
                }
            }
        }
        
        return Index;
    }
    
}
