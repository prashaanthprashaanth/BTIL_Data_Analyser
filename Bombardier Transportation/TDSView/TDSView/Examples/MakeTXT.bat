@echo off
echo ---------------------------------------------------------------------------
echo.
echo  Convert ED_V files into TXT files; Created 25.11.2013 by Bos, PPC Zuerich
echo.
echo ---------------------------------------------------------------------------
echo.

rem ******************************************************************
rem Adjust the path to TDSView.EXE
set TDSVIEWEXE="c:\Program Files\Bombardier Transportation\TDSViewer\TDSView.exe"
rem set TDSVIEWEXE="c:\Program Files\TDSViewer1-4-10-1\TDSView.exe"
rem ******************************************************************


rem check correct path to TDSView.EXE
IF NOT EXIST %TDSVIEWEXE% goto WRONGEXE

echo echo Proceed %%1 > dummy.bat

rem ******************************************************************
rem Adjust the line below with the the Language of the ED_D files, 
rem    the directory path to the ED_D files and optionally the output 
rem    directory: See TDSView Help
echo %TDSVIEWEXE% /TXT /L=DE %%1 D:\MAVIS >> dummy.bat
rem ******************************************************************
 
echo goto Result%%ERRORLEVEL%% >> dummy.bat

echo :Result9 >> dummy.bat
echo   echo -- Event filter doesn't match any available event name >> dummy.bat
echo   goto end  >> dummy.bat

echo :Result8 >> dummy.bat
echo   echo -- Incompatible version between ED_V and ED_D file >> dummy.bat
echo   goto end  >> dummy.bat

echo :Result7 >> dummy.bat
echo   echo -- Wrong number of arguments in command line >> dummy.bat
echo   goto end  >> dummy.bat

echo :Result6 >> dummy.bat
echo   echo -- File is already read >> dummy.bat
echo   goto end  >> dummy.bat

echo :Result5 >> dummy.bat
echo   echo -- Could not create textfile >> dummy.bat
echo   goto end  >> dummy.bat

echo :Result4  >> dummy.bat
echo   echo -- ED_T File not found >> dummy.bat
echo   goto end  	 >> dummy.bat

echo :Result3 >> dummy.bat
echo   echo -- ED_D File not found; Probably Version of ED_D doesn't fit to ED_V Version >> dummy.bat
echo   goto end  	 >> dummy.bat

echo :Result2  >> dummy.bat
echo   echo -- ED_V File not found >> dummy.bat
echo   goto end  	 >> dummy.bat

echo :Result1  >> dummy.bat
echo   echo -- Invalid File; Probably a text file that was already converted  >> dummy.bat
echo   goto end  	 >> dummy.bat

echo :Result0  >> dummy.bat
echo   echo - Ok, Textfile created!  >> dummy.bat
echo   goto end  	 >> dummy.bat


echo :end >> dummy.bat
echo echo. >> dummy.bat

for /r %%i in (ED_V_*.????) do call dummy.bat %%i


del dummy.bat
echo --------------------------------------------------------------------------
echo.
echo Conversion terminated
echo.
echo --------------------------------------------------------------------------

rem Exit on termination
exit

:WRONGEXE
echo ERROR: Wrong Path to TDSView.exe: %TDSVIEWEXE%
echo.
pause


