using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.SDK3.Data;
using TMPro;
using UnityEngine.UI;

public class CustomDialogue : UdonSharpBehaviour
{
    public DataDictionary m_DialogueNodes = new DataDictionary
    {
        {
            // Dialogue Node 0
            (DataToken)0, (DataToken) new DataDictionary
            {
                {(DataToken)"text", (DataToken)"[DEBUG] Node 0"},
                {
                    (DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"[DEBUG] Node 0 Option 1 [Leads to Node 1]",
                        (DataToken)"[DEBUG] Node 0 Option 2 [Leads to Node 2]"
                    }
                },
                {
                    (DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)1, // Option 0 leads to Dialogue 1
                        (DataToken)2  // Option 1 leads to Dialogue 2
                    }
                }
            }
        },
        {
            // Dialogue Node 1
            (DataToken)1, (DataToken)new DataDictionary
            {
                {(DataToken)"text", (DataToken)"[DEBUG] Node 1"},
                {
                    (DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"[DEBUG] Node 1 Option 1 [Leads to Node 0]",
                        (DataToken)"[DEBUG] Node 1 Option 2 [End]"
                    }
                },
                {
                    (DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)0,  // Option 0 leads back to Dialogue 0
                        (DataToken)(-1) // Option 1 ends the conversation
                    }
                }
            }
        },
        {
            // Dialogue Node 2
            (DataToken)2, (DataToken)new DataDictionary
            {
                {(DataToken)"text", (DataToken)"[DEBUG] Node 2"},
                {
                    (DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"[DEBUG] Node 2 Option 1 [Leads to Node 0]",
                        (DataToken)"[DEBUG] Node 2 Option 2 [End]"
                    }
                },
                {
                    (DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)0,  // Option 0 leads back to Dialogue 0
                        (DataToken)(-1) // Option 1 ends the conversation
                    }
                }
            }
        },
        {
            // Dialogue Node 3
            (DataToken)3, (DataToken)new DataDictionary
            {
                {(DataToken)"text", (DataToken)"[DEBUG] Node 3 [inaccessible]"},
                {
                    (DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"[DEBUG] Node 3 Option 1 [Leads to Node 0]",
                        (DataToken)"[DEBUG] Node 3 Option 2 [End]"
                    }
                },
                {
                    (DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)0,  // Option 0 leads back to Dialogue 0
                        (DataToken)(-1) // Option 1 ends the conversation
                    }
                }
            }
        }
    };
}
