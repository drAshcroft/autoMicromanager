; Script generated with the Venis Install Wizard

; Define your application name
!define APPNAME "test"
!define APPNAMEANDVERSION "test 0.6"

; Main Install settings
Name "${APPNAMEANDVERSION}"

OutFile "C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\csharp_core 2.1.1\Installers\License.exe"

; Modern interface settings


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
     Goto done
 
   foundDotNET:
     StrCpy $0 $R2
		 MessageBox MB_OK '"$R2" "C:\Program Files\Micromanager.NET\micromanager_net.dll" /tlb /codebase'
     Exec '"$R2" "C:\Program Files\Micromanager.NET\micromanager_net.dll" /silent /tlb /codebase'
		 
     
		 
		 
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

Section "test" Section1


  
  Call FindRegasm
	MessageBox MB_OK "$0"
  
SectionEnd




