@echo off
echo ---------------------------------------------------------------------------
echo.
echo  Convert Tab Separated TSV files into Excelfiles; Created 26.11.2013 by Bos, PPC Zuerich
echo.
echo ---------------------------------------------------------------------------
echo.

rem ******************************************************************
rem Adjust the path to TSV2EXCEL.EXE
set TSV2EXCELEXE="c:\Program Files\Bombardier Transportation\TDSViewer\Tsv2Excel.exe"
rem ******************************************************************


rem check correct path to TSV2EXCEL.EXE
IF NOT EXIST %TSV2EXCELEXE% goto WRONGEXE

echo echo Proceed %%1 > dummy.bat

echo %TSV2EXCELEXE% %%1>> dummy.bat
 
echo goto Result%%ERRORLEVEL%% >> dummy.bat


echo :Result4  >> dummy.bat
echo   echo -- Error occured >> dummy.bat
echo   goto end  	 >> dummy.bat

echo :Result3 >> dummy.bat
echo   echo -- Invalid directory path >> dummy.bat
echo   goto end  	 >> dummy.bat

echo :Result2  >> dummy.bat
echo   echo -- File %%1 doesn't exist >> dummy.bat
echo   goto end  	 >> dummy.bat

echo :Result1  >> dummy.bat
echo   echo -- No TSV-Filename given >> dummy.bat
echo   goto end  	 >> dummy.bat

echo :Result0  >> dummy.bat
echo   echo - Ok, xlsx file created!  >> dummy.bat
echo   goto end  	 >> dummy.bat


echo :end >> dummy.bat
echo echo. >> dummy.bat

for /r %%i in (*.TSV) do call dummy.bat %%i


del dummy.bat
echo --------------------------------------------------------------------------
echo.
echo Conversion terminated
echo.
echo --------------------------------------------------------------------------

rem Exit on termination
exit

:WRONGEXE
echo ERROR: Wrong Path to TSV2EXCEL.exe: %TSV2EXCELEXE%
echo.
pause


