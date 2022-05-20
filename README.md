# Ipnce-Editor
Animation editing tool for mobile ports of Ace Attorney games (AJ, AAI, AAI2)

# What is Ipnce and how does it work?
As you may probably know, Nintendo DS uses four types of files to draw an image properly:
1. NCGR (an uncolored tile image)
2. Palette (it's used for coloring an image)
3. NCER (a file that is responsible for placing tiles in roght places)
4. NANR (file that contains information about sprite animations)
Since AJ, AAI and AAI2 were originally made for DS, Capcom decided to port this drawing system into Unity mobile ports of these games.<br>
This drawing system has 3 files that has the same purposes as in Nintendo DS:
1. TexIdxEx (the same as NCGR except for a fact that titles look normal)
2. ColorPlteEx (it's a palette image)
3. Ipnce (a file that has the same purposes as NCER and NANR at the same time)
This system can be seen in lots of textures in mobile port. You can find the textures with the names like "XXX_TexIdxEx" and "XXX_ColorPlteEx" and the monobehaviour file with the name "XXX". (XXX - is the name of texture). This "XXX" monobehaviour file is Ipnce.<br>
These three files are parts of this sprite drawing system and are used with each other. Sometimes these sprites are already colored and instead of "XXX_TexIdxEx" they have name like "XXX_Direct16". These files do not have a color palette for them, but they have an ipnce file with them.

# How can I find these Ipnce files?
You can use Assets Bundle Extractor for looking into the contents of assets files inside game's cache.<br>
As I told you previously all TexIdxEx files have an Ipnce for them with similar name. For example, if you want to find Ipnce for etc0019_TexIdxEx look for Monobehaviour etc0019 file.
When you find the Ipnce, extract it by pressing extract raw. Then you can open extracted file with Ipnce Editor.

# How to use Ipnce Editor?
1. Press Open and choose Ipnce file, tile Image and color palette (if the program needs it).
2. Edit ipnce any way you want.
3. Save an ipnce.

# I want to implement an animation from one AA port to another. How can I do that?
While saving an Ipnce you can press Save As "the game you want implement it to" button. The program will transform Ipnce to the format that is used for the game you need.
But after that, you will need to edit the Unity file header data with some hex editor. For example, if you want to copy "REBUTTAL" animation from AAI1 to AAI2 you must: 
1. Press "Save As AAI2" button and save ipnce
2. Then open original "REBUTTAL" ipnce from AAI2 and copy header bytes from it
3. Replace header bytes of the file you saved with the bytes you have copied.

# TexIdxEx textures are uncolored and trying to redraw them with saving color palette is a mess. Is there any way to do that easily?
You can use Palette Applier () tool to add colors to the TexIdxEx image that you want. Then you can edit this colored Image and then uncolor it back with Palette Applier by pressing a Decolor flag.
