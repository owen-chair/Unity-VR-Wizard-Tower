using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.SDK3.Data;
using TMPro;
using UnityEngine.UI;

public class Dialogue_Wizard_3 : CustomDialogue
{
    public DataDictionary m_CustomDialogueNodes = new DataDictionary
    {
        {
            // Dialogue Node 0
            (DataToken)0, (DataToken) new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"wellwellwell" },
                {(DataToken)"text", (DataToken)"Well, well, well... what do we have here? A charming visitor? Or just another admirer?"},
                {
                    (DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"Are you flirting with me?",
                        (DataToken)"I just wanted directions...",
                        (DataToken)"What’s a wizard like you doing in a place like this?"
                    }
                },
                {
                    (DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)1, // Option 0 leads to Dialogue 1
                        (DataToken)2, // Option 1 leads to Dialogue 2
                        (DataToken)3  // Option 2 leads to Dialogue 3
                    }
                }
            }
        },
        {
            // Dialogue Node 1
            (DataToken)1, (DataToken) new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"ohinever" },
                {(DataToken)"text", (DataToken)"Oh, I would never be so bold... unless you want me to be."},
                {
                    (DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"Keep talking, wizard...",
                        (DataToken)"I think I need an adult."
                    }
                },
                {
                    (DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)4, // Option 0 leads to more flirting
                        (DataToken)(-1) // Option 1 ends the conversation
                    }
                }
            }
        },
        {
            // Dialogue Node 2
            (DataToken)2, (DataToken)new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"theway" },
                {(DataToken)"text", (DataToken)"Directions? I can show you the way... to my heart."},
                {(DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"That was terrible. I love it.",
                        (DataToken)"I regret asking."
                    }
                },
                {(DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)4, // More flirting
                        (DataToken)(-1) // Ends the conversation
                    }
                }
            }
        },
        {
            // Dialogue Node 3
            (DataToken)3, (DataToken)new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"awizardlikeme" },
                {(DataToken)"text", (DataToken)"A wizard like me? Oh, you know... brewing potions, casting spells, stealing hearts."},
                {(DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"Consider my heart stolen.",
                        (DataToken)"Can I get a refund?"
                    }
                },
                {(DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)4, // More flirting
                        (DataToken)(-1) // Ends the conversation
                    }
                }
            }
        },
        {
            // Dialogue Node 4
            (DataToken)4, (DataToken)new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"cuddlebuddies" },
                {(DataToken)"text", (DataToken)"I guess we're cuddle buddies now. Wanna come to my room?"},
                {(DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"Run away giggling.",
                        (DataToken)"Ask for a spell of eternal charm."
                    }
                },
                {(DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)(-1), // Ends the conversation with running away
                        (DataToken)(-1) // Ends with getting a spell
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
