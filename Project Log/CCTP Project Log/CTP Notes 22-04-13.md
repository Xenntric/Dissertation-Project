---
Previous Notes: 
---
### <<[[CTP Notes 22-04-12]] || [[CTP Notes 22-04-14]]>> ###


# CTP Notes 13-Apr-22

### Goals For Today
- [ ] Plan out functionality for answering questions
- [x] Implement functionality for global variables (managed in `DialogueManager.cs`)
- [ ] 

### Progress Notes
Fixed an issue where if the line had no emote written, it'd still try to set an emote image.

Fixed an issue where it wouldn't clear the emote image if there was no emote written in the line.

Global variables will have to be written as lists of variables the user will want to include in the scene; with the ability to check against lists of types.

For example; there are lists of Bools checking against whether the player has talked to Jam already, designated with syntax like `<--BOOL HasTalkedToJam-->` Where the manager/ loader/ whatever will have to identify the `<-- -->` syntax, then check against the Capitalised `BOOL` section, then check the following name against the user-set list of Bools. 

I think that's an acceptable system for setting global flags, but using conditionals based on them will be very difficult.

The only issue I can think is that with the current line Reading system, it'll be difficult to decide where to read next if the condition hasn't been met.

I guess I could use some sort of LookAhead system? Where the program will look for, say; a `*` or tab at the start of the line, denoting they are answers, and looking for the next line that didn't start with one??

The list of Global flags will not be acceptable, as there is no way to have a list of Bools with associated names, making them incredibly difficult to keep track of, antithetical to the 'easy to use tool' I aim to create;
![[F982868C-D728-4B15-B43E-2812815AF498.png]]

I'm sure there would be a complicated way of setting up an array of strings, and based on how they're named, appending to a list of tuples with strings and Bools, but it will honestly just be simpler to create a template C# file for the user to write into and then handling the logic in `DialogueManager.cs`.

If I am using global flags in a C# file, that won't be scalable across multiple scenes. 

I could ask the user to instantiate them in the top of the text file, using a tuple to pair them to a string that can be checked against? That actually may not be a terrible idea.

NPCs already have lists for both Bools and ints. I could load them in as the files are being read.

After a lot of faffing around and tea breaks and hitting my head into a wall, it works.

Initially, I was experimenting with making a generic function that would be able to distinguish and add variables reguardless of what type they were;
![[22C35CF0-5180-4CF3-9ABC-071094E08312.png]]

This did not work.
![[3CBEF364-D8BD-4C0C-BAC6-7AB2F9C58E49.png]]
I misunderstood how to use `object` and `dynamic` variables and in the end could not create the generic function I wanted.

Instead, I created two bespoke functions using methods already established earlier in the project;
![[C760ACD8-13B6-4F3D-8DD1-D920D06FFB6E.png]]

I also created this function as, similarly to the emote string, the capture system for getting contents of the variables also included the parentheses around what I wanted, and I had to make a similar function to remove excess. This function also removes white spaces from the strings, so it wouldn't matter what the user entered;
![[9C403F41-928B-4C36-A10F-C75F74DAAD2D.png]]

This is taken care of when loading the scene, and will set the NPC's 'Ints' and 'Bools' dictionaries to the contents of what the function found; 
![[D6A866B7-D7B4-4560-9380-B46E04E986E5.png]]

I also added a line, so the system won't capture any empty lines;
![[5ED2F0D7-59F9-4120-A8C0-CEB287D2DCCC.png]]

In the end, the system looks like this;
![[2BAB84A0-0839-4CA1-A190-72C8CBD5CB0D.gif]]

And Brick's text file looks like this;
![[08750FFB-D06A-432D-B642-52BDBC8FA639.png]]

This is a big step in the system, finally getting some sort of variables loaded and parsed. 

Next will need to be the 'IF' command in text, and then followed by the Questions, and then followed by commands to manipulate the character through the text file, i.e. EXIT(RIGHT) to make them translate offscreen to the right.

I also need to revisit the text box problem to remove that scroll bar appearing.


#### Personal Comments


#Note #CTP #Apr-2022