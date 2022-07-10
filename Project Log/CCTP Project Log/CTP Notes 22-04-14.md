---
Previous Notes: 
---
### <<[[CTP Notes 22-04-13]] || [[CTP Notes 22-04-21]]>> ###


# CTP Notes 14-Apr-22

### Goals For Today
- [x] Implement 'IF' conditional in text
- [ ] Plan out questions
- [x] Design a way to set/change variables previously initalised
- [x] Implement 'SET' function
- [ ] 

### Progress Notes
Okay, the next step is implementing the 'IF' conditional I was discussing yesterday.

This is going to be kind of annoying as it will need some way of looking ahead for answers, and more importantly, discerning what not to do if a condition hasn't been met.

Ink uses tabs to discern what to do, and I can't remember what YarnSpinner does (I'm not sure whether it actually does any sort of internal calculating). Either way, I'm going to have to decide, and I think I am going to try to go with tabs? In my head, it will work something like;

```Example_of_Dialogue_Txt_file
Text | emote
Text
IF(HasTalkedToJam)
	Text | emote
	Text
ELSE
	Text
Text | emote
Text | emote
Etc.
```

So it will be identifying 'IF' key word, then following each line that has a tab until 'ELSE' keyword/ until the line doesn't start with a tab. 

This system won't be able to take advantage of nested `if` statements, but I think that is just a concession that will need to be made for now.

I will need to use an `IEnumerator` at runtime to check for the right lines to say.

So if the command 'IF' is matched and conditions met, read the next line without the tab until 'ELSE' is matched, then skip all tabbed lines.

If the condition is not met, skip all tabbed lines until 'ELSE', then read the next line.

Oh god, I will also need a 'SET()' function within the text to change variables. God damnit.

'SET()' will need to interact with other scripts. So you will need to say like `SET(Brick.HasTalkedToJam = TRUE)`. That's not a terrible system, but will need to be able to cycle through every NPC's cached Bools/Ints/whatever.

It is now a lot of hours later, but I now have the 'SET()' function complete and accommodating of both booleans and integer types;

![[19BD0345-DCC5-4D64-92D4-530799EF6550.png]]

This function sucks, it's extremely costly and will probably not run at scale, but my god it works.

This is the steps through how it works
Brick initialises the bool;
![[CEEB483A-27FC-43AC-B90E-EBAB41FD694C.png]]

In Jam's text file it will check against the '--SET', then divide the string into the character, variable, and what the variable should be set to;
![[39D5F90B-DEA9-41EF-A92C-18D81F8F1669.png]]

The debug log of talking to Jam and parsing the 'SET' line;
![[29EE83B4-D421-4322-8CBF-28E25CB6F317.png]]



Next is the 'IF' conditional, which should be simpler(?)

It was not.

My coding style has massively fallen apart as I was trying to implement two big features in one day, so it's extremely messy at the moment, but it works.

`DialogueManager.cs` now has a condition for 'IF' variables;
![[439FED66-A270-4F37-9BA3-C60868D7EC80.png]]

Highly inefficient and currently doesn't have any code for comparing integers at the moment, but the logic is sound.

To implement this, I had to add lots of little catches and tweak existing functions to get them to work right, which is bad practice, but it functions now;
![[09318444-9398-41C4-BFDF-036A25747852.png]]

Tweaks like adding a catch for if the string is null or has one of the markers for being an answer (which should really be parsed immediately);
![[95F89B76-6FC0-41AD-92DC-9E1D3BAF278C.png]]

And a tweak to make removing whitespaces optional;
![[411A9637-6335-4F55-9CBC-2D1C5CF90078.png]]

With terrible coding practice aside, the results are, mostly, very sound.

Not talking to Jam first;
![[C5D96AFD-3DB1-496B-8724-12245F073873.gif]]

Talking to Jam first;


![[3DEFEC37-72F1-408B-8605-034EE3C09321.gif]]

And the text files controlling them;
`brick.txt`
![[6C7A2CCA-5921-44B4-BDD9-057982331A1C.png]]
`jam.txt`
![[14ACCB6D-0BFB-4BD6-A435-2BE5C8C4E312.png]]




#### Personal Comments


#Note #CTP #Apr-2022