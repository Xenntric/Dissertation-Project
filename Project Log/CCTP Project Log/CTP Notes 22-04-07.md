---
Previous Notes: 
---
### <<[[CTP Notes 22-04-06]] || [[CTP Notes 22-04-08]]>> ###


# CTP Notes 07-Apr-22

### Goals For Today
- [ ] Finish Emote Generation
- [x] Get Emote to appear on screen
- [x] Get Emotes to appear next to the NPC talking
- [x] Get Emotes to size relative to the NPC
- [x] Get lines read line by line
- [x] Advance lines
- [ ] 

### Progress Notes
Okay, with a lot more temporary measures, the sprite appears onscreen
Code;
![[831F1F42-5D8B-43D8-A7A8-0A154092C8F9.png]]
Result;
![[DB26B273-6A53-4BE8-85E2-B85F77E56647.png]]

Currently, the function is not set up to handle placing the emote next to the NPC and would need some refactoring.

Refactoring actually took a lot less effort than I expected, however, the functions are becoming more and more bloated with not-great practices;
![[680727C1-9A94-4BC8-86FC-94DA8B9D9F1F.png]]

![[B98508CB-7CF1-4431-A612-70E60FE77077.png]]
![[7DA56181-C961-4A0E-BF18-C080553C797C.png]]

Functionally, Parseline just checks for whether the text has an emote and returns the line back in two parts.

If there is an emote, then it moves onto MatchEmoteToImage, which takes the NPC node and type of emote. The NPC's position is stored, and it constructs a sprite just off the centre of the NPC.

Initially I ran into a problem where the sprite would be behind the NPC, and made the conscious choice to bring the Player's Zindex forward 1, and setting the sprite index to the NPC's + 1.

The result is pretty neat;
![[1AD7C45E-ED0C-4B6B-B70C-648EAEB597AA.gif]]

A clearer way of explaining the relation between text and sprite;
![[90C95457-8825-4CE8-8D7B-55BDFACD1EE3.png]]
![[E030FCF9-4368-44BE-92EE-ECAB17F10DA8.png]]

![[E1992E5F-99E9-4823-BFC7-B4D1081C43C1.png]]
![[4AA32F31-4DC4-4760-9695-89F9202D2ECF.png]]

![[18308F6F-AEBD-4B47-AD52-B624900461A0.png]]
![[A90EB43B-17EC-4159-BA52-7F9F61999746.png]]

Unfortunately, the positioning of the sprite is fairly inflexible, and I should really construct a tool for the user to manually select where the node should go.

After hours of trying to get a tool to work in the editor, I just couldn't. The documentation and forums are all in GDScript, and trying to find a way of getting functions to run in the editor was just a colossal pain.

Eventually I did find unofficial C# documentation for getting the [EditorSelection](https://paulloz.github.io/godot-csharp-api/3.4/Godot.EditorSelection.html) and [EditorInterface](https://paulloz.github.io/godot-csharp-api/3.4/Godot.EditorInterface.html#Godot_EditorInterface_GetSelection) classes, however I couldn't get any class/ function that used them to run when the game wasn't running.

It wasn't even a case of it crashing or refusing to run, it's just nothing happened;
![[8A575143-5BD1-4D20-ACFD-A4ABFA40008D.png]]
![[3CD03067-21E6-4236-B54C-D928A3B1F7EF.png]]

Ultimately, I think I'm going to just attach a 'TextPoint'/'EmotePoint' node to position them.

That path did turn out to be the easiest way of doing things, it's unseemly, though effective.

The next step is starting to iterate the lines, however to do that I need some way of connecting NPCs to lines read, and a colleague has made it abundantly clear that my current code is not scalable.

NPCs will need to be given their own class, this shouldn't be too hard, though I am annoyed I need an extra file, though this really can't be helped.

It's about a couple of hours later and the refactor went well, ultimately the code results in the exact same thing, however it will be much easier to keep track of individual NPCs;

`DialogueManager.cs`
![[076D8DD7-BC3D-4C8C-8938-65509D735744.png]]

`NPCs.cs`
![[6A1AFECA-8BE3-492D-B58C-042037DD37D4.png]]


Okay, I was having some trouble when it came to the lists, but I finally got it sorted;

`DialogueLoader.cs` now has a new function which is suitably related to loading the Lines into the character, searching for endlines and capturing them into a list of separate strings;
![[4A09533D-3A71-4520-AD46-DF279752D0D6.png]]

`NPCs.cs`'s Line list setter had to be messed around with, but now it works perfectly;
![[FE012078-8EBE-4316-B00A-C763AF2DF942.png]]
Result;
![[7AF83282-FE36-4397-85C9-EAF89ED2B8FB.gif]]

Characters will now increment the lines they read, finally.

This was actually a surprisingly big step for the project, and thankfully required minimal refactoring, though with the number of smaller changes/ catches/ baby proofing I will need to do for Loops, Bools, Variables, later on.

A known issue is that the emotes don't disappear. 

#### Personal Comments
I wonder if this works with gifs? It may need to be an array of still frames? I should test that.

I know I say this every time, but I really am happy about the project, I now almost have a solid week of daily work, and it's progressing quite fast.

#Note #CTP #Apr-2022