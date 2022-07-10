---
Previous Notes: 
---
### << || [[CTP Notes 22-01-22]]>> ###


# CTP Notes 21-Jan-22

## CURRENT PROGRESS
- Created Godot Project
- Switched to Godot Mono (oops)
- Implemented character into scene
- Placed characters to interact with 
- Character movement now working

### Progress GIFs
![[CBD5CD22-8162-4EEB-A534-5B36E5779F12.gif]]
(Character animation working, Character idling and blinking)

![[9E19E581-E4A8-443B-84B6-F061E51D4B9F.gif]]
(Character walking animation in progress, making use of Godot's state and blend machines in its animation window)

![[26DD2929-E7CB-415E-9C58-DFFE50974EE3.gif]]
(Character movement finally into game)

#### Personal Comments
Oh my god Godot's C# documentation/ integration is lacking, I am coming from a unity background and expected this to be different and that's ok, but the amount of trial and error I have to go through is agonising. Constantly dealing with 'this is depreciated' errors in my code and rarely given suggestions on what to replace it with.

I debated switching to GDScript when I was writing the player manager because i was very frustrated however I decided against it as the majority of this project is system based and C# would be the smarter choice.

Godot also only allows one script on an object, so I can't have a neat 'Scripts' node like I want.

I have no idea how Godot's C# 'FindNode' instruction works, as far as I can tell it doesn't, the only documentation on the website is for GDScript (Godot's own programming language), and only mentions that it is a slow process, like, I know, I dont care though, just let me use it. 
Instead I have used Godot's 'GetNode' instruction, and i need to declare the full filepath of the object I am referencing like the 'KinematicBody2D' or 'Skeleton2D'.  

Godot's Animation suite is actually pretty good, being very flexible with combining or triggering animations. Instead of Unity's 'Layer' system, Godot has a very easy 'Add2' or 'Add3' node to achieve that process. Very happy.

It's looking like this may have to be a Script-Based dialogue editor, as you *can* create new suites in Godot, but apparently it involves c++ coding and this project is big enough already.

Decided to do this project on Linux, because it seemed fun. I might write about how I was doing it as an extra challenge for this project, to try using a different OS? But to clarify; Godot does work in Windows and Mac and this project would be perfectly do-able in any OS.

## To Do list
- [ ] Explore more into the 'FindNode' command in Godot because it'd be very useful to find characters in the scene and cache them for the script writing process instead of manually entering it
- [x] Need to establish text coming up after being loaded in from a separate .JSON/ .txt file
- [ ] Google/ learn Regex
- [x] Establish a Standard for how to write Scripts to read
- [x] For this demo i am only looking at getting text boxes to be read and presented


Ideally Dialogue will read as

\<Speaker 1> : Hello there! I hope the demo is going well! | !

\<Speaker 1> : and they're happy with your progress | <3

#Note #CTP #Jan-2022