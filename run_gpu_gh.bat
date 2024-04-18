@echo off

set python_executable=E:\ComfyUI_windows_portable\python_embeded\python.exe
set python_script=E:\ComfyUI_windows_portable\ComfyUI\main.py
set arguments=--windows-standalone-build

"%python_executable%" -s "%python_script%" %arguments%

pause
