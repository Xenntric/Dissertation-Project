---
Previous Notes: 
---
### <<[[CTP Notes 22-04-03]] || [[CTP Notes 22-04-05]]>> ###


# CTP Notes 04-Apr-22

### Goals For Today
- [x] Pair dialogue with Character
- [x] Print script when interacting with character
- [ ] Read files line by line when interacting with that character
- [ ] 

### Progress Notes
To read the correct character's file, I'll need both the `PlayerMovement.cs`'s 'current_grab', and to find the index of that character under the 'NPCs' node.

I could do this by iterating over every NPC, checking for an identical name?
I don't think this is particularly efficient, but I don't think Godot has a 'Which child is this' function. Maybe I could make that? That seems unnecessary.

In the end, I elected to create a list of Tuples that pair both the character to their associated script;
![[94E6A7BA-F3EA-4076-849D-AF54C9E8F25F.png]]

![[55FDFDC5-7569-4064-9DC8-66F7EFE60260.png]]

It has just occurred to me that I should probably be pairing these as they are being loaded, so maybe I will change the dialogue loader to export this list instead?

The manager should be the engine that checks which file to reference, and the logic of parsing that file, the dialogue loader should really be handling this issue.

Implemented these changes;
![[542AB743-D238-4B80-AAE1-C059596FF201.png]]
![[E9D42311-5FEA-487E-BC17-2720C950448C.png]]

Now I need to read the script when on top of the character. I'll need the node the player is currently on and then print the associated script based on the index of that node.

Hit a slight wall. I can't seem to import any variables from `PlayerMovement.cs` and I'm not entirely sure why, all the appropriate variables are public, and the logic is sound when performed within `PlayerMovement.cs` so there must be some issue with Godot calling another class that inherits differently?`PlayerMovement.cs` inherits from KinematicBody2D, `DialogueManager.cs` inherits from Node.

No. None of that was true. It is hours and hours later, and it was because I didn't have a nodepath to the 'Dialogue Manager' node;
![[7F96D58D-8319-4E5B-B63D-EFE4F01E4A17.png]]

Currently, I have the strings being read when the NPC you've most recently walked into the area of;
![[845531E2-AB52-4117-8757-729213F86EB2.png]]

![[4DD8B84D-205E-4856-BEAE-B13B5E64D330.gif]]

This works really well, honestly, however the issue with reading the preloaded text files is that you can't read a string line-by-line.

The way around this is either to load the file when interacted with (not preferable), or to stipulate that you need to end lines with some sort of character (still not great).

At the moment, the logic of determining which NPC to interact with is handled in `DialogueManager.cs`, and requiring the user to manually include a reference to the 'Dialogue Manager' node. As well as calling to read the right file on an action, and adding the NPC the player has moved into, and finally popping it when leaving.
![[D369D510-B051-4F7C-AEDA-B2988FFCED4F.png]]

#### Personal Comments
I think I've finally gotten into a good flow with Godot, I'm really pleased with how this is shaping up.

I wrote that before I spent around 10 hours dealing with a null reference to the Dialogue Manager script. I just didn't realise it worked like that's, and I wasted an entire day.

I'm still feeling ok about the project, but a colleague mentioned that my dialogue manager should be static. Yeah, he's right, but oh my god I'm shocked I've even gotten it working this far, and the fact that the script needs to be somewhere in the scene right now is fine.

After talking with that colleague again, apparently there isn't a great option when reading the files. The colleague personally prefers an explicit character to end the line. So I may end up doing that.

#Note #CTP #Apr-2022