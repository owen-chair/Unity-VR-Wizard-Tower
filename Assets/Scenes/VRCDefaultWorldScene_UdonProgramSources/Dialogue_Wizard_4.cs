
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Data;

public class Dialogue_Wizard_4 : CustomDialogue
{
    public DataDictionary m_CustomDialogueNodes = new DataDictionary
    {
        {
            // Dialogue Node 0 – The Guarded Door
            (DataToken)0, (DataToken)new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"holdit"},
                {(DataToken)"text", (DataToken)"Hold it right there! This door leads to the upper floors. Guests are not allowed up."},
                {(DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"I was merely curious.",
                        (DataToken)"I'm looking for a room for the night.",
                        (DataToken)"I thought I'd sneak a peek inside."
                    }
                },
                {(DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)1, // Curious branch
                        (DataToken)2, // Need a room branch
                        (DataToken)3  // Sneak peek branch
                    }
                }
            }
        },
        {
            // Dialogue Node 1 – The Curious Guest
            (DataToken)1, (DataToken)new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"curiousityisone"},
                {(DataToken)"text", (DataToken)"Curiosity is one thing, but prying where you're not invited is another. The upper floors are off limits. So, kindly respect the rules."},
                {(DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"Alright, I understand.",
                        (DataToken)"But why such secrecy over a simple door?"
                    }
                },
                {(DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)(-1), // Ends conversation
                        (DataToken)4   // Leads to further explanation
                    }
                }
            }
        },
        {
            // Dialogue Node 2 – In Search of a Room
            (DataToken)2, (DataToken)new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"roomsforguests"},
                {(DataToken)"text", (DataToken)"Rooms for guests are located on this floor. This door leads to areas reserved strictly for the wizards and their work."},
                {(DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"Understood. I'll head back.",
                        (DataToken)"Are you sure there's no exception?"
                    }
                },
                {(DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)(-1),
                        (DataToken)5   // Exception inquiry branch
                    }
                }
            }
        },
        {
            // Dialogue Node 3 – The Sneak Peek
            (DataToken)3, (DataToken)new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"sneakingapeak"},
                {(DataToken)"text", (DataToken)"Sneaking a peek is not permitted. The wizards upstairs have made it clear: no unauthorized eyes beyond this door. Turn back before you cause trouble."},
                {(DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"Sorry, I didn't mean any harm.",
                        (DataToken)"But what's really behind that door?"
                    }
                },
                {(DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)(-1),
                        (DataToken)4  // Leads to the explanation node
                    }
                }
            }
        },
        {
            // Dialogue Node 4 – The Plain Explanation
            (DataToken)4, (DataToken)new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"itssimplereally"},
                {(DataToken)"text", (DataToken)"It's simple really. The floors beyond this door are for the wizards' own work. Advanced magic, exotic potions, and secretive experiments. All far beyond your intelligence, I'm sure."},
                {(DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"I see. I'll stay out of it.",
                        (DataToken)"It still feels a bit over the top."
                    }
                },
                {(DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)(-1),
                        (DataToken)5  // Further inquiry into the policy
                    }
                }
            }
        },
        {
            // Dialogue Node 5 – Further Inquiry on the Policy
            (DataToken)5, (DataToken)new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"youknowwhat"},
                {(DataToken)"text", (DataToken)"You know what, I'm starting to think you might be a spy of some sort. Off to the jail with you!"},
                {(DataToken)"options", (DataToken)new DataList{}},
                {(DataToken)"nextNodes", (DataToken)new DataList{}},
                {(DataToken)"trigger", (DataToken)"TP_TO_JAIL"  },
                {(DataToken)"triggerDelay", (DataToken)true  }
            }
        }
    };

    public void Start()
    {
        this.m_DialogueNodes = this.m_CustomDialogueNodes;
    }
}
