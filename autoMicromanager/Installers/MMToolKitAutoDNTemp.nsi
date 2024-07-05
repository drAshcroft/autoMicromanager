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
;!define MUI_FINISHPAGE_RUN "$INSTDIR\Micromanager_App.exe"
    !define MUI_FINISHPAGE_NOAUTOCLOSE
    !define MUI_FINISHPAGE_RUN
    !define MUI_FINISHPAGE_RUN_CHECKED
    !define MUI_FINISHPAGE_RUN_TEXT "Check Computer For .NET readiness"
    !define MUI_FINISHPAGE_RUN_FUNCTION "LaunchLink"
    !define MUI_FINISHPAGE_SHOWREADME_NOTCHECKED
    !define MUI_FINISHPAGE_SHOWREADME $INSTDIR\readme.txt


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


Function LaunchLink
  
  ExecShell "" "$INSTDIR\setup.exe"
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
	
	
	<%InsertFiles "$INSTDIR\" "..\MM_Net\bin\Debug\"%>
	<%InsertFiles "$INSTDIR\" "DotNetChecked\Publish\"%>
	<%InsertFiles "$INSTDIR\" "..\3rdParty\Python.net Runtime\"%>
	<%InsertFiles "$INSTDIR\" "..\3rdParty\CPPOriginalMicromanager\"%>
	
	CreateShortCut "$DESKTOP\autoMicromanager.lnk" "$INSTDIR\Micromanager_App.exe"
	
	
	CreateDirectory "$SMPROGRAMS\autoMicromanager"
	CreateShortCut "$SMPROGRAMS\autoMicromanager\Micromanager.NET.lnk" "$INSTDIR\Micromanager_App.exe"
	CreateShortCut "$SMPROGRAMS\autoMicromanager\Uninstall.lnk" "$INSTDIR\uninstall.exe"
	CreateShortCut "$SMPROGRAMS\autoMicromanager\Micromanager.NET Console.lnk" "$INSTDIR\Micromanager_Console.exe"
	CreateShortCut "$SMPROGRAMS\autoMicromanager\Create Hardware Config.lnk" "$INSTDIR\HardwareConfig.exe"
	
	<%InsertFiles "$INSTDIR\" "..\Tutorials\"%>
	CreateShortCut "$SMPROGRAMS\autoMicromanager\Tutorial-Manual.lnk" "$INSTDIR\MicroscopyToolkitHelpfile.pdf"
  CreateShortCut "$SMPROGRAMS\autoMicromanager\Class Documentation.lnk" "$INSTDIR\autoMicromanager Documented Class Library.chm"
	
	
	Call FindRegasm
SectionEnd

Section "Labview Tutorials 8.6" Section2
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
	<%InsertFiles "$R1\menus\Controls\autoMicromanager\" "..\LabViewMMControls86\"%>
	<%InsertFiles "$R1\menus\Controls\DotNet & ActiveX\autoMicromanager\" "..\LabViewMMControls86\"%>
	
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

  <%InsertFiles "$INSTDIR\Labview\" "..\LabviewMM86\"%>
	; Set Section Files and Shortcuts
	
	
  CreateDirectory "$SMPROGRAMS\autoMicromanager\Labview"
	<%InsertShortCuts "$SMPROGRAMS\autoMicromanager\Labview\" "$INSTDIR\Labview\" "..\LabViewMM86\*.vi"%>
	
	;CreateShortCut "$SMPROGRAMS\Micromanager.NET\Labview\Tutorial1.lnk" "$INSTDIR\Labview\Tutorial1.vi"
	
	
	empty2:
SectionEnd

Section /o "Labview Tutorials 8.0" Section3
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
	;<%;;;Insert;Files "$R1\menus\Controls\autoMicromanager\" "..\LabViewMMControls80\"%>
	;<%;;;Insert;Files "$R1\menus\Controls\DotNet & ActiveX\autoMicromanager\" "..\LabViewMMControls80\"%>
	
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

  <%InsertFiles "$INSTDIR\" "..\LabviewMM80\"%>
	; Set Section Files and Shortcuts
	
	
  CreateDirectory "$SMPROGRAMS\autoMicromanager\Labview"
	<%InsertShortCuts "$SMPROGRAMS\autoMicromanager\Labview\" "$INSTDIR\" "..\LabViewMM80\*.vi"%>
	
	;CreateShortCut "$SMPROGRAMS\Micromanager.NET\Labview\Tutorial1.lnk" "$INSTDIR\Labview\Tutorial1.vi"
	
	
	empty2:
SectionEnd

Section "Mathematica Tutorials" Section4

  
	SetOverwrite on

  <%InsertFiles "$INSTDIR\Mathematica\" "..\Mathematica\"%>
	; Set Section Files and Shortcuts
	
	
  CreateDirectory "$SMPROGRAMS\autoMicromanager\Mathematica"
	<%InsertShortCuts "$SMPROGRAMS\autoMicromanager\Mathematica\" "$INSTDIR\Mathematica\" "..\Mathematica\*.txt"%>
	
	;CreateShortCut "$SMPROGRAMS\Micromanager.NET\Labview\Tutorial1.lnk" "$INSTDIR\Labview\Tutorial1.vi"
	
	
	
	
	
	empty4:
SectionEnd

Section "Matlab Tutorials" Section5

 
	SetOverwrite on

  <%InsertFiles "$INSTDIR\Matlab\" "..\Matlab\"%>
	; Set Section Files and Shortcuts
	
	
  CreateDirectory "$SMPROGRAMS\autoMicromanager\Matlab"
	<%InsertShortCuts "$SMPROGRAMS\autoMicromanager\Matlab\" "$INSTDIR\Matlab\" "..\Matlab\*.m"%>
	
	;CreateShortCut "$SMPROGRAMS\Micromanager.NET\Labview\Tutorial1.lnk" "$INSTDIR\Labview\Tutorial1.vi"
	
	
	
	empty5:
SectionEnd

Section "Python Tutorials" Section6

  
	WriteRegStr HKCR ".pyn" "" "PythonNet.Script"
	WriteRegStr HKCR ".pyN" "" "PythonNet.Script"
  WriteRegStr HKCR "PythonNet.Script" "" "Python For .Net Script"
  WriteRegStr HKCR "PythonNet.Script\DefaultIcon" "" "$INSTDIR\PythonN.exe,1"
	WriteRegStr HKCR "PythonNet.Script\shell" "" "open"
	WriteRegStr HKCR "PythonNet.Script\shell\open\command" "" '$INSTDIR\PythonN.exe "%1"'
	
	SetOverwrite on

  <%InsertFiles "$INSTDIR\Python\" "..\python2.5-UCS2\"%>
	; Set Section Files and Shortcuts
	
	
  CreateDirectory "$SMPROGRAMS\autoMicromanager\Python"
	<%InsertShortCuts "$SMPROGRAMS\autoMicromanager\Python\" "$INSTDIR\Python\" "..\python2.5-UCS2\*.py"%>
	<%InsertShortCuts "$SMPROGRAMS\autoMicromanager\Python\" "$INSTDIR\Python\" "..\python2.5-UCS2\*.pyn"%>
	
	;CreateShortCut "$SMPROGRAMS\Micromanager.NET\Labview\Tutorial1.lnk" "$INSTDIR\Labview\Tutorial1.vi"
	
	System::Call 'Shell32::SHChangeNotify(i ${SHCNE_ASSOCCHANGED}, i ${SHCNF_IDLIST}, i 0, i 0)'
	
	empty6:
SectionEnd

Section "(D)COM/VB Tutorials" Section7

  
		
	SetOverwrite on

  <%InsertFiles "$INSTDIR\VB6\" "..\VB6MM\"%>
	; Set Section Files and Shortcuts
	
	
  CreateDirectory "$SMPROGRAMS\autoMicromanager\COM-VB Tutorials"
	<%InsertShortCuts "$SMPROGRAMS\autoMicromanager\COM-VB Tutorials\" "$INSTDIR\VB6\" "..\VB6MM\*.vbp"%>
	<%InsertShortCuts "$SMPROGRAMS\autoMicromanager\COM-VB Tutorials\" "$INSTDIR\VB6\" "..\VB6MM\*.txt"%>
	
	
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
	
	
	<%DeleteAllFiles %>
	<%DeleteAllShortcuts %>
	
	<% DeleteAllDirectories %>
  
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