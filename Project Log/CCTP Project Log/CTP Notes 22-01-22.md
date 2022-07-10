---
Previous Notes: 
---
### <<[[CTP Notes 22-01-21]] || [[CTP Notes 22-01-25]]>> ###


# CTP Notes 22-Jan-22
## CURRENT PROGRESS
- When player enters the NPC's collision radius, grabs NPC's StaticBody2D Node
- Ironed out a structure for the dialogue to be read by the DialogueManager that I haven't written.

### Progress GIFs
![[AE22823C-89F1-4500-BB37-FE33E29299AF.gif]]
(Player moving in and out of the radius' of Brick and Jam)

#### Personal Comments

Getting the collision information from Godot is actually like pulling teeth. It seems every aspect of Godot C# development is woefully underdeveloped. To detect the object an area2D is currently overlapping with you must go into the node tab next to the inspector, and link the 'Body_Entered' signal to a script, along with the name of the function to trigger when that signal goes off. While I can understand the flexibility this afford the developer, the fact that it is an easily missed tab in the top right of the window makes it unnessecarily difficult.

The Godot forums continue to be unhelpful, any help being given for GDScript. The pseudo code presented is not helpful because I have already planned out the psuedo code.

As for the Dialogue Reader, I still haven't written it. Regex is absolutely the way to go and I need to get the text files actually being read into it.

I initially tried to learn Regex without a plan of how the script will go?? Which was a colossal waste of time. At least now I have a plan

Scripts will be split into three parts


\<Speaker> : Dialogue | Emotion

Example:
Brick : Hello there! I hope the demo is going well! | !
Brick : and they're happy with your progress | <3

The text document will be read one line at a time, and dialogue split into separate text boxes will be needed to entered as a new line, the emotion is read lastly for the speech bubble to appear next to the sprite

## To Do List
- [ ] Dialogue Manager will be attached to a 'Scripts' node under the 'World' node<br>Dialogue Manager will get Player, and the object its interacting with, and find the associated script with the character.<br>Text will then be parsed line by line, and converted to text on the screen.


#Note #CTP #Jan-2022