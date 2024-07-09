# SanctuarySSModManager
Mod manager for Sanctuary: Shattered Sun

**This document is a work in progress**

## Background

[Sanctuary: Shattered Sun](https://www.sanctuaryshatteredsun.com/) is a new RTS that is being developed by Enhearten Media.  
This appears to be the long-awaited sequel to the Total Annhilation/Supreme Commander/Supreme Commander 2/Supreme Commander: Forged Alliance series.

The game is currently in development and isn't expected out until 2025.

Links:
TODO:  add links to patreon/discord/community

The game has been built specifically to support extensive modding, which is done by modifying the script files that will ship with the product.

## Overview
This project is designed to be a simple modding manager (similar to [SC2MM-170710-RC2](https://www.moddb.com/mods/revamp-mod/downloads/sc2-mod-manager-release-071017-rc2))

It will allow the user to perform the follow actions:


- mod management
	- creation
	- editing
	- packaging
	- publishing
- play management
	- mod installation
	- mod activation
	- mod renewal
	- game launch


## Tasks
- [ ] Where do we publish/retrieve from?  
- [ ] figure out if we can support patching (much smaller footprint, but added complexity).  See investigation below.
- [ ] Figure out more tasks to add here

## Investigation
I really want to be able to use patching (specifically the nuget library [DiffMatchPatch](https://www.nuget.org/packages/DiffMatchPatch/)) that is based off of googles [diff-match-patch](https://github.com/google/diff-match-patch) toolkit.  This would make the mods extremely small and lightweight.

However, before committing to this I need to understand how realistic using patch files will be.  So I'm going to create a bunch of unit tests to determine how feasible this aproach is:
* test creating patches from simple changes
* test undoing patches
* test merging two different patches
* test merging two different patches (reverse order)
* test undoing two different patches (in different orders)
* test merging three different patches, and reversing patches in different orders
* others

Once I have done this I will know if this pattern is feasible.  It's not the end of the world if this doesn't work, most of the existing mods for supcom+ are in the tens of megabyte range.  And it won't reduce sizes for lots of adds (think maps).

## Discussion
[Jon] This is a free-form area for anyone who wishes to contribute to have any kind of related discussion needed.  Please put your name in brackets so we can see where the comment is coming from

