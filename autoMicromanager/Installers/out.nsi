; Script generated with the Venis Install Wizard
!include "WordFunc.nsh"
!include "Locate.nsh"
!insertmacro WordReplace
!insertmacro WordFind


!define SHCNE_ASSOCCHANGED 0x8000000
!define SHCNF_IDLIST 0


!ifndef LabviewVersion
 !define LabviewVersion '!insertmacro _LabviewVersionRetrieve'
 
    !macro _LabviewVersionRetrieve LabviewPath
					push $1
          ReadRegStr $1 HKCR "Applications\LabVIEW.exe\shell\open\command" ""
					end:
					${WordReplace} $1 '"' "" "+" $1
					${WordReplace} $1 '%1' "" "+" $1
					${WordFind} "$1" "\" "-2{*" $1
					StrCpy $2 "$1"
          pop $1
    !macroend
 
!endif



Var STR_HAYSTACK
Var STR_NEEDLE
Var STR_CONTAINS_VAR_1
Var STR_CONTAINS_VAR_2
Var STR_CONTAINS_VAR_3
Var STR_CONTAINS_VAR_4
Var STR_RETURN_VAR
 
Function StrContains
  Exch $STR_NEEDLE
  Exch 1
  Exch $STR_HAYSTACK
  ; Uncomment to debug
  ;MessageBox MB_OK 'STR_NEEDLE = $STR_NEEDLE STR_HAYSTACK = $STR_HAYSTACK '
    StrCpy $STR_RETURN_VAR ""
    StrCpy $STR_CONTAINS_VAR_1 -1
    StrLen $STR_CONTAINS_VAR_2 $STR_NEEDLE
    StrLen $STR_CONTAINS_VAR_4 $STR_HAYSTACK
    loop:
      IntOp $STR_CONTAINS_VAR_1 $STR_CONTAINS_VAR_1 + 1
      StrCpy $STR_CONTAINS_VAR_3 $STR_HAYSTACK $STR_CONTAINS_VAR_2 $STR_CONTAINS_VAR_1
      StrCmp $STR_CONTAINS_VAR_3 $STR_NEEDLE found
      StrCmp $STR_CONTAINS_VAR_1 $STR_CONTAINS_VAR_4 done
      Goto loop
    found:
      StrCpy $STR_RETURN_VAR $STR_NEEDLE
      Goto done
    done:
   Pop $STR_NEEDLE ;Prevent "invalid opcode" errors and keep the
   Exch $STR_RETURN_VAR  
FunctionEnd
 
!macro _StrContainsConstructor OUT NEEDLE HAYSTACK
  Push "${HAYSTACK}"
  Push "${NEEDLE}"
  Call StrContains
  Pop "${OUT}"
!macroend
 
!define StrContains '!insertmacro "_StrContainsConstructor"'



; Define your application name
!define APPNAME "autoMicromanager"
!define APPNAMEANDVERSION "autoMicromanager 0.6"

; Main Install settings
Name "${APPNAMEANDVERSION}"
InstallDir "$PROGRAMFILES\autoMicromanager"
InstallDirRegKey HKLM "Software\${APPNAME}" ""
OutFile "SetupAutoMicromanager.exe"

; Modern interface settings
!include "MUI.nsh"

!define MUI_ABORTWARNING
!define MUI_FINISHPAGE_RUN "$INSTDIR\Micromanager_App.exe"
    !define MUI_FINISHPAGE_NOAUTOCLOSE
    ;!define MUI_FINISHPAGE_RUN
    !define MUI_FINISHPAGE_RUN_CHECKED
    !define MUI_FINISHPAGE_RUN_TEXT "Quick Start of autoMicromanger Standalone"
    !define MUI_FINISHPAGE_RUN_FUNCTION "LaunchLink"
    ;!define MUI_FINISHPAGE_SHOWREADME_NOTCHECKED
    !define MUI_FINISHPAGE_SHOWREADME "$INSTDIR\MicroscopyToolkitHelpfile.pdf"


!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_LICENSE "License.txt"
!insertmacro MUI_PAGE_COMPONENTS
!insertmacro MUI_PAGE_INSTFILES

!insertmacro MUI_PAGE_FINISH

!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES

; Set languages (first is default language)
!insertmacro MUI_LANGUAGE "English"
!insertmacro MUI_RESERVEFILE_LANGDLL

Function .onInit
         InitPluginsDir
FunctionEnd
 
Function .onInstSuccess
    ExecShell "" "$INSTDIR\Micromanager_Console.exe" ""
FunctionEnd


Function LaunchLink
  
  ExecShell "" "iexplore.exe" "$INSTDIR\QuickStart.htm"
FunctionEnd

Function SearchPathsRegasm
	 Push $0
   Push $1
   Push $2
   Push $3
   Push $4
   Push $5
	 Push $R0
		${locate::Open} "$WINDIR\Microsoft.NET" "/F=1 \
					/D=0 \
					/M=*.exe \
					/B=1" $0
	StrCmp $0 -1 0 loop
	MessageBox MB_OK "Error" IDOK close

	loop:
	${locate::Find} $0 $1 $2 $3 $4 $5 $6
  DetailPrint $3
	StrCmp $1 "" closeEmpty 
	StrCmp $3 "RegAsm.exe" close loop

	Goto loop
	close:
	   StrCpy $0 $1 
     Exec '"$1" "$INSTDIR\micromanager_net.dll" /silent /tlb /codebase'
	closeEmpty:
	${locate::Close} $0
	${locate::Unload}
	
     Pop $R0
		 
     Pop $5
     Pop $4
     Pop $3
     Pop $2
     Pop $1
     Exch $0
FunctionEnd

Function FindRegasm
   Push $0
   Push $1
   Push $2
   Push $3
   Push $4
   Push $5
	 Push $R0
	 Push $R1
	 Push $R2
   ;$0 is the top-level enumeration index we're searching (product)
   ;$1 is the second-level enumeration index we're searching (version)
   ;$2 is the top-level name we're searching (product)
   ;$3 is the second-level name we're searching (version)
   ;$4 is the last-level enumeration index we're searching (individual key)
   ;$5 is the last-level name we're searching (individual key)
 
   StrCpy $0 0 ; Start at the beginning of the Novell products
 
   ProductEnumStart:
 
     EnumRegKey $2 HKEY_LOCAL_MACHINE \
       "Software\Microsoft\Windows\CurrentVersion\Uninstall" $0
 
     IntOp $0 $0 + 1 ; Increment our counter
 
     ; No more products to search, give up!
     StrCmp $2 "" noRegasm
		 
		 Push $0
		 
		 Push $2
     Push "Microsoft .NET "
     Call StrContains
     Pop $0
     StrCmp $0 "" notfound
       Pop $0
		   
     ; Search within the product for version
     StrCpy $1 0
     VersionEnumStart:
       EnumRegValue $3 HKEY_LOCAL_MACHINE \
         "Software\Microsoft\Windows\CurrentVersion\Uninstall\$2" $1
 
       IntOp $1 $1 + 1
			 
			 
			 StrCmp $3 "" ProductEnumStart
       StrCmp "$1" "50" ProductEnumStart 
			 
       ; Search within this version for the key we're looking for     
			 StrCmp $3 "InstallLocation" 0 skip1
			 ;DetailPrint "\$2\$3"
			 ReadRegStr $R1 HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\$2" "$3" 
			 
			 IfFileExists "$R1RegAsm.exe" 0 skip1 
			
			 DetailPrint "$R1"
			 StrCpy $R2 "$R1RegAsm.exe"
			 DetailPrint "r2$R2"
			 goto foundDotNET
				 skip1:
       
     Goto VersionEnumStart
		 Goto done1 
		 notfound:
       Pop $0
     done1:
   Goto ProductEnumStart         
 
   noRegasm:
     StrCpy $0 ""
		 Call SearchPathsRegasm
     Goto done
 
   foundDotNET:
	   
     StrCpy $0 $R2
     Exec '"$R2" "$INSTDIR\micromanager_net.dll" /silent /tlb /codebase'
		 
     
		 
		 
   done:
	   Pop $R2
	   Pop $R1
	   Pop $R0
		 
     Pop $5
     Pop $4
     Pop $3
     Pop $2
     Pop $1
     Exch $0
FunctionEnd

Section "autoMicromanager" Section1

	; Set Section properties
	SetOverwrite on

	; Set Section Files and Shortcuts
	SetOutPath "$INSTDIR\" 
	File "..\3rdParty\Redist\vcredist_x86.exe"
	
	SetOutPath "$INSTDIR\" 
	File "..\3rdParty\Redist\dotnetfx35setup.exe"
SetOutPath "$INSTDIR\" 
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\DotNetChecked\Publish\DotNetChecker.application"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\DotNetChecked\Publish\setup.exe"
SetOutPath "$INSTDIR\Application Files\DotNetChecker_1_0_0_8"
 File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\DotNetChecked\Publish\Application Files\DotNetChecker_1_0_0_8\DotNetChecker.exe.deploy"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\DotNetChecked\Publish\Application Files\DotNetChecker_1_0_0_8\DotNetChecker.exe.manifest"
SetOutPath "$INSTDIR\Application Files\DotNetChecker_1_0_0_2"
 File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\DotNetChecked\Publish\Application Files\DotNetChecker_1_0_0_2\DotNetChecker.application"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\DotNetChecked\Publish\Application Files\DotNetChecker_1_0_0_2\DotNetChecker.exe.deploy"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\DotNetChecked\Publish\Application Files\DotNetChecker_1_0_0_2\DotNetChecker.exe.manifest"

	
SetOutPath "$INSTDIR\" 
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\ATMCD32D.DLL"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\FreeImage.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\FreeImageNET.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\FreeImageNET.xml"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\HardwareConfig.exe"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\ICSharpCode.SharpZipLib.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\ICSharpCode.TextEditor.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\Interop.ACTIVESKINLib.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\Interop.stdole.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\Interop.WIA.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\IronPython.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\IronPython.Modules.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\IronPython.Modules.xml"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\IronPython.xml"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\JoystickInterface.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\JoystickInterface.xml"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\LibIcon.bmp"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\license.txt"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\MCL_MicroDrive.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\MCL_NanoDrive.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\Micromanager_App.exe"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\Micromanager_Console.exe"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\Micromanager_net.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\Micromanager_net.dll.config"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\Microsoft.Dynamic.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\Microsoft.Ink.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\Microsoft.Scripting.Core.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\Microsoft.Scripting.Debugging.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\Microsoft.Scripting.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\Microsoft.Scripting.ExtensionAttribute.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\MMC.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\MMC410.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\MMCoreCS.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\mmgr_dal_DemoCamera.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\mmgr_dal_DemoRGBCamera.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\mmgr_dal_DemoStreamingCamera.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\mmgr_dal_NIMotionControl.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\mmgr_dal_NI_DAQ.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\mmgr_dal_PiMercuryStep.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\mmgr_dal_PVCAM.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\MMUI_GenericDeviceGUIs.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\MMUI_MyDevices.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\MMUI_ScriptModules.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\SciImage.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\SciImagePlugin.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\SyntaxFiles.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\test.sqlite"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\WeifenLuo.WinFormsUI.Docking.dll"
SetOutPath "$INSTDIR\UserData"
 File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\UserData\Placeholder.txt"
SetOutPath "$INSTDIR\ConfigFiles"
 File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\ConfigFiles\DemoScript.xml"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\ConfigFiles\DemoScript_Desktop.config"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\ConfigFiles\DemoScript_full.xml"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\MM_Net\bin\Debug\ConfigFiles\PlaceHolder.txt"

	
SetOutPath "$INSTDIR\" 
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\Python.net Runtime\clr.pyd"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\Python.net Runtime\clr.so"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\Python.net Runtime\Python.Runtime.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\Python.net Runtime\Python.Test.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\Python.net Runtime\pythonN.exe"

SetOutPath "$INSTDIR\" 
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\MCL_MicroDrive.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\MCL_NanoDrive.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_ABSCamera.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_Andor.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_AndorLaserCombiner.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_AOTF.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_Apogee.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_Arduino.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_ASIFW1000.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_ASIStage.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_ASIwptr.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_CoherentCube.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_Conix.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_CSUX.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_DemoCamera.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_DemoRGBCamera.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_DemoStreamingCamera.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_DTOpenLayer.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_GenericSLM.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_Hamamatsu.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_K8055.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_K8061.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_LeicaDMI.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_LeicaDMR.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_Ludl.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_MCL_MicroDrive.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_MCL_NanoDrive.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_Neos.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_Nikon.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_NikonAZ100.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_NikonTE2000.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_NikonTI.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_Olympus.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_ParallelPort.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_PCO_Camera.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_Pecon.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_PI.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_PIMercuryStep.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_Piper.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_PI_GCS.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_PrecisExcite.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_Prior.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_PVCAM.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_QCam.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_ScionCam.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_Sensicam.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_SerialManager.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_SimpleAF.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_SpectralLMM5.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_SpotCamera.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_SutterLambda.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_ThorlabsFilterWheel.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_ThorlabsSC10.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_TwainCamera.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_USBManager.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_Utilities.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_Vincent.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_Yokogawa.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_ZeissCAN.dll"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\3rdParty\CPPOriginalMicromanager\mmgr_dal_ZeissCAN29.dll"

	
	SetOutPath "$INSTDIR\" 
	File "QuickStart.htm"
SetOutPath "$INSTDIR\QuickStart_files\" 
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\QuickStart_files\filelist.xml"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\QuickStart_files\image001.png"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\QuickStart_files\image002.jpg"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\QuickStart_files\image003.png"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\QuickStart_files\image004.jpg"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\QuickStart_files\image005.png"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\QuickStart_files\image006.jpg"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\QuickStart_files\image007.png"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\QuickStart_files\image008.jpg"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\QuickStart_files\image009.png"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\QuickStart_files\image010.jpg"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\QuickStart_files\image011.png"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\QuickStart_files\image012.jpg"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\QuickStart_files\image013.png"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\QuickStart_files\image014.jpg"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\QuickStart_files\image015.png"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\QuickStart_files\image016.jpg"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\QuickStart_files\image017.png"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\QuickStart_files\image018.jpg"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\QuickStart_files\Thumbs.db"

	
  ExecWait '"$INSTDIR\vcredist_x86.exe" /q:a'
	ExecWait '"$INSTDIR\setup.exe" /q:a'
	CreateShortCut "$DESKTOP\autoMicromanager.lnk" "$INSTDIR\Micromanager_App.exe"
	
	
	CreateDirectory "$SMPROGRAMS\autoMicromanager"
	CreateShortCut "$SMPROGRAMS\autoMicromanager\autoMicromanager.lnk" "$INSTDIR\Micromanager_App.exe"
	
	CreateShortCut "$SMPROGRAMS\autoMicromanager\autoMicromanager Console.lnk" "$INSTDIR\Micromanager_Console.exe"
	
	
	CreateShortCut "$SMPROGRAMS\autoMicromanager\autoMicromanger Quick Start.lnk" "$INSTDIR\QuickStart.htm"
	
	
	CreateShortCut "$SMPROGRAMS\autoMicromanager\Tutorial-Manual.lnk" "$INSTDIR\MicroscopyToolkitHelpfile.pdf"
  CreateShortCut "$SMPROGRAMS\autoMicromanager\Class Documentation.lnk" "$INSTDIR\autoMicromanager Documented Class Library.chm"
SetOutPath "$INSTDIR\" 
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\Tutorials\autoMicromanager Documented Class Library.chm"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\Tutorials\ClassDiagram.bmp"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\Tutorials\MicroscopyToolkitHelpfile.pdf"

	CreateShortCut "$SMPROGRAMS\autoMicromanager\Create Hardware Config.lnk" "$INSTDIR\HardwareConfig.exe"
	CreateShortCut "$SMPROGRAMS\autoMicromanager\Uninstall.lnk" "$INSTDIR\uninstall.exe"
	
	Call FindRegasm
	
	
	;ExecWait '"$INSTDIR\vcredist_x86.exe" /q:a'
SectionEnd

Section /o "Labview 8.0 Controls and Tutorials" Section3
 push $1
          ReadRegStr $1 HKCR "Applications\LabVIEW.exe\shell\open\command" ""
					end:
					StrCmp $1 "" empty
					${WordReplace} $1 '"' "" "+" $1
					${WordReplace} $1 '%1' "" "+" $1
					${WordFind} "$1" "\" "-2{*" $1
					StrCpy $R1 "$1"
          pop $1
	; Set Section properties
	SetOverwrite on

	; Set Section Files and Shortcuts
SetOutPath "$R1\menus\Controls\autoMicromanager\" 
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabViewMMControls80\AllDeviceHolders.ctl"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabViewMMControls80\ChannelSetupControl.ctl"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabViewMMControls80\Joystick.ctl"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabViewMMControls80\NIEasyCore.ctl"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabViewMMControls80\NIImageProcessor.ctl"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabViewMMControls80\NILUTGraph.ctl"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabViewMMControls80\NIPropList.ctl"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabViewMMControls80\PictureBoard.ctl"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabViewMMControls80\SingleDeviceHolder.ctl"

SetOutPath "$R1\menus\Controls\DotNet & ActiveX\autoMicromanager\" 
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabViewMMControls80\AllDeviceHolders.ctl"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabViewMMControls80\ChannelSetupControl.ctl"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabViewMMControls80\Joystick.ctl"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabViewMMControls80\NIEasyCore.ctl"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabViewMMControls80\NIImageProcessor.ctl"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabViewMMControls80\NILUTGraph.ctl"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabViewMMControls80\NIPropList.ctl"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabViewMMControls80\PictureBoard.ctl"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabViewMMControls80\SingleDeviceHolder.ctl"

	
	goto good
 empty:
   StrCpy $R7 "Empty"
	 MessageBox MB_ICONEXCLAMATION|MB_OK "Cannot find Labview Installation, labview files will not be installed" IDOK Sec2End
	 goto Sec2End
 good:
   StrCpy $R7 "Labview" 
 Sec2End:	 
  StrCmp $R7 "Empty" empty2
	; Set Section properties
	SetOverwrite on

SetOutPath "$INSTDIR\" 
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM80\Controls not Loaded.txt"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM80\FindLibraryAndDevice.vi"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM80\Make Hardware Config File.vi"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM80\MMCoreOnly.vi"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM80\Test.bmp"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM80\Test.tif"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM80\Test2.tif"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM80\TEst2.xml"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM80\TEst2_Desktop.config"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM80\TEst2_full.xml"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM80\Test3.xml"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM80\Test3_Desktop.config"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM80\Test3_full.xml"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM80\Tutorial10_Full Application.vi"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM80\Tutorial1_Start Camera.vi"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM80\Tutorial2_Image Manipulation.vi"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM80\Tutorial3_Image Sequence.vi"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM80\Tutorial4_Image Save.vi"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM80\Tutorial5_Camera and Stage.vi"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM80\Tutorial6_Device Properties.vi"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM80\Tutorial7_Joystick Control.vi"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM80\Tutorial8_Camera_Stage_Filter.vi"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM80\Tutorial9.1_Image Catching.vi"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM80\Tutorial9.2_Register Paint Board.vi"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM80\Tutorial9.3_Image Catching with Channels.vi"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM80\Tutorial9_Channels.vi"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM80\Tutorial_ConfigFile_Device Control.vi"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM80\Tutorial_ConfigFile_Image Capture.vi"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM80\Tutorial_ConfigFile_Load Config File.vi"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM80\Tutorial_ConfigFile_Make Hardware Config File.vi"

	; Set Section Files and Shortcuts
	
	
  CreateDirectory "$SMPROGRAMS\autoMicromanager\Labview"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Labview\FindLibraryAndDevice.lnk" "$INSTDIR\FindLibraryAndDevice.vi"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Labview\Make Hardware Config File.lnk" "$INSTDIR\Make Hardware Config File.vi"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Labview\MMCoreOnly.lnk" "$INSTDIR\MMCoreOnly.vi"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Labview\Tutorial10_Full Application.lnk" "$INSTDIR\Tutorial10_Full Application.vi"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Labview\Tutorial1_Start Camera.lnk" "$INSTDIR\Tutorial1_Start Camera.vi"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Labview\Tutorial2_Image Manipulation.lnk" "$INSTDIR\Tutorial2_Image Manipulation.vi"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Labview\Tutorial3_Image Sequence.lnk" "$INSTDIR\Tutorial3_Image Sequence.vi"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Labview\Tutorial4_Image Save.lnk" "$INSTDIR\Tutorial4_Image Save.vi"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Labview\Tutorial5_Camera and Stage.lnk" "$INSTDIR\Tutorial5_Camera and Stage.vi"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Labview\Tutorial6_Device Properties.lnk" "$INSTDIR\Tutorial6_Device Properties.vi"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Labview\Tutorial7_Joystick Control.lnk" "$INSTDIR\Tutorial7_Joystick Control.vi"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Labview\Tutorial8_Camera_Stage_Filter.lnk" "$INSTDIR\Tutorial8_Camera_Stage_Filter.vi"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Labview\Tutorial9.1_Image Catching.lnk" "$INSTDIR\Tutorial9.1_Image Catching.vi"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Labview\Tutorial9.2_Register Paint Board.lnk" "$INSTDIR\Tutorial9.2_Register Paint Board.vi"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Labview\Tutorial9.3_Image Catching with Channels.lnk" "$INSTDIR\Tutorial9.3_Image Catching with Channels.vi"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Labview\Tutorial9_Channels.lnk" "$INSTDIR\Tutorial9_Channels.vi"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Labview\Tutorial_ConfigFile_Device Control.lnk" "$INSTDIR\Tutorial_ConfigFile_Device Control.vi"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Labview\Tutorial_ConfigFile_Image Capture.lnk" "$INSTDIR\Tutorial_ConfigFile_Image Capture.vi"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Labview\Tutorial_ConfigFile_Load Config File.lnk" "$INSTDIR\Tutorial_ConfigFile_Load Config File.vi"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Labview\Tutorial_ConfigFile_Make Hardware Config File.lnk" "$INSTDIR\Tutorial_ConfigFile_Make Hardware Config File.vi"

CreateShortCut "$SMPROGRAMS\autoMicromanager\Labview\Controls not Loaded.lnk" "$INSTDIR\Controls not Loaded.txt"

	;CreateShortCut "$SMPROGRAMS\Micromanager.NET\Labview\Tutorial1.lnk" "$INSTDIR\Labview\Tutorial1.vi"
	
	
	empty2:
SectionEnd


Section "Labview 8.6 Controls and Tutorials" Section2
 push $1
          ReadRegStr $1 HKCR "Applications\LabVIEW.exe\shell\open\command" ""
					end:
					StrCmp $1 "" empty
					${WordReplace} $1 '"' "" "+" $1
					${WordReplace} $1 '%1' "" "+" $1
					${WordFind} "$1" "\" "-2{*" $1
					StrCpy $R1 "$1"
          pop $1
	; Set Section properties
	SetOverwrite on

	; Set Section Files and Shortcuts
SetOutPath "$R1\menus\Controls\autoMicromanager\" 
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabViewMMControls86\AllDeviceHolders.ctl"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabViewMMControls86\ChannelSetupControl.ctl"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabViewMMControls86\LUTGraph.ctl"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabViewMMControls86\MMJoyStick.ctl"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabViewMMControls86\NIEasyCore.ctl"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabViewMMControls86\NIImageProcessor.ctl"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabViewMMControls86\NIPropList.ctl"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabViewMMControls86\PictureBoard.ctl"

SetOutPath "$R1\menus\Controls\DotNet & ActiveX\autoMicromanager\" 
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabViewMMControls86\AllDeviceHolders.ctl"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabViewMMControls86\ChannelSetupControl.ctl"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabViewMMControls86\LUTGraph.ctl"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabViewMMControls86\MMJoyStick.ctl"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabViewMMControls86\NIEasyCore.ctl"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabViewMMControls86\NIImageProcessor.ctl"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabViewMMControls86\NIPropList.ctl"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabViewMMControls86\PictureBoard.ctl"

	
	goto good
 empty:
   StrCpy $R7 "Empty"
	 MessageBox MB_ICONEXCLAMATION|MB_OK "Cannot find Labview Installation, labview files will not be installed" IDOK Sec2End
	 goto Sec2End
 good:
   StrCpy $R7 "Labview" 
 Sec2End:	 
  StrCmp $R7 "Empty" empty2
	; Set Section properties
	SetOverwrite on

SetOutPath "$INSTDIR\Labview\" 
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM86\FindLibraryAndDevice.vi"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM86\Make Hardware Config File.vi"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM86\MMCoreOnly.vi"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM86\Test.bmp"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM86\Test.tif"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM86\Test2.tif"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM86\TEst2.xml"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM86\TEst2_Desktop.config"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM86\TEst2_full.xml"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM86\Test3.xml"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM86\Test3_Desktop.config"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM86\Test3_full.xml"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM86\Tutorial10_Full Application.vi"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM86\Tutorial1_Start Camera.vi"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM86\Tutorial2_Image Manipulation.vi"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM86\Tutorial3_Image Sequence.vi"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM86\Tutorial4_Image Save.vi"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM86\Tutorial5_Camera and Stage.vi"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM86\Tutorial6_Device Properties.vi"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM86\Tutorial7_Joystick Control.vi"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM86\Tutorial8_Camera_Stage_Filter.vi"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM86\Tutorial9.1_Image Catching.vi"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM86\Tutorial9.2_Register Paint Board.vi"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM86\Tutorial9.3_Image Catching with Channels.vi"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM86\Tutorial9_Channels.vi"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM86\Tutorial_ConfigFile_Device Control.vi"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM86\Tutorial_ConfigFile_Image Capture.vi"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM86\Tutorial_ConfigFile_Load Config File.vi"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\LabviewMM86\Tutorial_ConfigFile_Make Hardware Config File.vi"

	; Set Section Files and Shortcuts
	
	
  CreateDirectory "$SMPROGRAMS\autoMicromanager\Labview"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Labview\FindLibraryAndDevice.lnk" "$INSTDIR\Labview\FindLibraryAndDevice.vi"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Labview\Make Hardware Config File.lnk" "$INSTDIR\Labview\Make Hardware Config File.vi"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Labview\MMCoreOnly.lnk" "$INSTDIR\Labview\MMCoreOnly.vi"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Labview\Tutorial10_Full Application.lnk" "$INSTDIR\Labview\Tutorial10_Full Application.vi"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Labview\Tutorial1_Start Camera.lnk" "$INSTDIR\Labview\Tutorial1_Start Camera.vi"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Labview\Tutorial2_Image Manipulation.lnk" "$INSTDIR\Labview\Tutorial2_Image Manipulation.vi"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Labview\Tutorial3_Image Sequence.lnk" "$INSTDIR\Labview\Tutorial3_Image Sequence.vi"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Labview\Tutorial4_Image Save.lnk" "$INSTDIR\Labview\Tutorial4_Image Save.vi"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Labview\Tutorial5_Camera and Stage.lnk" "$INSTDIR\Labview\Tutorial5_Camera and Stage.vi"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Labview\Tutorial6_Device Properties.lnk" "$INSTDIR\Labview\Tutorial6_Device Properties.vi"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Labview\Tutorial7_Joystick Control.lnk" "$INSTDIR\Labview\Tutorial7_Joystick Control.vi"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Labview\Tutorial8_Camera_Stage_Filter.lnk" "$INSTDIR\Labview\Tutorial8_Camera_Stage_Filter.vi"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Labview\Tutorial9.1_Image Catching.lnk" "$INSTDIR\Labview\Tutorial9.1_Image Catching.vi"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Labview\Tutorial9.2_Register Paint Board.lnk" "$INSTDIR\Labview\Tutorial9.2_Register Paint Board.vi"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Labview\Tutorial9.3_Image Catching with Channels.lnk" "$INSTDIR\Labview\Tutorial9.3_Image Catching with Channels.vi"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Labview\Tutorial9_Channels.lnk" "$INSTDIR\Labview\Tutorial9_Channels.vi"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Labview\Tutorial_ConfigFile_Device Control.lnk" "$INSTDIR\Labview\Tutorial_ConfigFile_Device Control.vi"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Labview\Tutorial_ConfigFile_Image Capture.lnk" "$INSTDIR\Labview\Tutorial_ConfigFile_Image Capture.vi"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Labview\Tutorial_ConfigFile_Load Config File.lnk" "$INSTDIR\Labview\Tutorial_ConfigFile_Load Config File.vi"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Labview\Tutorial_ConfigFile_Make Hardware Config File.lnk" "$INSTDIR\Labview\Tutorial_ConfigFile_Make Hardware Config File.vi"

	
	;CreateShortCut "$SMPROGRAMS\Micromanager.NET\Labview\Tutorial1.lnk" "$INSTDIR\Labview\Tutorial1.vi"
	
	
	empty2:
SectionEnd


Section "Mathematica Tutorials" Section4

  
	SetOverwrite on

SetOutPath "$INSTDIR\Mathematica\" 
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\Mathematica\Example 1_Get 1 Image.txt"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\Mathematica\Example 2_Control Device.txt"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\Mathematica\Example 3_Capture Images.txt"

	; Set Section Files and Shortcuts
	
	
  CreateDirectory "$SMPROGRAMS\autoMicromanager\Mathematica"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Mathematica\Example 1_Get 1 Image.lnk" "$INSTDIR\Mathematica\Example 1_Get 1 Image.txt"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Mathematica\Example 2_Control Device.lnk" "$INSTDIR\Mathematica\Example 2_Control Device.txt"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Mathematica\Example 3_Capture Images.lnk" "$INSTDIR\Mathematica\Example 3_Capture Images.txt"

	
	;CreateShortCut "$SMPROGRAMS\Micromanager.NET\Labview\Tutorial1.lnk" "$INSTDIR\Labview\Tutorial1.vi"
	
	
	
	
	
	empty4:
SectionEnd

Section "Matlab Tutorials" Section5

 
	SetOverwrite on

SetOutPath "$INSTDIR\Matlab\" 
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\Matlab\Example1.m"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\Matlab\Example1.txt"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\Matlab\Example2.m"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\Matlab\Example2.txt"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\Matlab\Example3.asv"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\Matlab\Example3.m"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\Matlab\Example3.txt"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\Matlab\ImageProducedHandler.asv"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\Matlab\ImageProducedHandler.m"

	; Set Section Files and Shortcuts
	
	
  CreateDirectory "$SMPROGRAMS\autoMicromanager\Matlab"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Matlab\Example1.lnk" "$INSTDIR\Matlab\Example1.m"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Matlab\Example2.lnk" "$INSTDIR\Matlab\Example2.m"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Matlab\Example3.lnk" "$INSTDIR\Matlab\Example3.m"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Matlab\ImageProducedHandler.lnk" "$INSTDIR\Matlab\ImageProducedHandler.m"

CreateShortCut "$SMPROGRAMS\autoMicromanager\Matlab\Example1.lnk" "$INSTDIR\Matlab\Example1.txt"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Matlab\Example2.lnk" "$INSTDIR\Matlab\Example2.txt"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Matlab\Example3.lnk" "$INSTDIR\Matlab\Example3.txt"

	;CreateShortCut "$SMPROGRAMS\Micromanager.NET\Labview\Tutorial1.lnk" "$INSTDIR\Labview\Tutorial1.vi"
	
	
	
	empty5:
SectionEnd

Section "Python 2.5.x Tutorials" Section6

  
	WriteRegStr HKCR ".pyn" "" "PythonNet.Script"
	WriteRegStr HKCR ".pyN" "" "PythonNet.Script"
  WriteRegStr HKCR "PythonNet.Script" "" "Python For .Net Script"
  WriteRegStr HKCR "PythonNet.Script\DefaultIcon" "" "$INSTDIR\PythonN.exe,1"
	WriteRegStr HKCR "PythonNet.Script\shell" "" "open"
	WriteRegStr HKCR "PythonNet.Script\shell\open\command" "" '$INSTDIR\PythonN.exe "%1"'
	
	SetOverwrite on

SetOutPath "$INSTDIR\Python\" 
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\python2.5-UCS2\MicromanagerScript.pyN"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\python2.5-UCS2\MicromanagerScript_Device.pyN"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\python2.5-UCS2\MicromanagerScript_Scipy.pyN"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\python2.5-UCS2\No_GUI_MicromanagerScript.pyN"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\python2.5-UCS2\Readme.txt"

	; Set Section Files and Shortcuts
	
	
  CreateDirectory "$SMPROGRAMS\autoMicromanager\Python"

CreateShortCut "$SMPROGRAMS\autoMicromanager\Python\MicromanagerScript.lnk" "$INSTDIR\Python\MicromanagerScript.pyN"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Python\MicromanagerScript_Device.lnk" "$INSTDIR\Python\MicromanagerScript_Device.pyN"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Python\MicromanagerScript_Scipy.lnk" "$INSTDIR\Python\MicromanagerScript_Scipy.pyN"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Python\No_GUI_MicromanagerScript.lnk" "$INSTDIR\Python\No_GUI_MicromanagerScript.pyN"

CreateShortCut "$SMPROGRAMS\autoMicromanager\Python\Readme.lnk" "$INSTDIR\Python\Readme.txt"

	;CreateShortCut "$SMPROGRAMS\Micromanager.NET\Labview\Tutorial1.lnk" "$INSTDIR\Labview\Tutorial1.vi"
	
	System::Call 'Shell32::SHChangeNotify(i ${SHCNE_ASSOCCHANGED}, i ${SHCNF_IDLIST}, i 0, i 0)'
	
	empty6:
SectionEnd

Section "Scilab Tutorials" Section8

 
	SetOverwrite on

SetOutPath "$INSTDIR\Scilab\" 
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\Scilab\Scilab Readme.html"

	; Set Section Files and Shortcuts
	SetOverwrite off
	SetOutPath "C:\Program Files\scilab-5.2.1\bin"
	File "..\3rdParty\SciLab\DotNet-Component-Scilab.dll"
	
	SetOverwrite on
  CreateDirectory "$SMPROGRAMS\autoMicromanager\Scilab"
CreateShortCut "$SMPROGRAMS\autoMicromanager\Scilab\Scilab Readme.lnk" "$INSTDIR\Scilab\Scilab Readme.html"

	
	empty8:
SectionEnd

Section "(D)COM/VB Tutorials" Section7

  
		
	SetOverwrite on

SetOutPath "$INSTDIR\VB6\" 
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\VB6MM\CoreImageConverter.bas"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\VB6MM\Form_MMPictureBoard.frm"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\VB6MM\Form_MMPictureBoard.log"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\VB6MM\ReadMe.txt"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\VB6MM\ScriptForm.frm"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\VB6MM\ScriptForm.frx"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\VB6MM\StartForm.frm"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\VB6MM\VBMicromanager.exe"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\VB6MM\VBMicromanager.frm"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\VB6MM\VBMicromanager.vbp"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\VB6MM\VBMicromanager.vbw"
File "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\autoMicromanager\Installers\..\VB6MM\VBScriptExample.vbs"

	; Set Section Files and Shortcuts
	
	
  CreateDirectory "$SMPROGRAMS\autoMicromanager\COM-VB Tutorials"
CreateShortCut "$SMPROGRAMS\autoMicromanager\COM-VB Tutorials\VBMicromanager.lnk" "$INSTDIR\VB6\VBMicromanager.vbp"

CreateShortCut "$SMPROGRAMS\autoMicromanager\COM-VB Tutorials\VBScriptExample.lnk" "$INSTDIR\VB6\VBScriptExample.vbs"

CreateShortCut "$SMPROGRAMS\autoMicromanager\COM-VB Tutorials\ReadMe.lnk" "$INSTDIR\VB6\ReadMe.txt"

	
	
	empty6:
SectionEnd
Section -FinishSection

  
	WriteRegStr HKLM "Software\${APPNAME}" "" "$INSTDIR"
	WriteRegStr HKLM "Software\${APPNAME}" "Path" "$INSTDIR"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "DisplayName" "${APPNAME}"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "UninstallString" "$INSTDIR\uninstall.exe"
	WriteUninstaller "$INSTDIR\uninstall.exe"
  
	
	
SectionEnd

; Modern install component descriptions
!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
	!insertmacro MUI_DESCRIPTION_TEXT ${Section1} ""
	!insertmacro MUI_DESCRIPTION_TEXT ${Section2} ""
	!insertmacro MUI_DESCRIPTION_TEXT ${Section3} ""
!insertmacro MUI_FUNCTION_DESCRIPTION_END

;Uninstall section
Section Uninstall

	;Remove from registry...
	DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}"
	DeleteRegKey HKLM "SOFTWARE\${APPNAME}"

	; Delete self
	Delete "$INSTDIR\uninstall.exe"

	; Delete Shortcuts
	Delete "$DESKTOP\autoMicromanager.lnk"
	Delete "$SMPROGRAMS\autoMicromanager\Micromanager.NET.lnk"
	Delete "$SMPROGRAMS\autoMicromanager\Uninstall.lnk"
	Delete "$SMPROGRAMS\autoMicromanager\Browse Hardware Libraries.lnk" 
	Delete "$SMPROGRAMS\autoMicromanager\Make Config File.lnk" 
  Delete "$SMPROGRAMS\autoMicromanager\Tutorial-Manual.lnk" 
	Delete  "$INSTDIR\autoMicromanager Documented Class Library.chm"
	Delete  "$SMPROGRAMS\autoMicromanager\Class Documentation.lnk"
	Delete "$SMPROGRAMS\autoMicromanager\Micromanager.NET Console.lnk" 
	Delete "$SMPROGRAMS\autoMicromanager\Create Hardware Config.lnk" 
	Delete "$INSTDIR\FindLibraryAndDevice.vi"
	Delete "$INSTDIR\MakeHardWareConfigFile.vi"
	
	; Clean up Micromanager.NET
	
	
	; Clean up LabviewControls
	push $1
          ReadRegStr $1 HKCR "Applications\LabVIEW.exe\shell\open\command" ""
					end:
					StrCmp $1 "" empty3
					${WordReplace} $1 '"' "" "+" $1
					${WordReplace} $1 '%1' "" "+" $1
					${WordFind} "$1" "\" "-2{*" $1
					StrCpy $R1 "$1"
          pop $1
	
	
Delete "$INSTDIR\DotNetChecker.application"
Delete "$INSTDIR\setup.exe"
Delete "$INSTDIR\Application Files\DotNetChecker_1_0_0_8\DotNetChecker.exe.deploy"
Delete "$INSTDIR\Application Files\DotNetChecker_1_0_0_8\DotNetChecker.exe.manifest"
Delete "$INSTDIR\Application Files\DotNetChecker_1_0_0_2\DotNetChecker.application"
Delete "$INSTDIR\Application Files\DotNetChecker_1_0_0_2\DotNetChecker.exe.deploy"
Delete "$INSTDIR\Application Files\DotNetChecker_1_0_0_2\DotNetChecker.exe.manifest"
Delete "$INSTDIR\ATMCD32D.DLL"
Delete "$INSTDIR\FreeImage.dll"
Delete "$INSTDIR\FreeImageNET.dll"
Delete "$INSTDIR\FreeImageNET.xml"
Delete "$INSTDIR\HardwareConfig.exe"
Delete "$INSTDIR\ICSharpCode.SharpZipLib.dll"
Delete "$INSTDIR\ICSharpCode.TextEditor.dll"
Delete "$INSTDIR\Interop.ACTIVESKINLib.dll"
Delete "$INSTDIR\Interop.stdole.dll"
Delete "$INSTDIR\Interop.WIA.dll"
Delete "$INSTDIR\IronPython.dll"
Delete "$INSTDIR\IronPython.Modules.dll"
Delete "$INSTDIR\IronPython.Modules.xml"
Delete "$INSTDIR\IronPython.xml"
Delete "$INSTDIR\JoystickInterface.dll"
Delete "$INSTDIR\JoystickInterface.xml"
Delete "$INSTDIR\LibIcon.bmp"
Delete "$INSTDIR\license.txt"
Delete "$INSTDIR\MCL_MicroDrive.dll"
Delete "$INSTDIR\MCL_NanoDrive.dll"
Delete "$INSTDIR\Micromanager_App.exe"
Delete "$INSTDIR\Micromanager_Console.exe"
Delete "$INSTDIR\Micromanager_net.dll"
Delete "$INSTDIR\Micromanager_net.dll.config"
Delete "$INSTDIR\Microsoft.Dynamic.dll"
Delete "$INSTDIR\Microsoft.Ink.dll"
Delete "$INSTDIR\Microsoft.Scripting.Core.dll"
Delete "$INSTDIR\Microsoft.Scripting.Debugging.dll"
Delete "$INSTDIR\Microsoft.Scripting.dll"
Delete "$INSTDIR\Microsoft.Scripting.ExtensionAttribute.dll"
Delete "$INSTDIR\MMC.dll"
Delete "$INSTDIR\MMC410.dll"
Delete "$INSTDIR\MMCoreCS.dll"
Delete "$INSTDIR\mmgr_dal_DemoCamera.dll"
Delete "$INSTDIR\mmgr_dal_DemoRGBCamera.dll"
Delete "$INSTDIR\mmgr_dal_DemoStreamingCamera.dll"
Delete "$INSTDIR\mmgr_dal_NIMotionControl.dll"
Delete "$INSTDIR\mmgr_dal_NI_DAQ.dll"
Delete "$INSTDIR\mmgr_dal_PiMercuryStep.dll"
Delete "$INSTDIR\mmgr_dal_PVCAM.dll"
Delete "$INSTDIR\MMUI_GenericDeviceGUIs.dll"
Delete "$INSTDIR\MMUI_MyDevices.dll"
Delete "$INSTDIR\MMUI_ScriptModules.dll"
Delete "$INSTDIR\SciImage.dll"
Delete "$INSTDIR\SciImagePlugin.dll"
Delete "$INSTDIR\SyntaxFiles.dll"
Delete "$INSTDIR\test.sqlite"
Delete "$INSTDIR\WeifenLuo.WinFormsUI.Docking.dll"
Delete "$INSTDIR\UserData\Placeholder.txt"
Delete "$INSTDIR\ConfigFiles\DemoScript.xml"
Delete "$INSTDIR\ConfigFiles\DemoScript_Desktop.config"
Delete "$INSTDIR\ConfigFiles\DemoScript_full.xml"
Delete "$INSTDIR\ConfigFiles\PlaceHolder.txt"
Delete "$INSTDIR\clr.pyd"
Delete "$INSTDIR\clr.so"
Delete "$INSTDIR\Python.Runtime.dll"
Delete "$INSTDIR\Python.Test.dll"
Delete "$INSTDIR\pythonN.exe"
Delete "$INSTDIR\MCL_MicroDrive.dll"
Delete "$INSTDIR\MCL_NanoDrive.dll"
Delete "$INSTDIR\mmgr_dal_ABSCamera.dll"
Delete "$INSTDIR\mmgr_dal_Andor.dll"
Delete "$INSTDIR\mmgr_dal_AndorLaserCombiner.dll"
Delete "$INSTDIR\mmgr_dal_AOTF.dll"
Delete "$INSTDIR\mmgr_dal_Apogee.dll"
Delete "$INSTDIR\mmgr_dal_Arduino.dll"
Delete "$INSTDIR\mmgr_dal_ASIFW1000.dll"
Delete "$INSTDIR\mmgr_dal_ASIStage.dll"
Delete "$INSTDIR\mmgr_dal_ASIwptr.dll"
Delete "$INSTDIR\mmgr_dal_CoherentCube.dll"
Delete "$INSTDIR\mmgr_dal_Conix.dll"
Delete "$INSTDIR\mmgr_dal_CSUX.dll"
Delete "$INSTDIR\mmgr_dal_DemoCamera.dll"
Delete "$INSTDIR\mmgr_dal_DemoRGBCamera.dll"
Delete "$INSTDIR\mmgr_dal_DemoStreamingCamera.dll"
Delete "$INSTDIR\mmgr_dal_DTOpenLayer.dll"
Delete "$INSTDIR\mmgr_dal_GenericSLM.dll"
Delete "$INSTDIR\mmgr_dal_Hamamatsu.dll"
Delete "$INSTDIR\mmgr_dal_K8055.dll"
Delete "$INSTDIR\mmgr_dal_K8061.dll"
Delete "$INSTDIR\mmgr_dal_LeicaDMI.dll"
Delete "$INSTDIR\mmgr_dal_LeicaDMR.dll"
Delete "$INSTDIR\mmgr_dal_Ludl.dll"
Delete "$INSTDIR\mmgr_dal_MCL_MicroDrive.dll"
Delete "$INSTDIR\mmgr_dal_MCL_NanoDrive.dll"
Delete "$INSTDIR\mmgr_dal_Neos.dll"
Delete "$INSTDIR\mmgr_dal_Nikon.dll"
Delete "$INSTDIR\mmgr_dal_NikonAZ100.dll"
Delete "$INSTDIR\mmgr_dal_NikonTE2000.dll"
Delete "$INSTDIR\mmgr_dal_NikonTI.dll"
Delete "$INSTDIR\mmgr_dal_Olympus.dll"
Delete "$INSTDIR\mmgr_dal_ParallelPort.dll"
Delete "$INSTDIR\mmgr_dal_PCO_Camera.dll"
Delete "$INSTDIR\mmgr_dal_Pecon.dll"
Delete "$INSTDIR\mmgr_dal_PI.dll"
Delete "$INSTDIR\mmgr_dal_PIMercuryStep.dll"
Delete "$INSTDIR\mmgr_dal_Piper.dll"
Delete "$INSTDIR\mmgr_dal_PI_GCS.dll"
Delete "$INSTDIR\mmgr_dal_PrecisExcite.dll"
Delete "$INSTDIR\mmgr_dal_Prior.dll"
Delete "$INSTDIR\mmgr_dal_PVCAM.dll"
Delete "$INSTDIR\mmgr_dal_QCam.dll"
Delete "$INSTDIR\mmgr_dal_ScionCam.dll"
Delete "$INSTDIR\mmgr_dal_Sensicam.dll"
Delete "$INSTDIR\mmgr_dal_SerialManager.dll"
Delete "$INSTDIR\mmgr_dal_SimpleAF.dll"
Delete "$INSTDIR\mmgr_dal_SpectralLMM5.dll"
Delete "$INSTDIR\mmgr_dal_SpotCamera.dll"
Delete "$INSTDIR\mmgr_dal_SutterLambda.dll"
Delete "$INSTDIR\mmgr_dal_ThorlabsFilterWheel.dll"
Delete "$INSTDIR\mmgr_dal_ThorlabsSC10.dll"
Delete "$INSTDIR\mmgr_dal_TwainCamera.dll"
Delete "$INSTDIR\mmgr_dal_USBManager.dll"
Delete "$INSTDIR\mmgr_dal_Utilities.dll"
Delete "$INSTDIR\mmgr_dal_Vincent.dll"
Delete "$INSTDIR\mmgr_dal_Yokogawa.dll"
Delete "$INSTDIR\mmgr_dal_ZeissCAN.dll"
Delete "$INSTDIR\mmgr_dal_ZeissCAN29.dll"
Delete "$INSTDIR\QuickStart_files\filelist.xml"
Delete "$INSTDIR\QuickStart_files\image001.png"
Delete "$INSTDIR\QuickStart_files\image002.jpg"
Delete "$INSTDIR\QuickStart_files\image003.png"
Delete "$INSTDIR\QuickStart_files\image004.jpg"
Delete "$INSTDIR\QuickStart_files\image005.png"
Delete "$INSTDIR\QuickStart_files\image006.jpg"
Delete "$INSTDIR\QuickStart_files\image007.png"
Delete "$INSTDIR\QuickStart_files\image008.jpg"
Delete "$INSTDIR\QuickStart_files\image009.png"
Delete "$INSTDIR\QuickStart_files\image010.jpg"
Delete "$INSTDIR\QuickStart_files\image011.png"
Delete "$INSTDIR\QuickStart_files\image012.jpg"
Delete "$INSTDIR\QuickStart_files\image013.png"
Delete "$INSTDIR\QuickStart_files\image014.jpg"
Delete "$INSTDIR\QuickStart_files\image015.png"
Delete "$INSTDIR\QuickStart_files\image016.jpg"
Delete "$INSTDIR\QuickStart_files\image017.png"
Delete "$INSTDIR\QuickStart_files\image018.jpg"
Delete "$INSTDIR\QuickStart_files\Thumbs.db"
Delete "$INSTDIR\autoMicromanager Documented Class Library.chm"
Delete "$INSTDIR\ClassDiagram.bmp"
Delete "$INSTDIR\MicroscopyToolkitHelpfile.pdf"
Delete "$R1\menus\Controls\autoMicromanager\AllDeviceHolders.ctl"
Delete "$R1\menus\Controls\autoMicromanager\ChannelSetupControl.ctl"
Delete "$R1\menus\Controls\autoMicromanager\Joystick.ctl"
Delete "$R1\menus\Controls\autoMicromanager\NIEasyCore.ctl"
Delete "$R1\menus\Controls\autoMicromanager\NIImageProcessor.ctl"
Delete "$R1\menus\Controls\autoMicromanager\NILUTGraph.ctl"
Delete "$R1\menus\Controls\autoMicromanager\NIPropList.ctl"
Delete "$R1\menus\Controls\autoMicromanager\PictureBoard.ctl"
Delete "$R1\menus\Controls\autoMicromanager\SingleDeviceHolder.ctl"
Delete "$R1\menus\Controls\DotNet & ActiveX\autoMicromanager\AllDeviceHolders.ctl"
Delete "$R1\menus\Controls\DotNet & ActiveX\autoMicromanager\ChannelSetupControl.ctl"
Delete "$R1\menus\Controls\DotNet & ActiveX\autoMicromanager\Joystick.ctl"
Delete "$R1\menus\Controls\DotNet & ActiveX\autoMicromanager\NIEasyCore.ctl"
Delete "$R1\menus\Controls\DotNet & ActiveX\autoMicromanager\NIImageProcessor.ctl"
Delete "$R1\menus\Controls\DotNet & ActiveX\autoMicromanager\NILUTGraph.ctl"
Delete "$R1\menus\Controls\DotNet & ActiveX\autoMicromanager\NIPropList.ctl"
Delete "$R1\menus\Controls\DotNet & ActiveX\autoMicromanager\PictureBoard.ctl"
Delete "$R1\menus\Controls\DotNet & ActiveX\autoMicromanager\SingleDeviceHolder.ctl"
Delete "$INSTDIR\Controls not Loaded.txt"
Delete "$INSTDIR\FindLibraryAndDevice.vi"
Delete "$INSTDIR\Make Hardware Config File.vi"
Delete "$INSTDIR\MMCoreOnly.vi"
Delete "$INSTDIR\Test.bmp"
Delete "$INSTDIR\Test.tif"
Delete "$INSTDIR\Test2.tif"
Delete "$INSTDIR\TEst2.xml"
Delete "$INSTDIR\TEst2_Desktop.config"
Delete "$INSTDIR\TEst2_full.xml"
Delete "$INSTDIR\Test3.xml"
Delete "$INSTDIR\Test3_Desktop.config"
Delete "$INSTDIR\Test3_full.xml"
Delete "$INSTDIR\Tutorial10_Full Application.vi"
Delete "$INSTDIR\Tutorial1_Start Camera.vi"
Delete "$INSTDIR\Tutorial2_Image Manipulation.vi"
Delete "$INSTDIR\Tutorial3_Image Sequence.vi"
Delete "$INSTDIR\Tutorial4_Image Save.vi"
Delete "$INSTDIR\Tutorial5_Camera and Stage.vi"
Delete "$INSTDIR\Tutorial6_Device Properties.vi"
Delete "$INSTDIR\Tutorial7_Joystick Control.vi"
Delete "$INSTDIR\Tutorial8_Camera_Stage_Filter.vi"
Delete "$INSTDIR\Tutorial9.1_Image Catching.vi"
Delete "$INSTDIR\Tutorial9.2_Register Paint Board.vi"
Delete "$INSTDIR\Tutorial9.3_Image Catching with Channels.vi"
Delete "$INSTDIR\Tutorial9_Channels.vi"
Delete "$INSTDIR\Tutorial_ConfigFile_Device Control.vi"
Delete "$INSTDIR\Tutorial_ConfigFile_Image Capture.vi"
Delete "$INSTDIR\Tutorial_ConfigFile_Load Config File.vi"
Delete "$INSTDIR\Tutorial_ConfigFile_Make Hardware Config File.vi"
Delete "$R1\menus\Controls\autoMicromanager\AllDeviceHolders.ctl"
Delete "$R1\menus\Controls\autoMicromanager\ChannelSetupControl.ctl"
Delete "$R1\menus\Controls\autoMicromanager\LUTGraph.ctl"
Delete "$R1\menus\Controls\autoMicromanager\MMJoyStick.ctl"
Delete "$R1\menus\Controls\autoMicromanager\NIEasyCore.ctl"
Delete "$R1\menus\Controls\autoMicromanager\NIImageProcessor.ctl"
Delete "$R1\menus\Controls\autoMicromanager\NIPropList.ctl"
Delete "$R1\menus\Controls\autoMicromanager\PictureBoard.ctl"
Delete "$R1\menus\Controls\DotNet & ActiveX\autoMicromanager\AllDeviceHolders.ctl"
Delete "$R1\menus\Controls\DotNet & ActiveX\autoMicromanager\ChannelSetupControl.ctl"
Delete "$R1\menus\Controls\DotNet & ActiveX\autoMicromanager\LUTGraph.ctl"
Delete "$R1\menus\Controls\DotNet & ActiveX\autoMicromanager\MMJoyStick.ctl"
Delete "$R1\menus\Controls\DotNet & ActiveX\autoMicromanager\NIEasyCore.ctl"
Delete "$R1\menus\Controls\DotNet & ActiveX\autoMicromanager\NIImageProcessor.ctl"
Delete "$R1\menus\Controls\DotNet & ActiveX\autoMicromanager\NIPropList.ctl"
Delete "$R1\menus\Controls\DotNet & ActiveX\autoMicromanager\PictureBoard.ctl"
Delete "$INSTDIR\Labview\FindLibraryAndDevice.vi"
Delete "$INSTDIR\Labview\Make Hardware Config File.vi"
Delete "$INSTDIR\Labview\MMCoreOnly.vi"
Delete "$INSTDIR\Labview\Test.bmp"
Delete "$INSTDIR\Labview\Test.tif"
Delete "$INSTDIR\Labview\Test2.tif"
Delete "$INSTDIR\Labview\TEst2.xml"
Delete "$INSTDIR\Labview\TEst2_Desktop.config"
Delete "$INSTDIR\Labview\TEst2_full.xml"
Delete "$INSTDIR\Labview\Test3.xml"
Delete "$INSTDIR\Labview\Test3_Desktop.config"
Delete "$INSTDIR\Labview\Test3_full.xml"
Delete "$INSTDIR\Labview\Tutorial10_Full Application.vi"
Delete "$INSTDIR\Labview\Tutorial1_Start Camera.vi"
Delete "$INSTDIR\Labview\Tutorial2_Image Manipulation.vi"
Delete "$INSTDIR\Labview\Tutorial3_Image Sequence.vi"
Delete "$INSTDIR\Labview\Tutorial4_Image Save.vi"
Delete "$INSTDIR\Labview\Tutorial5_Camera and Stage.vi"
Delete "$INSTDIR\Labview\Tutorial6_Device Properties.vi"
Delete "$INSTDIR\Labview\Tutorial7_Joystick Control.vi"
Delete "$INSTDIR\Labview\Tutorial8_Camera_Stage_Filter.vi"
Delete "$INSTDIR\Labview\Tutorial9.1_Image Catching.vi"
Delete "$INSTDIR\Labview\Tutorial9.2_Register Paint Board.vi"
Delete "$INSTDIR\Labview\Tutorial9.3_Image Catching with Channels.vi"
Delete "$INSTDIR\Labview\Tutorial9_Channels.vi"
Delete "$INSTDIR\Labview\Tutorial_ConfigFile_Device Control.vi"
Delete "$INSTDIR\Labview\Tutorial_ConfigFile_Image Capture.vi"
Delete "$INSTDIR\Labview\Tutorial_ConfigFile_Load Config File.vi"
Delete "$INSTDIR\Labview\Tutorial_ConfigFile_Make Hardware Config File.vi"
Delete "$INSTDIR\Mathematica\Example 1_Get 1 Image.txt"
Delete "$INSTDIR\Mathematica\Example 2_Control Device.txt"
Delete "$INSTDIR\Mathematica\Example 3_Capture Images.txt"
Delete "$INSTDIR\Matlab\Example1.m"
Delete "$INSTDIR\Matlab\Example1.txt"
Delete "$INSTDIR\Matlab\Example2.m"
Delete "$INSTDIR\Matlab\Example2.txt"
Delete "$INSTDIR\Matlab\Example3.asv"
Delete "$INSTDIR\Matlab\Example3.m"
Delete "$INSTDIR\Matlab\Example3.txt"
Delete "$INSTDIR\Matlab\ImageProducedHandler.asv"
Delete "$INSTDIR\Matlab\ImageProducedHandler.m"
Delete "$INSTDIR\Python\MicromanagerScript.pyN"
Delete "$INSTDIR\Python\MicromanagerScript_Device.pyN"
Delete "$INSTDIR\Python\MicromanagerScript_Scipy.pyN"
Delete "$INSTDIR\Python\No_GUI_MicromanagerScript.pyN"
Delete "$INSTDIR\Python\Readme.txt"
Delete "$INSTDIR\Scilab\Scilab Readme.html"
Delete "$INSTDIR\VB6\CoreImageConverter.bas"
Delete "$INSTDIR\VB6\Form_MMPictureBoard.frm"
Delete "$INSTDIR\VB6\Form_MMPictureBoard.log"
Delete "$INSTDIR\VB6\ReadMe.txt"
Delete "$INSTDIR\VB6\ScriptForm.frm"
Delete "$INSTDIR\VB6\ScriptForm.frx"
Delete "$INSTDIR\VB6\StartForm.frm"
Delete "$INSTDIR\VB6\VBMicromanager.exe"
Delete "$INSTDIR\VB6\VBMicromanager.frm"
Delete "$INSTDIR\VB6\VBMicromanager.vbp"
Delete "$INSTDIR\VB6\VBMicromanager.vbw"
Delete "$INSTDIR\VB6\VBScriptExample.vbs"

Delete "$SMPROGRAMS\autoMicromanager\Labview\FindLibraryAndDevice.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Labview\Make Hardware Config File.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Labview\MMCoreOnly.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Labview\Tutorial10_Full Application.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Labview\Tutorial1_Start Camera.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Labview\Tutorial2_Image Manipulation.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Labview\Tutorial3_Image Sequence.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Labview\Tutorial4_Image Save.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Labview\Tutorial5_Camera and Stage.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Labview\Tutorial6_Device Properties.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Labview\Tutorial7_Joystick Control.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Labview\Tutorial8_Camera_Stage_Filter.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Labview\Tutorial9.1_Image Catching.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Labview\Tutorial9.2_Register Paint Board.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Labview\Tutorial9.3_Image Catching with Channels.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Labview\Tutorial9_Channels.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Labview\Tutorial_ConfigFile_Device Control.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Labview\Tutorial_ConfigFile_Image Capture.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Labview\Tutorial_ConfigFile_Load Config File.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Labview\Tutorial_ConfigFile_Make Hardware Config File.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Labview\Controls not Loaded.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Labview\FindLibraryAndDevice.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Labview\Make Hardware Config File.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Labview\MMCoreOnly.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Labview\Tutorial10_Full Application.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Labview\Tutorial1_Start Camera.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Labview\Tutorial2_Image Manipulation.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Labview\Tutorial3_Image Sequence.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Labview\Tutorial4_Image Save.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Labview\Tutorial5_Camera and Stage.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Labview\Tutorial6_Device Properties.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Labview\Tutorial7_Joystick Control.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Labview\Tutorial8_Camera_Stage_Filter.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Labview\Tutorial9.1_Image Catching.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Labview\Tutorial9.2_Register Paint Board.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Labview\Tutorial9.3_Image Catching with Channels.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Labview\Tutorial9_Channels.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Labview\Tutorial_ConfigFile_Device Control.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Labview\Tutorial_ConfigFile_Image Capture.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Labview\Tutorial_ConfigFile_Load Config File.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Labview\Tutorial_ConfigFile_Make Hardware Config File.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Mathematica\Example 1_Get 1 Image.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Mathematica\Example 2_Control Device.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Mathematica\Example 3_Capture Images.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Matlab\Example1.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Matlab\Example2.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Matlab\Example3.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Matlab\ImageProducedHandler.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Matlab\Example1.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Matlab\Example2.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Matlab\Example3.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Python\MicromanagerScript.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Python\MicromanagerScript_Device.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Python\MicromanagerScript_Scipy.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Python\No_GUI_MicromanagerScript.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Python\Readme.lnk"
Delete "$SMPROGRAMS\autoMicromanager\Scilab\Scilab Readme.lnk"
Delete "$SMPROGRAMS\autoMicromanager\COM-VB Tutorials\VBMicromanager.lnk"
Delete "$SMPROGRAMS\autoMicromanager\COM-VB Tutorials\VBScriptExample.lnk"
Delete "$SMPROGRAMS\autoMicromanager\COM-VB Tutorials\ReadMe.lnk"

	
RMDir "$INSTDIR\"
RMDir "$INSTDIR\Application Files\DotNetChecker_1_0_0_8\"
RMDir "$INSTDIR\Application Files\DotNetChecker_1_0_0_2\"
RMDir "$INSTDIR\UserData\"
RMDir "$INSTDIR\ConfigFiles\"
RMDir "$INSTDIR\QuickStart_files\"
RMDir "$R1\menus\Controls\autoMicromanager\"
RMDir "$R1\menus\Controls\DotNet & ActiveX\autoMicromanager\"
RMDir "$INSTDIR\Labview\"
RMDir "$INSTDIR\Mathematica\"
RMDir "$INSTDIR\Matlab\"
RMDir "$INSTDIR\Python\"
RMDir "$INSTDIR\Scilab\"
RMDir "$INSTDIR\VB6\"

  
	empty3:
	; Remove remaining directories
	RMDir "$SMPROGRAMS\autoMicromanager\Labview"
  RMDir "$SMPROGRAMS\autoMicromanager\Mathematica"
  RMDir "$SMPROGRAMS\autoMicromanager\Matlab"
  RMDir "$SMPROGRAMS\autoMicromanager\Python"
	RMDir "$SMPROGRAMS\autoMicromanager\Tutorials"
	RMDir "$SMPROGRAMS\autoMicromanager\COM-VB Tutorials"
	RMDir "$SMPROGRAMS\autoMicromanager"
	RMDir "$INSTDIR\Tutorials\"
	RMDir "$INSTDIR\ConfigFiles\"
	RMDir "$INSTDIR\Application Files\"
	RMDir "$INSTDIR\"

SectionEnd

; eof
