---
Previous Notes: 
---
### <<[[CTP Notes 22-04-08]] || [[CTP Notes 22-04-10]]>> ###


# CTP Notes 09-Apr-22

### Goals For Today
- [x] Render Text
- [ ] Add Syntax for the end of a text file
- [ ] 

### Progress Notes
I have set up another class to try to handle text generation.

IT FINALLY WORKS.

I have no idea how I was setting it up before, but it works exactly like the emote sprites.

I am not sure why I was having such trouble with this all day.

Okay, the issue I was having as that I was not defining a size for the text box to display text in, this has been an embarrassing episode of trial and error;
![[DC3B3EBF-1167-4ED5-A11F-7A0D5741B879.png]]


An issue with the text is that it renders from the top left of the rect, so it starts from the TextPoint instead of directly on top of it;
![[CFB7A92D-CE53-4C4D-9474-5E2469CEE7B8.png]]

This has been slightly fixed by getting the width of ht string based on which font is being loaded in. Ultimately, this isn't a great fix because I have to multiply the entire Vector2 by 2, and even that doesn't work in certain strings?? Will need further investigation.

The DialogueLoader node had to be moved down to the bottom of the scene as Text was rendering behind the NPCs as they were higher in the hierarchy than the DialogueLoader node. A simple fix.

This is the result;
![[9B1367DE-AA66-40B1-8A6D-0803BD056DB0.gif]]

Come to think of it, I may need to make a Z-list diagram of my scene for future Z fighting issues.

The last part of my 'must-have's, is the syntax for editing global variables, I am very close to finishing, and then polishing this system.

To testing it, I will need to start a new Godot Project and see how easily I can drop the system in.

#### Personal Comments


#Note #CTP #Apr-2022