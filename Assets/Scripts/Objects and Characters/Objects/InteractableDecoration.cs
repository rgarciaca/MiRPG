using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class InteractableDecoration : Interactive
{
    public Decoration item;

    private SpriteRenderer spRenderer;
    private bool experienceAssigned = false;

    // Start is called before the first frame update
    void Start()
    {
        colliderInt = GetComponent<BoxCollider2D>();
        colliderInt.offset = new Vector2(-0.3f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnValidate()
    {
        spRenderer = GetComponent<SpriteRenderer>();
        spRenderer.sortingLayerName = "Interactive items";
        gameObject.name = item.nombre;
        gameObject.layer = 11;
        spRenderer.sprite = item.sprite;

        //gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        colliderInt = GetComponent<BoxCollider2D>();
        colliderInt.isTrigger = true;
        //colliderInt.size = new Vector2(1, 1);
        //colliderInt.offset = new Vector2(0, 0);
    }

    public override void Interaction()
    {
        GameManager.instance.player.GetComponentInParent<PlayerController>().ShowSpeechBubble(item.commentary);
        if (item.experienceChange > 0 && !experienceAssigned)
        {
            PlayerAttributes.instance.experience += item.experienceChange;

            //StatsPanel.instance.UpdateEquipmentValues((Equipment)item);
            StatsPanel.instance.UpdateExperienceBar();
            experienceAssigned = true;

        }


    }

}
