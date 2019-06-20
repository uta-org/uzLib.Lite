@echo off
for /R %%i in (*.cs) do todos.exe -p -f "%%i"
pause