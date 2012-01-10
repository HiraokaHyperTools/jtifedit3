; example1.nsi
;
; This script is perhaps one of the simplest NSIs you can make. All of the
; optional settings are left to their default settings. The installer simply 
; prompts the user asking them where to install, and drops a copy of example1.nsi
; there. 

!define APP   "jtifedit3"
!define TITLE "J TIFF Editor 3"

!define VER    "1.0.9"
!define APPVER "1_0_9"

!define MIME "image/tiff"

!define EXT ".tif"
!define EXT2 ".tiff"

;--------------------------------

; The name of the installer
Name "${TITLE} -- ${VER}"

; The file to write
OutFile "Setup_${APP}_${APPVER}_user.exe"

; The default installation directory
InstallDir "$APPDATA\${APP}"

; Registry key to check for directory (so if you install again, it will
; overwrite the old one automatically)
InstallDirRegKey HKCU "Software\HIRAOKA HYPERS TOOLS, Inc.\${APP}" "Install_Dir"

; Request application privileges for Windows Vista
RequestExecutionLevel user

AutoCloseWindow true

AllowSkipFiles off

!define DOTNET_VERSION "2.0"

!include "DotNET.nsh"
!include LogicLib.nsh

;--------------------------------

; Pages

Page license
Page directory
Page components
Page instfiles

LicenseData GNUGPL2.txt

UninstPage uninstConfirm
UninstPage instfiles

;--------------------------------

!ifdef SHCNE_ASSOCCHANGED
!undef SHCNE_ASSOCCHANGED
!endif
!define SHCNE_ASSOCCHANGED 0x08000000

!ifdef SHCNF_FLUSH
!undef SHCNF_FLUSH
!endif
!define SHCNF_FLUSH        0x1000

!ifdef SHCNF_IDLIST
!undef SHCNF_IDLIST
!endif
!define SHCNF_IDLIST       0x0000

!macro UPDATEFILEASSOC
  IntOp $1 ${SHCNE_ASSOCCHANGED} | 0
  IntOp $0 ${SHCNF_IDLIST} | ${SHCNF_FLUSH}
; Using the system.dll plugin to call the SHChangeNotify Win32 API function so we
; can update the shell.
  System::Call "shell32::SHChangeNotify(i,i,i,i) ($1, $0, 0, 0)"
!macroend

;--------------------------------

; The stuff to install
Section "${APP}" ;No components page, name is not important
  SectionIn ro

  ; Set output path to the installation directory.
  SetOutPath $INSTDIR

  !insertmacro CheckDotNET ${DOTNET_VERSION}

  ; Put file there
  File "bin\x86\release\jtifedit3.exe"
  File "bin\x86\release\jtifedit3.pdb"
  File ".\MAPISendMailSa.exe"
  File ".\FreeImage.dll"
  File ".\FreeImageNET.dll"
  File "1.ico"

  WriteRegStr HKCU "Software\Classes\${APP}" "" "${TITLE}"
  WriteRegstr HKCU "Software\Classes\${APP}\DefaultIcon" "" "$INSTDIR\1.ico,0"
  WriteRegStr HKCU "Software\Classes\${APP}\shell\open\command" "" '"$INSTDIR\${APP}" "%1"'

  WriteRegStr HKCU "Software\HIRAOKA HYPERS TOOLS, Inc.\${APP}" "Install_Dir" "$INSTDIR"

  ; Write the uninstall keys for Windows
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP}" "DisplayName" "${TITLE}"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP}" "UninstallString" '"$INSTDIR\uninstall.exe"'
  WriteRegDWORD HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP}" "NoModify" 1
  WriteRegDWORD HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP}" "NoRepair" 1
  WriteUninstaller "uninstall.exe"
  
SectionEnd ; end the section

Section "関連付け(現在のアカウント)"
  WriteRegStr HKCU "Software\Classes\${EXT}" "" "${APP}"
  WriteRegStr HKCU "Software\Classes\${EXT}" "Content Type" "${MIME}"
  WriteRegStr HKCU "Software\Classes\${EXT}\OpenWithProgids" "${APP}" ""
  
  WriteRegStr HKCU "Software\Classes\${EXT2}" "" "${APP}"
  WriteRegStr HKCU "Software\Classes\${EXT2}" "Content Type" "${MIME}"
  WriteRegStr HKCU "Software\Classes\${EXT2}\OpenWithProgids" "${APP}" ""

  DeleteRegValue HKCU "Software\Microsoft\Windows\CurrentVersion\Explorer\FileExts\${EXT}" ""
  DeleteRegValue HKCU "Software\Microsoft\Windows\CurrentVersion\Explorer\FileExts\${EXT}" "Progid"
  DeleteRegValue HKCU "Software\Microsoft\Windows\CurrentVersion\Explorer\FileExts\${EXT}" "Application"

  DeleteRegValue HKCU "Software\Microsoft\Windows\CurrentVersion\Explorer\FileExts\${EXT2}" ""
  DeleteRegValue HKCU "Software\Microsoft\Windows\CurrentVersion\Explorer\FileExts\${EXT2}" "Progid"
  DeleteRegValue HKCU "Software\Microsoft\Windows\CurrentVersion\Explorer\FileExts\${EXT2}" "Application"

  DetailPrint "関連付け更新中です。お待ちください。"
  !insertmacro UPDATEFILEASSOC
SectionEnd

Section "スタートメニュー(現在のアカウント)"
  CreateDirectory "$SMPROGRAMS\${TITLE}"
  CreateShortCut "$SMPROGRAMS\${TITLE}\Uninstall.lnk" "$INSTDIR\uninstall.exe" "" "$INSTDIR\uninstall.exe" 0
  CreateShortCut "$SMPROGRAMS\${TITLE}\起動.lnk" "$INSTDIR\${APP}.exe" "" "$INSTDIR\${APP}.exe" 0
SectionEnd

Section "起動"
  SetOutPath $INSTDIR
  Exec "$INSTDIR\jtifedit3.exe"
SectionEnd

;--------------------------------

; Uninstaller

Section "Uninstall"

  ; Remove registry keys
  DeleteRegKey HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP}"
  DeleteRegKey HKCU "Software\HIRAOKA HYPERS TOOLS, Inc.\${APP}"
  
  DeleteRegValue HKCU "Software\Classes\${EXT}\OpenWithProgids" "${APP}"
  DeleteRegValue HKCU "Software\Classes\${EXT2}\OpenWithProgids" "${APP}"

  ReadRegStr $0 HKCU "Software\Classes\${EXT}" ""
  ${If} $0 == "${APP}"
  ReadRegStr $0 HKLM "Software\Classes\${EXT}" ""
  WriteRegStr   HKCU "Software\Classes\${EXT}" "" "$0"
  ${EndIf}

  ReadRegStr $0 HKCU "Software\Classes\${EXT2}" ""
  ${If} $0 == "${APP}"
  ReadRegStr $0 HKLM "Software\Classes\${EXT2}" ""
  WriteRegStr   HKCU "Software\Classes\${EXT2}" "" "$0"
  ${EndIf}

  ; Remove files and uninstaller
  Delete "$INSTDIR\1.ico"
  Delete "$INSTDIR\FreeImage.dll"
  Delete "$INSTDIR\FreeImageNET.dll"
  Delete "$INSTDIR\jtifedit3.exe"
  Delete "$INSTDIR\jtifedit3.pdb"
  Delete "$INSTDIR\MAPISendMailSa.exe"

  DetailPrint "関連付け更新中です。お待ちください。"
  !insertmacro UPDATEFILEASSOC

  Delete "$INSTDIR\uninstall.exe"

  ; Remove shortcuts, if any
  Delete "$SMPROGRAMS\${TITLE}\Uninstall.lnk"
  Delete "$SMPROGRAMS\${TITLE}\起動.lnk"

  ; Remove directories used
  RMDir "$SMPROGRAMS\${TITLE}"
  RMDir "$INSTDIR"

SectionEnd
