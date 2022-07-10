---
Previous Notes: 
---
### <<[[CTP Notes 22-04-05]] || [[CTP Notes 22-04-07]]>> ###


# CTP Notes 06-Apr-22

### Goals For Today
- [ ] Get Regex counting every line that actually has content in it
- [ ] Establish a syntax for user-dictated Bools, Strings, Ints, and Loops
- [ ] Get Emotes to appear next to the character
- [ ] 

### Progress Notes
After a lot of deliberation, I'm first going to target the emote images that will appear next to the character when talking to them.

I have three emotes already, and in my head you will load these emotions into the in-scene script of `DialogueManager.cs`.

Again I'm running into a problem with wanting to make this a really generalised tool however it's requiring a lot of 'you have to do it this way', I guess this is something that would be helped by testing however it's looking like I won't be able to get around to that.

Choosing to focus on the emote manager first, I've made it so that the user needs to expand and add images for emotes that they want to pop up;
![[07B519BC-7DD4-4BF2-9336-96D200B423FC.png]]

Next, I tried to check simply for the 'emote' variable matched the image's filename, however that didn't succeed;
![[2A8D63D2-4D64-4EDB-86D6-07616634D6EB.png]]
![[8FBE38B9-92DE-42EA-B15A-875296698F42.png]]
The resource name is functionally useless as I have no idea what the 'StreamedTexture' number is going to be named, so instead I added some further RegEx checks for the file path string. 

After it finds the file path, and adding '.PNG' to the end of the emote variable (as a temporary measure), it compares the strings if they're identical.

I also needed to remove the white spaces around the emote string to make it more robust.

![[ACF7538B-D971-4336-B180-165C0872D456.png]]
![[E099DF11-16E4-4241-9886-8372BDF32FAA.png]]

![[89F988C6-26C1-463D-816A-7F6F86AA4C00.png]]

Next, I need to make the StreamedTexture into a sprite that then appears next to the character, and freeing that sprite upon the next line.



#### Personal Comments
When it comes to rendering the text on screen, I'm going to want to let the user pick font, size, and space.

#Note #CTP #Apr-2022