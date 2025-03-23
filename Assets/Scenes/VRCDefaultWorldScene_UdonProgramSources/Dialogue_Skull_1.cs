using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Data;

public class Dialogue_Skull_1 : CustomDialogue
{
    public DataDictionary m_CustomDialogueNodes = new DataDictionary
    {
        {
            // Dialogue Node 0 – Initial Confrontation
            (DataToken)0, (DataToken)new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"putmedown"},
                {(DataToken)"text", (DataToken)"Put me down and speak to me, you thieving rat! Disturbing my eternal rest, and desecrating my corpse! Take one more step and you'll have the rest of my body to answer to!"},
                {(DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"I was just trying to help!",
                        (DataToken)"I didn't know you'd mind...",
                    }
                },
                {(DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)1, // Trying to help
                        (DataToken)2, // Didn't know
                    }
                }
            }
        },
        {
            // Dialogue Node 1 – Trying to Help
            (DataToken)1, (DataToken)new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"tryingtohelp"},
                {(DataToken)"text", (DataToken)"'Trying to help'?! You've broken my neck and ran around the morgue with me in your stinky sweaty hands!"},
                {(DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"The statue upstairs told me to bless you.",
                        (DataToken)"Actually I washed you in the blessed pool, you should be thankful!"
                    }
                },
                {(DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)4, // Statue explanation
                        (DataToken)5  // Generic defense
                    }
                }
            }
        },
        {
            // Dialogue Node 2 – Didn't Know
            (DataToken)2, (DataToken)new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"ofcourseimind"},
                {(DataToken)"text", (DataToken)"Of course I mind! Would you like it if someone touched your bones?!"},
                {(DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"Maybe... depends...",
                        (DataToken)"But the statue told me to!"
                    }
                },
                {(DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)5, // Generic defense
                        (DataToken)4  // Statue explanation
                    }
                }
            }
        },
        {
            // Dialogue Node 3 – Dismissive
            (DataToken)3, (DataToken)new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"impudent"},
                {(DataToken)"text", (DataToken)"You impudent wretch! You will regret this disrespect!"},
                {(DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"Wait! I was told to do this! The statue upstairs told me to bless you!"
                    }
                },
                {(DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)4  // Statue explanation
                    }
                }
            }
        },
        {
            // Dialogue Node 4 – Statue Explanation
            (DataToken)4, (DataToken)new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"ahthatwouldbe"},
                {(DataToken)"text", (DataToken)"Ah.. that would be Archmage Nightwood. He probably thinks it's funny to send mortals to bother me. Take me to him immediately."},
                {(DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"Alright, follow me...",
                        (DataToken)"What about the rest of your body?"
                    }
                },
                {(DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)8, // Leads to movement event
                        (DataToken)6  // Same movement event
                    }
                }
            }
        },
        {
            // Dialogue Node 5 – Defensive
            (DataToken)5, (DataToken)new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"andyourenot"},
                {(DataToken)"text", (DataToken)"And you're not even a wizard! Why are you touching me in the first place?!"},
                {(DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"I was told by the statue upstairs to bless you.",
                        (DataToken)"Your corpse kind of stinks so I was washing it"
                    }
                },
                {(DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)4, // Statue explanation
                        (DataToken)9  // Statue explanation
                    }
                }
            }
        },
        {
            // Dialogue Node 9 – Defensive
            (DataToken)9, (DataToken)new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"ofcourseitsmells"},
                {(DataToken)"text", (DataToken)"Of course 'it' smells, I'M A CORPSE! What kind of sick pervert touches corpses anyway!"},
                {(DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"I was told by the statue upstairs to bless you.",
                        (DataToken)"I was looking for some cool ornaments for my bedroom."
                    }
                },
                {(DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)4, // Statue explanation
                        (DataToken)3  // Statue explanation
                    }
                }
            }
        },
        {
            // Dialogue Node 6 – Defensive
            (DataToken)6, (DataToken)new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"dontworry"},
                {(DataToken)"text", (DataToken)"Don't worry about it, just take me to the statue and don't drop me!"},
                {(DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"Alright, follow me...",
                        (DataToken)"Why can't you just use your body to walk?"
                    }
                },
                {(DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)8, // Statue explanation
                        (DataToken)7  // Defensive
                    }
                }
            }
        },
        {
            // Dialogue Node 7 – Defensive
            (DataToken)7, (DataToken)new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"haveyouever"},
                {(DataToken)"text", (DataToken)"Have you ever tried commanding a skeleton with no flesh whilst being dead? It's not easy!"},
                {(DataToken)"options", (DataToken)new DataList
                    {
                        (DataToken)"Alright, follow me...",
                    }
                },
                {(DataToken)"nextNodes", (DataToken)new DataList
                    {
                        (DataToken)8, // Statue explanation
                    }
                }
            }
        },
        {
            // Dialogue Node 8 – Movement Event
            (DataToken)8, (DataToken)new DataDictionary
            {
                {(DataToken)"sound", (DataToken)"veryfunny"},
                {(DataToken)"text", (DataToken)"Very funny. You'll obviously have to carry me there, and don't drop me..."},
                {(DataToken)"options", (DataToken)new DataList{}},
                {(DataToken)"nextNodes", (DataToken)new DataList{}},
                {(DataToken)"trigger", (DataToken)"RETURN_SKULL_TO_STATUE"}
            }
        }
    };

    public void Start()
    {
        this.m_DialogueNodes = this.m_CustomDialogueNodes;
    }
}
