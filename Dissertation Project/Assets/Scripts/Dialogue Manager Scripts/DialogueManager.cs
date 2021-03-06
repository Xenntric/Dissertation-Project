using Godot;
using System;
using Godot.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;


public class DialogueManager : Node
{
    private RegexCheck Check;
    private DialogueLoader Load;
    private DialogueLogic Logic;
    
    [Export] private List<Texture> Emotes;
    private List<Node> NPCstack;
    private List<NPCs> NPCList;

    private RichTextLabel CurrentText;
    private Sprite CurrentEmote;

    private Node targetNPC;
    private  bool readLine = false;

	public override void _Ready()
	{
        NPCstack = new List<Node>();
        NPCList = new List<NPCs>();

        AddChild(CurrentEmote = new Sprite());
        AddChild(CurrentText = new RichTextLabel());

        CurrentText.BbcodeEnabled = true;

        for (int i = 0; i < GetNode("../NPCs").GetChildren().Count; i++)
        {
            NPCList.Add(new NPCs());
            NPCList[i].Character = (GetNode("../NPCs").GetChild(i));
            NPCList[i].TextFile = (Load.LoadFile(NPCList[i].Character));

            //If there's no file, don't bother trying to find the lines.
            if(NPCList[i].TextFile != null )
            {
                NPCList[i].Lines = (Load.LoadLines(NPCList[i].TextFile));
                NPCList[i].Bools = (Load.LoadBools(NPCList[i].Lines));
                NPCList[i].Ints  = (Load.LoadInts(NPCList[i].Lines));

                foreach (var Bool in NPCList[i].Bools)
                {
                    GD.Print("Bools @ " + NPCList[i].Character.Name + ": " + Bool.Key.ToString());
                    NPCList[i].TimesTalkedTo++;
                }
                foreach (var Int in NPCList[i].Ints)
                {
                    GD.Print("Ints @ " + NPCList[i].Character.Name + ": " + Int.Key.ToString() + " = " + Int.Value.ToString());
                    NPCList[i].TimesTalkedTo++;
                }
            }
        }
	}
    public void ReadLine()
    {
        GD.Print("NPCstack volume = " + NPCstack.Count);
        foreach (var NPC in NPCstack)
        {
            GD.Print(NPC.Name);
        }
        //If the Player is near an NPC
        if(NPCstack.Any())
        { 
            //For(amount of NPCs in Scene)
            for (int i = 0; i < NPCList.Count; i++)
            {
                GD.Print("searching for: " + NPCList[i].Character.Name);

                //Finds latest NPC the player has collided with
                if (NPCstack[NPCstack.Count -1] == NPCList[i].Character)
                {
                    targetNPC = NPCList[i].Character;
                    var enumerator = new RegexCheck.Parse();
                    
                    /***CATCH FOR NOT FINDING FILE ***/
                    if (NPCList[i].TextFile == null)
                    {
                        GD.PrintErr("No File for " + NPCList[i].Character.Name + " found");
                        return;
                    }

                    /*** CATCH FOR NO TARGETS NODE***/
                    var HasTargetNode = false;
                    foreach (Node child in targetNPC.GetChildren())
                    {
                        if(child.Name == "Targets")
                        {
                            HasTargetNode = true;
                            break;
                        }
                    }
                    if(!HasTargetNode)
                    {
                        GD.PrintErr(NPCList[i].Character.Name+ " has no 'Targets' node");
                        return;
                    }

                    var ReadLine = NPCList[i].Lines[NPCList[i].TimesTalkedTo];
                    var character = NPCList[i].Character;
                    Tuple<string, string> linetosay = new Tuple<string, string>(null, null);

                    enumerator = SetEnumerator(NPCList[i], ReadLine, ref linetosay);
                    ActOnEnumerator(i, enumerator, character, linetosay);

                    GD.Print("NPC " + NPCList[i].Character.Name);
                    GD.Print("Times Talked To: " + NPCList[i].TimesTalkedTo);
                    return;
                }
            }
        }
    }

    private RegexCheck.Parse SetEnumerator(NPCs npc, string ReadLine, ref Tuple<string, string> linetosay)
    {
        RegexCheck.Parse enumerator;

        if(ReadLine.Substr(0,1) == "*" && !readLine)
        {
            GD.Print("skipping");
            enumerator = RegexCheck.Parse.SkipLine;
            return enumerator;
        }

        if(ReadLine.Substr(0,1) == "*" && readLine)
        {
            GD.Print("readLine");
            ReadLine = Logic.PopFirst(Logic.PopFirst(ReadLine));
        }

        if (Check.END.IsMatch(ReadLine))
        {
            enumerator = RegexCheck.Parse.End;
        }
        
        else if (Check.IF.IsMatch(ReadLine))
        {
            var SetLine = Check.CaptureParentheses(ReadLine);
            if(Check.Has("=",SetLine) && Regex.Match(SetLine, @"[0-9]").Success)
            {
                IfNumber(npc, SetLine);
            }

            else
            {
                SetLine = IfBool(npc, SetLine);
            }

            enumerator = RegexCheck.Parse.If;
        }

        else if (Check.SET.IsMatch(ReadLine))
        {
            GD.Print("Found Set");
            SetInTextVariable(ReadLine);
            enumerator = RegexCheck.Parse.Set;
        }

        else if(Check.PLAY.IsMatch(ReadLine))
        {
            var HasAnimPlayer = false;
            foreach (Node child in npc.Character.GetChildren())
            {
                if(child is AnimationPlayer)
                {
                    HasAnimPlayer = true;
                    npc.AnimationPlayer = GetNode<AnimationPlayer>(child.GetPath());
                }
            }

            if(HasAnimPlayer)
            {
                npc.AnimationPlayer.Stop();
                npc.AnimationPlayer.Play(Check.CaptureParentheses(ReadLine));
            }
            else
            {
                GD.PrintErr("No Animation Player Found");
            }
            enumerator = RegexCheck.Parse.PlayAnimation;
        }
        else
        {
            //if an emote sprite is already present, get rid of it
            if (CurrentEmote.Texture != null)
            {
                CurrentEmote.Texture = null;
            }
            if (Check.HasEMOTE(ReadLine))
            {
                linetosay = new Tuple<string, string>(
                    Check.DIALOGUE.Match(ReadLine).ToString(),
                    Logic.RemoveWhitespace(Logic.PopFirst(Check.EMOTE.Match(ReadLine).ToString())));
                enumerator = RegexCheck.Parse.DialogueEmote;
                GD.Print("Dialogue to read: " + linetosay.Item1);
                GD.Print("emote to read: " + linetosay.Item2);
            }
            else
            {
                linetosay = new Tuple<string, string>(
                    Check.DIALOGUE.Match(ReadLine).ToString(),null);
                    
                GD.Print("No Emote Found");
                enumerator = RegexCheck.Parse.Dialogue;
            }
        }

        return enumerator;
    }

    private string IfBool(NPCs npc, string SetLine)
    {
        bool trueFalsePath = false;
        string GetVar = "";
        string GetCharacter;
        
        //If user is trying to reference another character's booleans
        if(Check.Has(".", SetLine))
        {
            GetVar = Logic.PopFirst(Check.CaptureWithin(".","$",SetLine));
            if (SetLine.Substr(0, 1) == "!")
            {
                GetCharacter = Logic.PopFirst(Logic.PopLast(Check.CaptureWithin("^",".",SetLine))).ToString();
                trueFalsePath = false;
            }
            else
            {
                GetCharacter = Logic.PopLast(Check.CaptureWithin("^",".",SetLine)).ToString();
                trueFalsePath = true;
            }

            foreach (var NPC in NPCList)
            {
                if(NPC.Character.Name == GetCharacter)
                {
                    npc = NPC;
                    SetLine = GetVar;
                    if(!trueFalsePath)
                    {
                        SetLine = "!" + GetVar;
                    }
                }
            }
        }

        GD.Print("CHARACTER TO CHECK: " + npc.Character.Name);
        GD.Print("BOOL TO CHECK: " + GetVar);
        GD.Print("PARSE: " + SetLine);

        
        GD.Print("Boolean IF");
        if (SetLine.Substr(0, 1) != "!")
        {
            //if True
            foreach (var key in npc.Bools.Keys)
            {
                if (key == SetLine && npc.Bools[key])
                {
                    GD.Print("Got it!");
                    readLine = true;
                    break;
                }
                else
                {
                    GD.Print("TRUE cant find");
                    readLine = false;
                }
            }
        }
        else
        {
            GD.Print("Is negative check");
            SetLine = Logic.PopFirst(SetLine);
            //if false;
            foreach (var key in npc.Bools.Keys)
            {

                if (key == SetLine && !npc.Bools[key])
                {
                    GD.Print("Got it!");
                    readLine = true;
                    break;
                }
                else
                {
                    GD.Print("FALSE cant find");
                    readLine = false;
                }
            }
        }

        return SetLine;
    }

    private void IfNumber(NPCs npc, string SetLine)
    {
        var CheckVariable = Check.CaptureWithin("^", "=", SetLine);
        var CheckNumber = Logic.PopFirst(Check.CaptureWithin("=", "$", SetLine));

        bool isNot = false;
        GD.Print("Integer IF");
        var parseChars = SetLine.ToCharArray();
        for (int i = 0; i < parseChars.Length; i++)
        {
            if (parseChars[i] == '=')
            {
                if (parseChars[i - 1] == '!')
                {
                    GD.Print("Is negative check");
                    CheckVariable = Logic.PopLast(CheckVariable);
                    isNot = true;
                }
            }
        }
        CheckVariable = Logic.PopLast(CheckVariable);
        GD.Print("Variable to check: " + CheckVariable);
        GD.Print("Number to check: " + CheckNumber);

        if (!isNot)
        {
            //if True
            foreach (var key in npc.Ints.Keys)
            {
                GD.Print("Checking Key: " + key);
                if (key == CheckVariable && npc.Ints[key] == System.Int32.Parse(CheckNumber))
                {
                    GD.Print("TRUE Actually Yes");
                    readLine = true;
                    break;
                }
                else
                {
                    GD.Print("TRUE Actually No");
                    readLine = false;
                }
            }
        }
        else
        {
            //if False
            foreach (var key in npc.Ints.Keys)
            {
                if (key == CheckVariable && npc.Ints[key] != System.Int32.Parse(CheckNumber))
                {
                    GD.Print("FALSE Actually Yes");
                    readLine = true;
                    break;
                }
                else
                {
                    GD.Print("FALSE Actually No");
                    readLine = false;
                }
            }
        }
    }

    private void SetInTextVariable(string ReadLine)
    {
        var SetLine      = Check.CaptureParentheses(ReadLine);
        var SetVar       = Logic.PopLast(Check.CaptureWithin("^","=",SetLine));
        var SetCharacter = Logic.PopLast(Check.CaptureWithin("^",".",SetLine));
        var SetType      = Logic.PopFirst(Logic.PopLast(Check.CaptureWithin(".","=",SetLine)));
        var SetValue     = Logic.PopFirst(Check.CaptureWithin("=","$",SetLine));

        GD.Print("Var = " + SetVar);
        GD.Print("Character = " + SetCharacter);
        GD.Print("Type = " + SetType);
        GD.Print("Value = " + SetValue);

        foreach (var NPC in NPCList)
        {
            if (NPC.Character.Name == SetCharacter)
            {
                GD.Print("Found Character");

                foreach (var key in NPC.Bools.Keys)
                {
                    GD.Print("Finding Bools");
                    if (key == SetType)
                    {
                        GD.Print("Found Key");
                        GD.Print("Character: " + NPC.Character.Name + " with key: " + key + " with value: " + NPC.Bools[key]);
                        NPC.Bools[key] = System.Boolean.Parse(SetValue);
                        GD.Print("Character: " + NPC.Character.Name + " with key: " + key + " with value: " + NPC.Bools[key]);
                        GD.Print("Set Key");
                        break;
                    }

                    GD.Print("Checking next Bool");
                }

                GD.Print("Moving on to ints...");

                foreach (var key in NPC.Ints.Keys)
                {
                    GD.Print("Finding Ints");
                    if (key == SetType)
                    {
                        GD.Print("Found Key");
                        NPC.Ints[key] = System.Int32.Parse(SetValue);
                        break;
                    }
                }

            }
        }
    }

    private void ActOnEnumerator(int i, RegexCheck.Parse enumerator, Node character, Tuple<string, string> linetosay)
    {
        switch (enumerator)
        {
            case RegexCheck.Parse.DialogueEmote:
                {

                    DisplayText(character, linetosay.Item1);
                    MatchEmoteToImage(character, linetosay.Item2);

                    NPCList[i].TimesTalkedTo = (NPCList[i].TimesTalkedTo + 1);
                    break;
                }
            case RegexCheck.Parse.Dialogue:
                {
                    DisplayText(character, linetosay.Item1);

                    NPCList[i].TimesTalkedTo = (NPCList[i].TimesTalkedTo + 1);
                    break;
                }
            case RegexCheck.Parse.End:
                {
                    //Clear all Text and Emotes on screen
                    DisplayText(character, null);
                    CurrentEmote.Texture = null;                    
                    GD.Print("Ended");
                    break;
                }
            case RegexCheck.Parse.If:
                {
                    
                    //Because this line is a function, I don't want it to interrupt the flow of the
                    //Dialogue, so I am forcing the next line to get read
                    NPCList[i].TimesTalkedTo = (NPCList[i].TimesTalkedTo + 1);

                    ReadLine();
                    break;
                }
            case RegexCheck.Parse.Set:
                {
                    GD.Print("Incrementing Talked to");
                    
                    //Because this line is a function, I don't want it to interrupt the flow of the
                    //Dialogue, so I am forcing the next line to get read
                    NPCList[i].TimesTalkedTo = (NPCList[i].TimesTalkedTo + 1);

                    ReadLine();
                    break;
                }
            case RegexCheck.Parse.SkipLine:
                {
                    NPCList[i].TimesTalkedTo = (NPCList[i].TimesTalkedTo + 1);
                    ReadLine();
                    break;                
                }
            case RegexCheck.Parse.PlayAnimation:
                {
                    NPCList[i].TimesTalkedTo = (NPCList[i].TimesTalkedTo + 1);
                    ReadLine();
                    break;
                }

            default:
                break;
        }
    }

    private void MatchEmoteToImage(Node NPC, string emote)
    {
        emote = emote + ".PNG";
        foreach (var image in Emotes)
        {
            var emoteFilename = Logic.RemoveWhitespace(Check.FILENAME.Match(image.ResourcePath).ToString());
            //GD.Print("bing " + emoteFilename);
            
            
            string NPCsPath = NPC.GetPath();
            var NPCproperties = GetNode<StaticBody2D>(NPCsPath);
            if (emote == emoteFilename.ToString())
            {
                DisplayEmote(image, NPCsPath, NPCproperties);
            }
            else
            {
                //Emote is not a recognised emote
            }
        }
    }

    private void DisplayEmote(Texture image, string NPCsPath, StaticBody2D NPCproperties)
    {
        //Targets the EmotePoint node which needs to be the first Child of 'Targets'
        
        var EmotePoint = GetNode(new NodePath(NPCsPath + "/" + "Targets")).GetChild(0) as Node2D;
        var NPCPoint = GetNode(new NodePath(NPCsPath)) as Node2D;
        GD.Print("Emote Position: " + EmotePoint.Position);

        //Set Emote to desired PNG
        CurrentEmote.Texture = image;
        CurrentEmote.Scale = new Vector2(NPCproperties.Scale);
        CurrentEmote.Position = new Vector2(EmotePoint.Position);
        CurrentEmote.ZIndex = NPCproperties.ZIndex + 1;
    }

    private void DisplayText(Node NPC, string DialogueToEnact)
    {
        string NPCsPath = NPC.GetPath();
        var TextPoint = GetNode(new NodePath(NPCsPath + "/" + "Targets")).GetChild(1) as Node2D;

        CurrentText.RectSize = new Vector2(CurrentText.GetFont("mono_font").GetStringSize(DialogueToEnact) * 2);
        var centre = new Vector2(CurrentText.RectSize.x / 2, 0);
        if(DialogueToEnact != null && DialogueToEnact.Substr(0,1) == "*")
        {
            DialogueToEnact = Logic.PopFirst(DialogueToEnact);
        }
        CurrentText.BbcodeText = ("[center]" + DialogueToEnact + "[/center]");

        CurrentText.SetGlobalPosition(new Vector2(TextPoint.Position) - centre);
        CurrentText.ShowOnTop = true;
        //CurrentText.SetIndexed("position:z", 100);
    }

    public void AddNPC(Node NPC)
    {
        NPCstack.Add(NPC);
    }
    public void PopNPC(Node NPC)
    {
        if(NPC == targetNPC)
        {
            CurrentText.Clear();
            CurrentEmote.Texture = null;
        }
        NPCstack.Remove(NPC);
        GD.Print("Npc stack @: " + (NPCstack.Count));
    }

    /* EditorSelection editorSelection;
    public Godot.Collections.Array returnSelection()
    {
        return editorSelection.GetSelectedNodes();
    }  */
}
