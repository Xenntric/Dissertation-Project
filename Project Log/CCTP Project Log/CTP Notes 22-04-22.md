---
Previous Notes: 
---
### <<[[CTP Notes 22-04-21]] || >> ###


# CTP Notes 22-Apr-22

### Goals For Today
- [x] Refactor Code
- [x] Finish Integer Conditionals
- [x] Start Integer setting
- [ ] 

### Progress Notes
Okay, I haven't kept amazing progress of today, so this will be a little all over the place;

##### Functions in a conditional path

Previously, you weren't able to use a function after a * but now you can;
![[CF6BCD53-1D77-4EAC-A946-4442BCE2D3B8.png]]
![[6DCD2ECD-122D-49D3-838C-994B49DD9F8B.gif]]

##### Removed All Regex syntax checks from main Class into own Class
'Has' checks are now been moved into their own function;

![[4A1D0AEC-0C2B-4B0B-A94E-216618B76C2F.png]]
![[B94082FE-B3B8-4AA6-9613-33CCEEC4835B.png]]

'Capture between' and specifically 'Capture parentheses' now has their own functions;
![[C1559E6E-D18A-47BB-B4D5-32FAEAB1929B.png]]
![[69CD46F4-16BB-495B-B796-617B443DDDA7.png]]
It was important to move the 'Capture within' feature into its own thing because it was massively complicated and difficult to read every time.

##### If an animation is already playing, it'll overwrite the next one
This has been fixed by adding a line to stop animations before the next one plays;

![[E650C120-F824-49F8-AEA0-7C94EB36D154.png]]

![[B32B4688-D895-4016-8F6F-3A689F96E68C.gif]]
(Not a great example of an interruption, but the animation would not have played that long otherwise)

##### Popping First, Last, and removing Whitespaces have their own functions

![[4A7510A8-198D-4971-8414-7714BDECB2B8.png]]
Previously there were two functions, in different classes, that did the same thing. It was awful. Now if you need to manipulate a string in a specific way, you can do it modularly
![[CA5E7C42-0BA4-4372-A2DE-0425B9D38E13.png]]

##### Integer Checking
You can now check an IF conditional with against an integer;

![[590CD168-5605-49EE-A1DB-B1C91DA29B13.png]]

![[840099DE-46F4-494C-9AA8-7D337C9B76F4.gif]]

![[36D18F28-51AF-4200-AC6D-516F2C6CCCEC.png]]![[9BC4CF94-3068-4943-9545-4CBE6985507A.png]]

It's pretty verbose atm, but it did bring up an interesting bug;

##### Will set readLine based on final variable
If there were multiple conditions listed, it'd loop through all of them to check the condition - good!
But once it matched properly it would continue, and then readLine would be set to false, and it'd skip the conditionals.

This has been fixed by adding `break;` when it matches properly.

##### Discovered why it loops multiple times
Occasionally in the Output it'll give a bunch of 'TimesTalkedTo' readouts;
![[F40BAD8D-14F2-49EF-9D17-49B23E4DB2E4.png]]

It's because it skips that number of lines, to read the next correct line;
![[FCF11122-5940-463D-8E80-8EB99BF1BE7D.png]]

#### Personal Comments
Setting integers crashes the thingy because there is no character name set atm. I should probably just make it so if there is no character specified, it just assumes the current NPC.

#Note #CTP #Apr-2022