using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.SDK3.Data;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : UdonSharpBehaviour
{
    public Canvas m_DialogueCanvas;                 // Assign your dialogue canvas in the Inspector
    public TextMeshProUGUI m_DialogueTextComponent; // Assign your dialogue text component
    public Button[] m_OptionButtons;                // Assign your option buttons

    private int m_CurrentDialogueIndex = 0;

    private DataDictionary m_DialogueNodes;
    public Wizard_NPC m_NPC_Reference;

    public QuestManager m_QuestManager;

    void Start()
    {
        HideDialogueUI();
    }

    public bool LoadDialogue()
    {
        // Get the parent of this gameObject
        GameObject gameObjectParent = transform.parent.gameObject;
        if (gameObjectParent == null)
        {
            Debug.LogError("Parent object not found.");
            return false;
        }

        // Retrieve the dialogue nodes from the Dialogue_Wizard_1 script which is in CustomDialogue gameObject which is the next gameObject after this, in the same parent
        // so find objects in the same parent called CustomDialogue
        GameObject customDialogue = gameObjectParent.transform.parent.Find("CustomDialogue").gameObject;
        if (customDialogue == null)
        {
            Debug.LogError("CustomDialogue object not found.");
            return false;
        }

        CustomDialogue dialogue = customDialogue.GetComponent<CustomDialogue>();
        if (dialogue == null)
        {
            Debug.LogError("CustomDialogue component not found.");
            return false;
        }

        this.m_DialogueNodes = dialogue.m_DialogueNodes;

        return true;
    }

    public void StartDialogue()
    {
        if (this.LoadDialogue())
        {
            this.m_CurrentDialogueIndex = 0;  // Start from the initial dialogue
            ShowDialogue();
        }
    }

    private void ShowDialogue()
    {
        // Retrieve the current dialogue node
        DataToken nodeToken;
        if (!this.m_DialogueNodes.TryGetValue((DataToken)this.m_CurrentDialogueIndex, out nodeToken))
        {
            Debug.LogError("Dialogue node not found.");
            return;
        }

        if (nodeToken.TokenType != TokenType.DataDictionary)
        {
            Debug.LogError("Invalid dialogue node data.");
            return;
        }

        DataDictionary currentNode = nodeToken.DataDictionary;

        // Get the dialogue text
        if (!currentNode.TryGetValue((DataToken)"text", out DataToken textToken))
        {
            Debug.LogError("Dialogue text not found.");
            return;
        }

        if (textToken.TokenType != TokenType.String)
        {
            Debug.LogError("Invalid dialogue text data.");
            return;
        }

        string dialogueText = textToken.String;

        this.m_DialogueTextComponent.text = dialogueText;

        // Get the options
        if (!currentNode.TryGetValue((DataToken)"options", out DataToken optionsToken) ||
            !currentNode.TryGetValue((DataToken)"nextNodes", out DataToken nextNodesToken))
        {
            Debug.LogError("Options or nextNodes not found in the current dialogue node.");
            return;
        }

        if (optionsToken.TokenType != TokenType.DataList || nextNodesToken.TokenType != TokenType.DataList)
        {
            Debug.LogError("Invalid options or nextNodes data.");
            return;
        }

        DataList options = optionsToken.DataList;
        DataList nextNodes = nextNodesToken.DataList;

        int optionCount = options.Count;

        for (int i = 0; i < this.m_OptionButtons.Length; i++)
        {
            if (i < optionCount)
            {
                this.m_OptionButtons[i].gameObject.SetActive(true);

                DataToken optionTextToken;
                if (!options.TryGetValue(i, out optionTextToken) || optionTextToken.TokenType != TokenType.String)
                {
                    this.m_OptionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "Option " + i;
                }
                else
                {
                    string optionText = optionTextToken.String;
                    this.m_OptionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = optionText;
                }

            }
            else
            {
                this.m_OptionButtons[i].gameObject.SetActive(false);
            }
        }

        if (currentNode.TryGetValue((DataToken)"sound", out DataToken soundToken))
        {
            if (soundToken.TokenType == TokenType.String)
            {
                PlaySound(soundToken.String);
                if(this.m_CurrentlyPlayingAudioSource != null && optionCount == 0)
                {
                    SendCustomEventDelayedSeconds(
                        nameof(this.EndDialogue),
                        this.m_CurrentlyPlayingAudioSource.clip.length + 0.5f
                    );
                }
            }
        }

        bool triggerDelay = false;
        if (currentNode.TryGetValue((DataToken)"triggerDelay", out DataToken triggerDelayToken))
        {
            if (triggerDelayToken.TokenType == TokenType.Boolean)
            {
                triggerDelay = triggerDelayToken.Boolean;
            }
        }

        float triggerDelayTime = 0.0f;
        if (this.m_CurrentlyPlayingAudioSource != null)
        {
            triggerDelayTime = this.m_CurrentlyPlayingAudioSource.clip.length + 0.5f;
        }

        if (currentNode.TryGetValue((DataToken)"trigger", out DataToken triggerToken))
        {
            if (triggerToken.TokenType == TokenType.String)
            {
                if (this.m_QuestManager != null)
                {
                    
                    this.m_QuestManager.OnTrigger(
                        triggerToken.String,
                        triggerDelay ? triggerDelayTime : 0.0f
                    );
                }
            }
        }

        ShowDialogueUI();
    }

    private AudioSource m_CurrentlyPlayingAudioSource = null;
    public void PlaySound(string soundName)
    {
        // Check if this GameObject has a parent
        if (transform.parent == null)
        {
            Debug.LogError("No parent transform found for this GameObject.");
            return;
        }

        // Get the parent GameObject
        GameObject gameObjectParent = transform.parent.gameObject;

        // Check if the parent has a parent (grandparent)
        if (gameObjectParent.transform.parent == null)
        {
            Debug.LogError("No grandparent transform found for this GameObject.");
            return;
        }

        // Find the CustomDialogue Transform under the grandparent
        Transform customDialogueTransform = gameObjectParent.transform.parent.Find("CustomDialogue");
        if (customDialogueTransform == null)
        {
            Debug.LogError("CustomDialogue object not found.");
            return;
        }
        GameObject customDialogue = customDialogueTransform.gameObject;

        // Find the sound object under CustomDialogue
        Transform soundTransform = customDialogue.transform.Find("sound_" + soundName);
        if (soundTransform == null)
        {
            Debug.LogError("Sound object 'sound_" + soundName + "' not found.");
            return;
        }
        GameObject soundObject = soundTransform.gameObject;

        // Get the AudioSource component
        AudioSource audioSource = soundObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found on " + soundObject.name + ".");
            return;
        }

        // Play the sound
        audioSource.Play();

        this.m_CurrentlyPlayingAudioSource = audioSource;
    }

    // Option selection methods linked in the Inspector
    public void OnDialogueSelected_1()
    {
        OnOptionSelected(0);
    }

    public void OnDialogueSelected_2()
    {
        OnOptionSelected(1);
    }

    public void OnDialogueSelected_3()
    {
        OnOptionSelected(2);
    }

    private float m_LastOptionSelectedTime = 0.0f;
    public void OnOptionSelected(int optionIndex)
    {
        if (Time.time - this.m_LastOptionSelectedTime < 0.5f)
        {
            return;
        }

        this.m_LastOptionSelectedTime = Time.time;

        if (this.m_CurrentlyPlayingAudioSource != null && this.m_CurrentlyPlayingAudioSource.isPlaying)
        {
            return;
        }

        // Retrieve the current dialogue node
        DataToken nodeToken;
        if (!this.m_DialogueNodes.TryGetValue((DataToken)this.m_CurrentDialogueIndex, out nodeToken))
        {
            Debug.LogError("Dialogue node not found.");
            return;
        }

        if (nodeToken.TokenType != TokenType.DataDictionary)
        {
            Debug.LogError("Invalid dialogue node data.");
            return;
        }

        DataDictionary currentNode = nodeToken.DataDictionary;

        // Get the nextNodes list
        if (!currentNode.TryGetValue((DataToken)"nextNodes", out DataToken nextNodesToken))
        {
            Debug.LogError("nextNodes not found in the current dialogue node.");
            return;
        }

        if (nextNodesToken.TokenType != TokenType.DataList)
        {
            Debug.LogError("Invalid nextNodes data.");
            return;
        }

        DataList nextNodes = nextNodesToken.DataList;

        if (optionIndex < 0 || optionIndex >= nextNodes.Count)
        {
            Debug.LogError("Option index is out of bounds.");
            return;
        }

        // Get the next dialogue ID
        DataToken nextDialogueIdToken;
        if (!nextNodes.TryGetValue(optionIndex, out nextDialogueIdToken))
        {
            Debug.LogError("Next dialogue ID not found.");
            return;
        }

        if (nextDialogueIdToken.TokenType != TokenType.Int)
        {
            Debug.LogError("Invalid next dialogue ID data.");
            return;
        }

        int nextDialogueId = nextDialogueIdToken.Int;

        if (nextDialogueId == -1)
        {
            // End the conversation
            HideDialogueUI();
            this.m_NPC_Reference.OnDialogueEnded();
            return;
        }
        else
        {
            // Move to the next dialogue node
            this.m_CurrentDialogueIndex = nextDialogueId;
            ShowDialogue();
        }
    }

    private void ShowDialogueUI()
    {
        if (this.m_DialogueCanvas != null)
        {
            this.m_DialogueCanvas.gameObject.SetActive(true);
        }
    }

    private void HideDialogueUI()
    {
        if (this.m_DialogueCanvas != null)
        {
            this.m_DialogueCanvas.gameObject.SetActive(false);
        }
    }

    public void EndDialogue()
    {
        HideDialogueUI();
        this.m_NPC_Reference.OnDialogueEnded();
        this.m_CurrentDialogueIndex = 0;
    }

    public override void Interact()
    {
        StartDialogue();
    }
}