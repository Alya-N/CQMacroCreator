# CQMacroCreator

CQMacroCreator is a "GUI' for Alya's Cosmos Quest PvE Instance Solver. Please download it here: https://github.com/MatthieuBonne/C-Hero-Calc

Just place CQMacroCreator.exe in the same folder as CosmosQuest.exe and run it.
![screenshot](http://dcouv.fr/cqmc202006.png)

# What's new
v4.9 - Horsemen & Halloween2020 heroes

v4.7.5 - Archers & Eternals

v4.7.4 - Astronauts

v4.7.2 - Easter2020 heroes

v4.6 - Halloween2019 heroes

v4.5a - Subatomic and gladiator heroes

v4.3b - T31+ monsters

v4.3a - Witches, aWanderer, aNerissa

v4.2b - Mother

v4.2 - Easter and aquatic heroes

v4.1g - Adjinns and new quests

v4.1f - B-day and dragons

v4.1e - Drifter chest heroes

v4.1d - Cupid

v4.1c - Fairies (S7)

v4.1a - Destructor chest heroes

v4.0b - Added Promotions

# How to get Authentication Ticket and KongregateID
Open the game and open the console(Ctrl+Shift+J or F12), choose "Network" tab.
![screenshot](https://image.prntscr.com/image/jbnEU_vqS22_fNBQDpf-zQ.png)
Without closing the Network tab refresh the game website. A lot of entries should appear. You need to find one called "LoginWithKongregate"(you can use search/filter option placed above all entries) and click on it.
![screenshot](https://image.prntscr.com/image/rq85HuDfR2qxRgmAHYx_hw.png)
On the right side you should see "Request Payload" section if you are using Chrome or Maxthon, on Firefox you should see "Params" tab. Authentication Ticket and your KongregateID will be written just there. Just copy them and paste into "MacroSettings" file(auth ticket should be in 2nd line, kong ID in line 3, don't use quotes). I included example MacroSettings file in the repository so you can see how it should look like.
![screenshot](https://image.prntscr.com/image/q9yVmqa4Rg6dnwQmfIHDIg.png)
## WARNING
**NEVER share your Authentication Ticket with anyone. If someone has access to it he will be able to do a lot of nasty stuff with your game like enter tournaments with bad lineups, spend your UM or clear your PvP grid.**

### List of all files required for calc to work
* CosmosQuest.exe - Alya's calc
* cqdata.txt - included with calc, lists all available heroes
* CQMacroCreator.exe
* Settings.json - created by CQMC and shared with CQAutomater
