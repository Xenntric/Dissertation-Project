---
Previous Notes: 
---
### <<[[CTP Notes 22-04-04]] || [[CTP Notes 22-04-06]]>> ###


# CTP Notes 05-Apr-22

### Goals For Today
- [x] Start picking apart the file strings into lines, text, and actions
- [ ] Start including flags for variables within the files

### Progress Notes
Ok, today is the day I actually have to create the regex parser.

To answer a question brought up yesterday, I'm sticking with the files being read in as a string, then dissecting the string; regex allows me to pick the lines apart line-by-line, which was the only upside to keeping it as a file anyway.

Okay, that actually wasn't as hard as I was expecting.

This is the file it's meant to be reading;
![[8FA17D93-5D6D-4011-B5A8-E7A378CF6DDE.png]]

The initial regex attempt worked great for the initial section of the text, however when printing the 'emotion' section it included the `|` character.

Code;
![[0F0A80EB-7458-441C-84D2-2FA1C2279F41.png]]
Result;
![[A0CDCB9F-37F6-4B49-866A-B6A11370A731.png]]

After a lot of trying, I couldn't get the regex to match whilst excluding the `|` and so I gave up and constructed a function just to remove the first character

Code;
![[01050B91-01B1-4391-9E0C-3D464689BDB6.png]]
Result;
![[55DE960F-3C8C-416A-8E8D-49BAD690F782.png]]

An inelegant solution, but at the moment this is just what needs to get done for now, the next step is to compare the string to a series of if statements to pair the emotion with performing an animation.

Ok, so after some testing, the whole thing crashes if you don't include that pipe/ emotion. If you add a pipe later, then it includes all text up to that line in one fell swoop, as it doesn't stop for new lines.

I will have to add a lot more checks to see:

- Break string up into lines (only search for endlines)
- Check each line for text (start of string til emotion)
- Check each line for emotion (pipe to new line)
- If it has a pipe, load the emotion display function
- Else just print the text

From this, I can go into different text effects for the speech, and triggering character emotes as the emote sign appears next to them.

Spent some time reorganising the Parser, now it can:
- Count number of lines
- Break files up into individual lines to read
- Check if those lines have an emotion indicator
- If yes, print text and emotion/ if no, print only text

Code;
![[E8A47D41-9146-4E86-A27F-21B820DD3106.png]]
Result;
![[588D6A37-DF95-434C-9C5D-830D50F274DB.gif]]

This is a good start, but I think the next big step is getting those global flags and start reading lines sequentially.

Some tools I would like are:
- Conditionals (indicate line is a question, answers are denoted with `*` before text)
- Loops (dialogue will loop sequentially until conditional (can just loop forever))
- Bools (has player chosen this option)
- Ints (append some value in the world with new int, i.e. give gold)


#### Personal Comments


#Note #CTP #Apr-2022