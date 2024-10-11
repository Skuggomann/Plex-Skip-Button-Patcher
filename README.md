# Plex Skip Button Length Patcher

This program edits the Plex web player code to change the skip button skip length from 10s/30s to whatever you want.

* The program takes 0, 1 or 2 input parameters as integers
	* 0 parameters: 	Changes to 5s/5s
	* 1 parameter:	Changes to [Parameter1]s/[Parameter1]s
	* 2 parameters :	Changes to [Parameter1]s/[Parameter2]s
* The input parameters can be added to the end of a shortcut target to the executable like so: `"C:\Plex Skip Button Patcher.exe" 5 10`
* The program requires admin privileges because of the Plex file permissions on the default install
* The program just removes the index.html script integrity checks instead of updated them with the correct checksum because it works

