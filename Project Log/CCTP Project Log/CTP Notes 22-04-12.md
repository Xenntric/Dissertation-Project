---
Previous Notes: 
---
### <<[[CTP Notes 22-04-10]] || [[CTP Notes 22-04-13]]>> ###


# CTP Notes 12-Apr-22

### Goals For Today
- [x] Create Regex Class
- [ ] Implement Regex variables
- [x] Make Backend more scalable
- [x] Add switch case based on regex matches
- [ ] 

### Progress Notes
Today I'm going to start on getting the in-text-file commands working, however to do this I will need to greatly change how the line reading system works

Currently, the line reading system is fairly linear, broken up into stages that only account for if the line has dialogue and an emote or just dialogue. Unfortunately, this means I will need to add checks for if the line has a specified variable.

First, creating the regex class full of the patterns the dialogue manager will check against, I will be implementing the variables listed in the previous note and taking out all the regex checks from the dialogue manager that check for patterns;

`RegexCheck.cs`
![[2537EBF4-5266-4DBA-880C-2B06BBD5852A.png]]
![[69C89A8C-6BAD-4BBB-B363-6BF3A7F79348.png]]
`RegexCheck.cs` contains all the variables I could think of so far and also has the checks for if the line has an emote, as well as what the dialogue and emote are.

Okay, now I have to overhaul how I parse lines.

It's a couple of hours later(?) and the 'ReadLine' function now checks if there isn't a text file associated with the character.

'ReadLine' also now establishes the Lines and lines that are parsed very differently;

![[FCBCE569-8269-4BEC-B3FB-04AE1276C633.png]]

'SetEnumerator' now handles logic for parsing lines, setting the enumerator to different modes based on the content of the line;
![[4D431EDD-1EE4-4A29-8EAA-80D797BBEE2D.png]]


After the enumerator is set, logic for what happens based on the line is now in a big, flexible switch statement. Depending on what the line is now it will display both emote and dialogue, just the dialogue, or end the reading the text file;
![[2AEC744A-ED82-4C62-9606-1307E883C8BF.png]]

The result isn't much different from the previous note, however now if the program can't find an associated text file, it won't crash. Additionally, if the player continues to talk to the NPC, the program won't try to read a line that doesn't exist and crash;
![[C15D078D-6929-4B57-B669-301DC54383BB.gif]]

Today was a lot of backend development to set me up for introducing the logic for more variable, but this is the final stretch; after the global variables for the scene are set up and the dialogue manager can act on them, I will actually be more or less finished!

I only need to get the variables in, pairing them with global variables, and then getting questions and answers working. I expect Q&A is going to be a massive pain and I only really have like 3 days with my current schedule.

That's worrying.

#### Personal Comments


#Note #CTP #Apr-2022