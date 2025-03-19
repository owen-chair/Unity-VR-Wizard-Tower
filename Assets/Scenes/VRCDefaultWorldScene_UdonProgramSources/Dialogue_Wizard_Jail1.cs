
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Data;

public class Dialogue_Wizard_Jail1 : CustomDialogue
{
    public DataDictionary m_CustomDialogueNodes = new DataDictionary
    {
        {
            // Dialogue Node 0 – Opening Interrogation
            (DataToken)0, (DataToken)new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"tellmewhy"},
                {(DataToken)"text", (DataToken)"Well, well well... It seems we have a slimy spy amongst us. Tell me — why are you so interested in the wizards tower?"},
                {(DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"I didn't do anything wrong!",
                        (DataToken)"I was just curious.",
                        (DataToken)"I was looking for someone."
                    }
                },
                {(DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)1, // Denial branch
                        (DataToken)2, // Curious branch
                        (DataToken)3  // Looking-for-someone branch
                    }
                }
            }
        },
        {
            // Dialogue Node 1 – Denial Branch
            (DataToken)1, (DataToken)new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"donttrytoplay"},
                {(DataToken)"text", (DataToken)"Don't try to play innocent with me. Your eyes say otherwise. Now, explain yourself—what exactly are you doing here?"},
                {(DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"I was just passing by.",
                        (DataToken)"I was investigating the tower."
                    }
                },
                {(DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)4, // Leads to the common inquiry node
                        (DataToken)5  // Admits a hint of snooping
                    }
                }
            }
        },
        {
            // Dialogue Node 2 – Curious Branch
            (DataToken)2, (DataToken)new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"curiosity"},
                {(DataToken)"text", (DataToken)"Curiosity in these parts is a dangerous game. Now tell me, why were you really here?"},
                {(DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"I wanted to see the secrets for myself.",
                        (DataToken)"I was following someone.",
                        (DataToken)"I was simply lost."
                    }
                },
                {(DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)5, // Admits a hint of snooping
                        (DataToken)6, // Following someone branch
                        (DataToken)4  // Common inquiry node
                    }
                }
            }
        },
        {
            // Dialogue Node 3 – Looking-for-Someone Branch
            (DataToken)3, (DataToken)new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"lookingforsomeone"},
                {(DataToken)"text", (DataToken)"Looking for someone in a restricted area? That hardly sounds innocent. Who were you after?"},
                {(DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"A friend of mine.",
                        (DataToken)"I saw someone go up here and wanted to see for myself.",
                        (DataToken)"That's none of your business."
                    }
                },
                {(DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)4, // Common inquiry node
                        (DataToken)6, // Suspicious branch
                        (DataToken)7  // Hostile branch
                    }
                }
            }
        },
        {
            // Dialogue Node 4 – Common Inquiry Node
            (DataToken)4, (DataToken)new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"youranswers"},
                {(DataToken)"text", (DataToken)"Your answers are as vague as they are unconvincing. Be clear — what are you actually doing here?"},
                {(DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"I saw some magic and wanted to learn more powerful spells!",
                        (DataToken)"I heard there secret meetings taking place upstairs."
                    }
                },
                {(DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)8, // Final warning node
                        (DataToken)8  // Final warning node
                    }
                }
            }
        },
        {
            // Dialogue Node 5 – The Suspicious Admission
            (DataToken)5, (DataToken)new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"soyouadmit"},
                {(DataToken)"text", (DataToken)"So you admit you were snooping around? Explain yourself!"},
                {(DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"I was told there is powerful magic here.",
                        (DataToken)"I was looking for my friend!"
                    }
                },
                {(DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)8, // Final warning node
                        (DataToken)8  // Final warning node
                    }
                }
            }
        },
        {
            // Dialogue Node 6 – Following Someone Branch
            (DataToken)6, (DataToken)new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"followingsomeone"},
                {(DataToken)"text", (DataToken)"Following someone into a restricted zone is a dangerous gambit. What makes you think you had the right to do that?"},
                {(DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"I'm just a simple wanderer, I didn't think it was so serious!",
                        (DataToken)"I'm not afraid of your threats."
                    }
                },
                {(DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)8, // Final warning node
                        (DataToken)8  // Final warning node
                    }
                }
            }
        },
        {
            // Dialogue Node 7 – Hostile Branch
            (DataToken)7, (DataToken)new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"playingdumb"},
                {(DataToken)"text", (DataToken)"Playing dumb won't save you here. I demand an explanation!"},
                {(DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"Just tell me what you want me to say and I'll say it...",
                        (DataToken)"Fine! I'm a spy. I read all your books and know all your secrets! Muahahaha!"
                    }
                },
                {(DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)8, // Final warning node
                        (DataToken)9  // Additional confrontation branch
                    }
                }
            }
        },
        {
            // Dialogue Node 8 – Final Warning
            (DataToken)8, (DataToken)new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"enough"},
                {(DataToken)"text", (DataToken)"Enough. Whether driven by curiosity, defiance, or guilt, you've overstepped your bounds. Your actions have consequences."},
                {(DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"I'm sorry.",
                        (DataToken)"I accept my fate.",
                        (DataToken)"There's nothing you can do to me."
                    }
                },
                {(DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)10, // Final outcome
                        (DataToken)10, // Final outcome
                        (DataToken)10  // Final outcome
                    }
                }
            }
        },
        {
            // Dialogue Node 9 – Additional Confrontation
            (DataToken)9, (DataToken)new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"knewit"},
                {(DataToken)"text", (DataToken)"I knew it! Even the way you walk is suspicious!"},
                {(DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"..."
                    }
                },
                {(DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)10
                    }
                }
            }
        },
        {
            // Dialogue Node 10 – Final Outcome with Trigger
            (DataToken)10, (DataToken)new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"reportyou"},
                {(DataToken)"text", (DataToken)"I'm going to report this to Archmage Tywn! I expect he will be here to deal with you shortly."},
                {(DataToken)"options", (DataToken)new DataList{}},
                {(DataToken)"nextNodes", (DataToken)new DataList{}},
                {(DataToken)"trigger", (DataToken)"WALL_SMASH"},
                {(DataToken)"triggerDelay", (DataToken)true}
            }
        }
    };


    public void Start()
    {
        this.m_DialogueNodes = this.m_CustomDialogueNodes;
    }
}
