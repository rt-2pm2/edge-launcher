The C# application allows to launch the Microsoft Edge browser both on url and on local files.
Actually, the main purpose of this application is to restore the capability to call Microsoft Edge from the command prompt on a local file.
This workaround has become necessary, since the previous method "microsoft-edge: file://filename" does not work anymore. 

====
To open a file with Edge from the Windows command prompt:
./me_launcher "file://[path_to_file]"
