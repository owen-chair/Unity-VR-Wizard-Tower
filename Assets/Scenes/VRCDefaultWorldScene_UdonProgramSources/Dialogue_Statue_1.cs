using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Data;

public class Dialogue_Statue_1 : CustomDialogue
{
    public DataDictionary m_CustomDialogueNodes = new DataDictionary
    {
        {
            // Dialogue Node 0 – Statue Greeting
            (DataToken)0, (DataToken)new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"welcomeguest"},
                {(DataToken)"text", (DataToken)"Welcome, guest, to the Grand Wizards' Tower. You have my blessing."},
                {(DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"Who are you?",
                    }
                },
                {(DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)1, // Who are you
                    }
                }
            }
        },
        {
            // Dialogue Node 1 – Who Are You
            (DataToken)1, (DataToken)new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"imarchmage"},
                {(DataToken)"text", (DataToken)"I am Archmage Nightwood. My soul is bound to this statue, and I continue to serve this realm even beyond my death."},
                {(DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"How did that happen?",
                        (DataToken)"I tend to avoid magical entities... They usually cause trouble..."
                    }
                },
                {(DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)4, // How did that happen?
                        (DataToken)2 
                    }
                }
            }
        },
        {
            (DataToken)2, (DataToken)new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"youarewise"},
                {(DataToken)"text", (DataToken)"You are wise to be cautious. However, this tower is a safe haven under my protection. You will not be harmed here."},
                {(DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"How did you end up bound to the statue?",
                    }
                },
                {(DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)4, // What happened
                    }
                }
            }
        },
        {
            // Dialogue Node 3 – Can You Help Me
            (DataToken)3, (DataToken)new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"thereisasoul"},
                {(DataToken)"text", (DataToken)"There is a soul that lingers in unrest. A skull, locked away in the morgue below. It must be blessed."},
                {(DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"Why must it be blessed?",
                        (DataToken)"What happens if I do this?"
                    }
                },
                {(DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)6, // Why must it be blessed?
                        (DataToken)6  // What happens?
                    }
                }
            }
        },
        {
            // Dialogue Node 4 – What Happened to You
            (DataToken)4, (DataToken)new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"iboundmyself"},
                {(DataToken)"text", (DataToken)"I bound myself to the statue in order to continue my work as the founder of the Grand Wizards' Tower. I remain here in advisory capacity only."},
                {(DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"That sounds like powerful magic."
                    }
                },
                {(DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)5  // Discuss magic
                    }
                }
            }
        },
        {
            // Dialogue Node 5 – Sympathy Response
            (DataToken)5, (DataToken)new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"yesinfact"},
                {(DataToken)"text", (DataToken)"Yes, in fact the spell itself was the cause of my mortal end. I've been here for over 400 years."},
                {(DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"Impressive, what sort of role do you play now?"
                    }
                },
                {(DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)9 // Leads to request
                    }
                }
            }
        },
        {
            // Dialogue Node 9 – Request
            (DataToken)9, (DataToken)new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"thewizardsgenerally"},
                {(DataToken)"text", (DataToken)"The wizards generally look to me for advice and as a scrying aide. I have the wizards run errands and quests to serve the Tower. Speaking of such, would you be able to help me with a small task?"},
                {(DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"Sure, what do you need?"
                    }
                },
                {(DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)3 // Leads to request
                    }
                }
            }
        },
        {
            // Dialogue Node 6 – Why Bless the Skull
            (DataToken)6, (DataToken)new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"thesoulattach"},
                {(DataToken)"text", (DataToken)"The soul attached to that skull is in turmoil. A proper blessing will grant it peace."},
                {(DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"I will do it.",
                        (DataToken)"And if it doesn’t work?"
                    }
                },
                {(DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)7, // Accept quest
                        (DataToken)8  // Concern about failure
                    }
                }
            }
        },
        {
            // Dialogue Node 7 – Accept Quest
            (DataToken)7, (DataToken)new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"goodretrieve"},
                {(DataToken)"text", (DataToken)"Good. Retrieve the skull, bless it, and return here."},
                {(DataToken)"options", (DataToken)new DataList{}},
                {(DataToken)"nextNodes", (DataToken)new DataList{}},
                {(DataToken)"trigger", (DataToken)"START_SKULL_QUEST"},
                {(DataToken)"triggerDelay", (DataToken)true}
            }
        },
        {
            // Dialogue Node 8 – Concern About Failure
            (DataToken)8, (DataToken)new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"ifitdoesnot"},
                {(DataToken)"text", (DataToken)"If it does not work... then we may have a different problem on our hands."},
                {(DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"I will do it anyway."
                    }
                },
                {(DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)7
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