using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("InkJSON")]
    [SerializeField] private TextAsset inkJSON;

    private bool isPlayerInRange;

    private void Awake()
    {
        isPlayerInRange = false;
        visualCue.SetActive(false);
    }

    private void Update()
    {
    
        if (isPlayerInRange && !DialogueManager.GetInstance().isDialoguePlaying)
        {
            visualCue.SetActive(true);
            if (Keyboard.current[Key.I].wasPressedThisFrame)
            {
                    DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
            }

        } else
        {
            visualCue.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isPlayerInRange = true;
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isPlayerInRange = false;
        }
    }


}
