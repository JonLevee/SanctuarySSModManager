@echo off
for /f "usebackq" %%a in (`dir /s /b bin obj`) do (
	rmdir %%a /s /q
)
 