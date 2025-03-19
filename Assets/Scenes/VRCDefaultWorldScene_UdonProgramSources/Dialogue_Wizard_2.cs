using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.SDK3.Data;
using TMPro;
using UnityEngine.UI;

public class Dialogue_Wizard_2 : CustomDialogue
{
    public DataDictionary m_CustomDialogueNodes = new DataDictionary
    {
        {
            // Dialogue Node 0
            (DataToken)0, (DataToken) new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"ratherbusy" },
                {(DataToken)"text", (DataToken)"I am rather busy, please leave me alone."},
                {
                    (DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"Please daddy? uwu",
                        (DataToken)"Sowwy :3"
                    }
                },
                {
                    (DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)2, // Option 0 leads to Dialogue 2
                        (DataToken)3  // Option 1 leads to Dialogue 3
                    }
                }
            }
        },
        {
            // Dialogue Node 1
            (DataToken)1, (DataToken) new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"ok" },
                {(DataToken)"text", (DataToken)"Ok."},
                {
                    (DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"Who are you?",
                        (DataToken)"What is this place?",
                        (DataToken)"Nevermind."
                    }
                },
                {
                    (DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)2, // Option 0 leads to Dialogue 1
                        (DataToken)3,  // Option 1 leads to Dialogue 2
                        (DataToken)(-1) // Option 2 ends the conversation
                    }
                }
            }
        },
        {
            // Dialogue Node 2
            (DataToken)2, (DataToken)new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"notyourdaddy" },
                {(DataToken)"text", (DataToken)"I am not your daddy, please leave me alone."},
                {(DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"Ok."
                    }
                },
                {(DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)(-1) // Option 0 ends the conversation
                    }
                }
            }
        },
        {
            // Dialogue Node 3
            (DataToken)3, (DataToken)new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"ok" },
                {(DataToken)"text", (DataToken)"Ok."},
                {(DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"Ok."
                    }
                },
                {(DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)(-1) // Option 1 ends the conversation.
                    }
                }
            }
        }
    };

    public void Start()
    {
        this.m_DialogueNodes = this.m_CustomDialogueNodes;
    }
}
