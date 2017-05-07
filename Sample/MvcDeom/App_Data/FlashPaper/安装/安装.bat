move license.inf ..\
RunDll32 advpack.dll,LaunchINFSection ..\license.inf,DefaultInstall
regsvr32 /s ..\Flash.ocx
regsvr32 /s ..\OfficePrintAddIn.dll
regsvr32 /s ..\FlashPaperContextMenu.dll
move ..\license.inf
move ..\flashpaperprinterdrv2.dll
move ..\flashpaperprinterui2.dll
..\fpdriversetup i
move flashpaperprinterdrv2.dll ..\
move flashpaperprinterui2.dll ..\