---
Previous Notes: 
---
### <<[[CTP Notes 22-04-07]] || [[CTP Notes 22-04-09]]>> ###


# CTP Notes 08-Apr-22

### Goals For Today
- [x] Finish Emote Generation
- [ ] Get text rendering on screen
- [ ] 


### Progress Notes
Only one sprite is generated at a time now.

This entire day was trial and error fighting with RichTextLabel to get text rendering on screen.

So Godot has two text systems; Label and RichTextLabel. Label is a simplistic text system where you can assign a text box and set the contents as well as how it is formatted; it's fine, okay, alright, suitable.

Whereas RichTextLabel can do everything regular labels can, and it has additional functionality like a scrollbar, fonts, BBCode which handles colour and effects like wave (think Runescape), and built-in text crawling, so RichTextLabel is the obvious choice. Getting it to run in DialogueManager has been impossible as it needs to inherit directly from Control, which conflicts other things in the class which also need to inherit from Node.

I attempted so many things trying to get it to even show up on the screen, even though it was storing all the variables correctly.

I wouldn't say today has been a waste but it is agonising that I had a week straight of good progress ruined by this brick wall.




#### Personal Comments


#Note #CTP #Apr-2022