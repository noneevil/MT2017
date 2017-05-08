regsvr32 /s /u ..\Flash.ocx
regsvr32 /s /u ..\OfficePrintAddIn.dll
regsvr32 /s /u ..\FlashPaperContextMenu.dll

reg delete "HKLM\Software\Macromedia" /f
reg delete "HKCU\Software\Macromedia" /f

reg delete "HKLM\SOFTWARE\Microsoft\Office\Excel\Addins\OfficePrintAddin.Connect" /f
reg delete "HKLM\SOFTWARE\Microsoft\Office\PowerPoint\Addins\OfficePrintAddin.Connect" /f
reg delete "HKLM\SOFTWARE\Microsoft\Office\Word\Addins\OfficePrintAddin.Connect" /f

RD /Q /S "%USERPROFILE%\Application Data\Macromedia"

..\fpdriversetup u


