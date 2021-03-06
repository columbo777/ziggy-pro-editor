# ziggy-pro-editor
Automatically exported from code.google.com/p/ziggy-pro-editor

Introduction

Aka, Building a PRO custom track for a custom CON song for RB3. (and thanks for that!)
Details

Building a PRO custom track for a custom CON song for RB3 (again, thanks for playing!)

So, you want to add PRO guitar to RB or CUSTOM tracks? Here is the process to do this, based on build 70/11 (stable) of the tool. NOTE: We are reopening maintenance on the tool in Q3.

Prep Work Copy the track (CON File) to a USB stick via the XBOX. NOTE: If you don't have the CON file, you need to EXTRACT the track you want from your XBOX. There are a number of tools to do this. I have the USB HD to PC cable, and I can extract using Horizon or 360 Content Manager v3. Alternatively, you can copy the track or pack to the USB stick. Just get the CON to your PC.

Optional but very useful! On a PC, open the stick using Modio or Horizon (better), or tool of your choice and extract out the individual files. You will need the .mid file, the songs.dta file. Since the .mogg is encrypted, we'll use a work around. i.e. YouTUBE.

An audio track is incredibly useful. I prefer the YouTube? version of the song in-game because it eliminates tiny timing differences that can occur between the game and the studio recording. Use a YOUTUBE to MP3 webssite to capture yourself a file... In the editor, There is an "MP3 Start Offset" field that allows you to insert a value in milliseconds (-4000 is a good guess) to adjust your MP3 to the first note of the song so it syncs up like the mogg file would. Once you get that squared away, it will be time to get to work!

Start the actual creation process: 1. Open up the Ziggy editor (v71). 2. If you did not extract necessary files from the CON file above, Load the CON original track into the editor. NOTE: This is a 2-step process. Otherwise, skip to step 3.

    a. Click the Package Editor tab and OPEN the CON file. Extract the song.dta and the

    <title>

    .mid file b. it is useful to also extract the songs.dta file 

3a. NOW, From the TOP menu, select File -> Open -> Open Guitar 5 midi 3b. Now press the "CREATE From Open Midi" button. Various fields should populate in the editor.

4. Next we Create the “empty” Pro track that will contain your pro guitar upgrade.

    a. Go to the Track Editor Tab. b. Click the button, “Init from 5 button”.

        Various fields will populate based on this creation. 

NOTE: The created file will be the pro file for the track.

    c. SAVE this file as the PRO and then on the songs tab select this saved track as your pro. d. If you extracted the songs.dta file, the songID box should be populated with ID and difficulties. NOTE: Due to a BUG, non numeric songIDs do not auto-populate (until fixed). So, open songs.dta and copy/paste the SONGID from that file. For short name, just remove any versioning or author initials so it sorts correctly in game. Manually choose your difficulty of play (best to match what the original track difficulties were rated as) 

d. Supply the MP3 file (optional) to play as you review and play with offset values as necessary (a studio song is typically a -4000 entry here) e. decide to follow along with MIDI or MP3 (or even both) and select the appropriate checkboxes. g. Hit the SAVE button.

click the tiny PLAY button on either the first tab or the note editor tab and watch the editor play along with you. (Are you getting just midi notes? Check the boxes at the bottom of the first tab and see if you are playing back mp3)

5. The Editor should now have a PRO track with dummy open notes in it (step 4b). You can see what it has done by navigating to the Track Editor tab and scrolling the top navigation bars to a populated section. Note: You are always going to chart the EXPERT guitar track. (see radio button on right of TRACK TAB, set to EXPERT). 6. charting the songs: There are 3 ways to chart the notes...

    a. CHART (place each note just like with EOF) best method. b. PLAY CAPTURE (MIDI instrument and MIDI to USB cable required). c. Play capture can be note by note or real time. 

NOTE: Despite these multiple supported ways, we most always want 6a.

7. On the Note Editor tab you now build your pro track. You will place notes based on the location of the 5 button notes. They are to be the same "duration" (or width) of the 5 button notes. If you click on one of the dummy pro notes, it will make that the current note.

a. Double-Click a PRO note (the first note is a good start) b. In the columns in the far left, you can build the note or chord you want. The notes will go from High E (Top) to Low E (bottom). (Remove default zeros if you double clicked the note to start). Note the CHAN column which later will allow you to set X notes... That is for later...

For example, if I want an OPEN A, I fill in top to bottom 0 2 2 2 0 and leave the Low E blank (blank is NOT open, it means skip. 0 is open)

This is a good time to SET the ROOT NOTE marker (this tells the game what to display as the note on the screen). Toggle the correct box and select the correct note from the pull down selection. if its different from what the editor thought it might be, you want to resave that note with the new marker (click [+] again). Try to get that right BEFORE using copy pattern to propagate notes or patterns.

c. You need to careful as the note being edited is the one CLICKED (and in bold). When you define the note/chord you will press (or click) the + key and the note will update on the upper portion of the screen. NOTE: The cursor will jump to the NEXT note automatically. d. I recommend creating the first verse and saving the work for testing at that point. e. You probably have not noticed yet, but you are building a standard pro GUITAR track. You can navigate to the second tab and switch to BASS and PRO_BASS to chart that track. Please chart Bass also and avoid the "copy guitar over bass" track which was for vs. games that never materialized. No Bass makes columbo cry.

At this point, press play and watch the editor run through your pro along with the 5 button. Is it going well? SAVE each time you are happy with your progress! (File, save, pro midi)

7. When you think you have completed your PRO, its time to COMPILE. a. on the SONG TAB, find the "REBUILD" button (to the right of CON Package:) and build it. The program runs auto checks to insure RB3 compatibility. If there are warnings, you can click the "check" button next to it and get a readout of your mistakes. If you fix mistakes, hit REBUILD before CHECK again or it will undo the changes! (bug?) b. Make sure that copy guitar to bass in UNCHECKED if you built your own bass track or it will be destroyed. also keep those auto-generate difficulties checked unless you want to do all those reductions by hand. The algorithm for reductions is pretty solid and game compliant.

8: COPY notes: When you chart a note or sequence of notes that is going to occur multiple times in a song, you should COPY it throughout the song. This works best when the 5 button is well done. Highlight your notes pattern (in the pro window) and then click the magnifying glass under "copy pattern". WOOOSH.

Editor Functions The editor was built for speed. To be efficient, you will have to memorize the keyboard shortcuts (often denoted in the editor) and master search and replace (below).

Here are the most commonly used features:

1. Copy, find and replace. The holy grail of the editor since so many sequences of notes will repeat. This will save your hours of single note plotting. a. PROPAGATING a note or string of notes. i. Highlight the note or sting of notes you want to be the same everywhere in the song. ii. Under COPY PATTERN, hit the magnifying glass. It's that simple. iii. The options underneath the copy box allow for less or more control over the pattern matching. For your own sanity, LEAVE Forward Only Checked at all costs.

REPLACING (much more difficult to get the hand of, and I stopped using it) i. Select the note pattern (mouse grab). Use Control Key if you need to scroll and continue adding notes. ii. In the left note box, change the note to the new desired notes and hit + b. Next, let’s use Find/Replace a Chord function. i. Take your “to be modified chord” STORE it by selecting it and then using the Store Chord (or G-key) in the CHORDS box on the right. (in my case x57xxx). ii. Somewhere in the chart, create the desired Chords (in my case xx023x). iii. Select the new chord. iv. Use the Copy Pattern function to propagate the change. c. Finally, lets replace an entire sequence i. FIND the string you wish to replace. (in my case, an open G followed by 7 open Bs). ii. Now edit that string to be what you would want it to be (for this case 67777777 A string) iii. You can now use the copy pattern < and > buttons to look through the song and see the matches (which are based on the 5 button). Navigate back to your string in step ii. iv. Click the Replace Notes button under the Copy Pattern Box. ctrl-Z to undo whatever you might have just done!

2. Creating the lesser difficulties : Done via Auto creation using the GEN DIFFICULTIES button which utilizes finger reduction to simplify the gameplay. On the TRACK EDITOR tab, you generate lesser difficulties AFTER completing the Expert track.

To shrink or increase the note font in either of the scrolling screens: From the NOTE screen, use – or + (not on the number pad) to increase or decrease the note fonts size

APPENDIX A Certain note type construction using the Modifiers tab

when creating arpeggios (aka broken chord) use the 'add ghost notes' check box to automatically add helper arpeggio (inversions work the same way). NOTE that due to a bug, there is no way to undo these, so if you want arpeggios, do them LAST.

SOLO sections: You WANT to accurately chart the ENTIRE solo and let the reduction process take care of the rest. Dont just phone in some silly dummy notes.

FIN When you are all done and your message is "Rebuild OK" you can now add the PRO to your USB stick and test in the game. If it does not show up in the game, you either forgot to delete your cache, or you fat-fingered the SONG ID. 
