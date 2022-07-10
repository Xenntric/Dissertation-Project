---
Previous Notes: 
---
### <<[[CTP Notes 22-04-14]] || [[CTP Notes 22-04-22]]>> ###


# CTP Notes 21-Apr-22

### Goals For Today
- [x] Establish a method of playing animations through text files 
- [x] Implement Animation Player
- [ ] Refactor code
- [ ] Finish the set checker for integers
- [ ] Allow user to set conditionals in answers, not just text
- [ ] 

### Progress Notes
The final implementation I want to do is to allow the user to play animations through the text files, in my head the syntax looks something like; `PLAY(animation name)`, treating it similarly to the contidionals.

In practicality, I should really make a class for functions, just so the Dialogue Manager isn't full of checks.

Actually, that wasn't super hard since I have everything set up relatively modularly, it searches for the '--PLAY' line, then grabs everything inside the parentheses, removes the parentheses and uses the AnimationPlayer the function found to then play the animation with the same name. It was actually a lot more slick than I anticipated;

Code;
![[44777D27-148B-4E2B-ACC8-F20456F036DD.png]]

Text;
![[056653FE-593C-45CE-A0B1-67213F434676.png]]

Result;
![[534F9FF8-B706-4E64-B793-2F660472D9CC.gif]]

#### Personal Comments
That's all the major functions out of the way now; now all I have to do is refactor; removing redundant code and moving all the logic from `DialogueManager.cs` into a new `DialogueEngine.cs` file.

Then comes the write-up.


#Note #CTP #Apr-2022