using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Net;
using Intendix.Board;
using Unity.ItemRecever;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UnicornSpellerInterface : MonoBehaviour
{ 
    public static string ips = "127.0.0.1";
    public static int port = 1000;
    public static IPAddress ip = IPAddress.Parse(ips);

    public RawImage m_RawImage;
    //Select a Texture in the Inspector to change to
    public Texture[] m_Textures = new Texture[28];

    private int currentItem = 0;

    public string picture1="";
    public string picture2="";

    public static bool isBackPressed;
    public static bool isSelectPressed;
    public static bool isForwardPressed;

     public RawImage m_RawImage1;
     public RawImage m_RawImage2;
     public RawImage m_RawImage3;
    //Select a Texture in the Inspector to change to
    public Texture[] m_Textures2 = new Texture[24];
    public Camera cam1;
    public Camera cam2;


    
        protected Transform trackingTarget;

    void Update()
	{
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("RightArrow");
            NextButton();
        }
            if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Wololooo");
             SelectButton();
        }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("LeftArrow");
            BackButton();
        }

        if(isBackPressed) 
        {
            BackButton();
            m_RawImage.texture = m_Textures[currentItem];
            isBackPressed = false;
        }
        if(isSelectPressed)
        {
            SelectButton();
            m_RawImage.texture = m_Textures[currentItem];
            Debug.Log(m_RawImage.mainTexture);
            isSelectPressed = false;
        }
        if(isForwardPressed)
        {
            NextButton();
            m_RawImage.texture = m_Textures[currentItem];
           isForwardPressed = false;
        }
    }

    void Start()
    {
        connection(ip, port);
        m_RawImage.texture = m_Textures[currentItem];

        
        isForwardPressed = false;
        isSelectPressed = false;
    }

    public void NextButton()
    {
        currentItem++;
        if(currentItem > m_Textures.Length-1){
            currentItem = 0;
        }
        m_RawImage.texture = m_Textures[currentItem];
        
    }

        public void SelectButton()
    {
        m_RawImage.texture = m_Textures[currentItem];

        if( picture1 == "" && picture2 == ""){
        picture1 = m_Textures[currentItem].name;
        Debug.Log(picture1 + " " +  picture2 + "asfdghj");
        }

         if( picture2 == "" && picture1 != ""){
        picture2 = m_Textures[currentItem].name;
       Debug.Log(picture1 + " " +  picture2);

        cam1.enabled = false;
        cam2.enabled = true;
         }
            
         
        
    }


        public void BackButton()
    {
        currentItem--;
        if(currentItem < 0){
            currentItem = m_Textures.Length-1;
        }
        m_RawImage.texture = m_Textures[currentItem];
        
    }

    void connection(IPAddress ip, int port)
    {
        // Connection with Unicorn BCI
        try
        {
            //Start listening for Unicorn Speller network messages
            SpellerReceiver r = new SpellerReceiver(ip, port);

            //attach items received event
            r.OnItemReceived += OnItemReceived;

            Debug.Log(String.Format("Listening to {0} on port {1}.", ip, port));
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }

    }

    private void OnItemReceived(object sender, EventArgs args)
    {
        ItemReceivedEventArgs eventArgs = (ItemReceivedEventArgs)args;
        Debug.Log(String.Format("Received BoardItem:\tName: {0}\tOutput Text: {1}", eventArgs.BoardItem.Name, eventArgs.BoardItem.OutputText));

        if (eventArgs.BoardItem.OutputText == "Left")
        {
            isBackPressed = true;
            Debug.Log("Left");
            BackButton();
            m_RawImage.texture = m_Textures[currentItem];

            
        }
        if (eventArgs.BoardItem.OutputText == "Select")
        {
            isSelectPressed = true;
            Debug.Log("Select");
            SelectButton();
            m_RawImage.texture = m_Textures[currentItem];
            
        }
        if (eventArgs.BoardItem.OutputText == "Right")
        {
            isForwardPressed = true;
            Debug.Log("Right");
            NextButton();
            m_RawImage.texture = m_Textures[currentItem];

            
        }
    }   
}
        
    

