using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using Cinemachine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public GameObject floor;
    public Transform playerSpawnPoint;
    public Sprite sprite;
    public GameObject speechBubble;
    public LayerMask layerInteraccion;

    [HideInInspector] public Vector2 gazeDirection = new Vector2(0, -1f);

    private Vector2 maxBound;
    private Vector2 minBound;
    private float objectWidth;
    private float objectHeight;

    private Rigidbody2D rigidBody;
    private Animator anim;
    private ContactFilter2D itemsFilter;


    private int walkHashCode;
    private int xHashCode;
    private int yHashCode;
    private int speed;

    private bool isSpeechBubbleShown;

    [HideInInspector] public float ejeHorizontal { get; private set; }
    [HideInInspector] public float ejeVertical { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = new Vector3(0, 0, 0);

        walkHashCode = Animator.StringToHash("Andando");
        xHashCode = Animator.StringToHash("X");
        yHashCode = Animator.StringToHash("Y");

        GameObject jugadorGO;
        string path = "Assets/Prefabs/Player/Player" + GameManager.instance.gameData.playerData.gender.ToString() +
            GameManager.instance.gameData.playerData.traits.ToString() + ".prefab";

        jugadorGO = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
        GameObject jugadorIns = GameObject.Instantiate(jugadorGO, transform);
        GameManager.instance.sprPlayer = jugadorIns.GetComponent<SpriteRenderer>().sprite;

        GameManager.instance.player = jugadorIns;
        GameManager.instance.player.transform.position = playerSpawnPoint.position;

        //cv = GetComponent<CinemachineVirtualCamera>();
        if (virtualCamera != null)
        {
            virtualCamera.m_Follow = GameManager.instance.player.transform;
        }

        rigidBody = GetComponentInChildren<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

        speed = GameManager.instance.gameData.playerData.speed;

        //floor.GetComponent<TilemapRenderer>().bounds.extents.x;

        maxBound = floor.GetComponent<TilemapRenderer>().bounds.max;
        minBound = floor.GetComponent<TilemapRenderer>().bounds.min;
        objectWidth = transform.GetComponentInChildren<SpriteRenderer>().bounds.extents.x; //extents = size of width / 2
        objectHeight = transform.GetComponentInChildren<SpriteRenderer>().bounds.extents.y; //extents = size of height / 2

        itemsFilter.layerMask = layerInteraccion;
        itemsFilter.useLayerMask = true;

        speechBubble.SetActive(false);
        isSpeechBubbleShown = false;

    }

    private void FixedUpdate()
    {
        if ((ejeHorizontal != 0 || ejeVertical != 0))
        {
            Vector2 vectorVelocidad = new Vector2(ejeHorizontal, ejeVertical) * (float)speed;
            rigidBody.velocity = vectorVelocidad;
            rigidBody.inertia = 0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ejeHorizontal = Input.GetAxis("Horizontal");
        ejeVertical = Input.GetAxis("Vertical");

        if (ejeHorizontal != 0 || ejeVertical != 0)
        {
            SetXYAnimator();

            anim.SetBool(walkHashCode, true);

            if (isSpeechBubbleShown)
            {
                speechBubble.transform.position = new Vector2(GameManager.instance.player.transform.position.x + 1.3f,
                                              GameManager.instance.player.transform.position.y + 1.3f);
            }

            //GirarSprite();
        }
        else
        {
            anim.SetBool(walkHashCode, false);
        }

        if (Input.GetButtonDown("Inventory"))
        {
            StatsPanel.instance.ChangeInfoPanelsState();
        }

        DetermineGazeDirection();
        
    }


    private void LateUpdate()
    {
        Vector3 viewPos = GameManager.instance.player.transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, minBound.x + objectWidth + 0.3f, maxBound.x - objectWidth - 0.3f);
        viewPos.y = Mathf.Clamp(viewPos.y, minBound.y + objectHeight + 0.3f, maxBound.y - objectHeight - 0.3f);
        GameManager.instance.player.transform.position = viewPos;
    }


    private void SetXYAnimator()
    { 
        anim.SetFloat(xHashCode, ejeHorizontal);
        anim.SetFloat(yHashCode, ejeVertical);
    }

    private void DetermineGazeDirection()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            gazeDirection = new Vector2(ejeHorizontal, ejeVertical);
        }
    }

    public RaycastHit2D[] InteractingItems()
    {
        RaycastHit2D[] results = Physics2D.CircleCastAll(GameManager.instance.player.transform.position, 1f, gazeDirection.normalized, 0f, layerInteraccion);
            
        return results;
    }

    public void ShowSpeechBubble(string message)
    {
        speechBubble.SetActive(true);
        speechBubble.transform.position = new Vector2(GameManager.instance.player.transform.position.x + 1.3f, 
                                                      GameManager.instance.player.transform.position.y + 1.3f);
        isSpeechBubbleShown = true;

        speechBubble.GetComponentInChildren<TextMeshPro>().text = message;

    }
}
