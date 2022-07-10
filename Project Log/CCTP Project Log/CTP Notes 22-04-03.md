---
Previous Notes: 
---
### <<[[CTP Notes 22-04-02]] || [[CTP Notes 22-04-04]]>> ###


# CTP Notes 03-Apr-22

### Goals For Today
- [ ] Get text file reading line by line
- [x] Establish a standard for reading text in and how it relates to characters
- [x] Create a way to read in character specific files, so they be called when next to them
- [ ] 

### Progress Notes
Godot keeps crashing in my project, I have no idea why, but it happens all the time, and it is incredibly aggravating.

After reinstalling Godot from scratch, it still happens; thankfully it doesn't happen in different projects, so I can solve this by reverting to a previous git commit.

That solved it. Hours of frustration, and it was probably because the system was looking for a file that was no longer there. Googling issues for Godot are usually fruitless.

Finally, I can now start my work and I have finally gotten it reading line by line when I press Enter;
![[B3D24907-39DC-4963-A3D5-B11F8970EC54.gif]]

I am loading all scripts in at once, and will need to establish a standard like, the user will need to call their script the name of the character in the scene. And will need to put that character under a node like 'NPC' or 'CHARACTERS' just so I can target who says what and also the position of each character.

The next step is getting a user made list of characters in the scene and their names, and getting two text files for both Brick and Jam.

Then when im over each of them, it plays their script line by line.

Ok, so my current issue is actually to do with 'well, how can you link the dialogue to whatever character you're currently on top of?' And I think the solution is that the user doesn't need to manually enter any names. Just search for the characters under the 'NPC' node for names, and then you'll also have a hit box.

Currently, the struct will load character text files in. To invoke this struct, I need to invoke it with the private 'NPCs' array, which gets the children of the 'NPCs' node.

From this I can call the text file of the character the player is over, and when pressing enter it'll spit out the body of the file.

My logic was sound!

After a long time faffing around, getting array initialisation errors I got it working, using the `System.IO.Path` tool to combine strings into a path the file reader could understand.
![[46EAD590-705A-4CE9-9D59-47648AA891BA.png]]

Making the text files into a list makes it a lot easier;
`DialogueManager.cs`
![[3146C3A8-F7DF-40E5-9FA3-7FD4E50C04D7.png]]

`DialogueLoader.cs` 
![[AB7D831D-C0F3-47AB-BA54-E6E0E0BE4C27.png]]
Result;
![[6C2A3FDB-8187-4644-A8A4-7A19397BF9D7.png]]

##### The Standardisation
Okay, so for this method to work there needs to be a node containing all the NPC that talk, called 'NPCs', there must also be a file with that character's name in all lowercase in the directory 'Assets/Dialogue/\[name]'.

I don't feel like this is too much to ask the designer? Although later I will need to break this up into separate scenes if someone wants to use this manager across multiple scenes.

#### Personal Comments
The Godot crashing issue genuinely massively frustrating, not being able to identify the issue with the project, I can only assume the project was assuming a file would be there and getting a pointer error. I pushed through as I couldn't waste another day baby raging because Godot was throwing a fit.

As much as I hate programming, breaking through that wall gives a massive dopamine rush.

#Note #CTP #Apr-2022