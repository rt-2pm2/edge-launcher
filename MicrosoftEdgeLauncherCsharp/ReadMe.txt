The C# application allows to launch the Microsoft Edge browser both on url and on local files.
Actually, the main purpose of this application is to restore the capability to call Microsoft Edge from the command prompt with a local file.
This workaround has become necessary, since the previous method "microsoft-edge: file://filename" does not work anymore. 

With this application it is possible to:
1) Launch the Microsoft Edge browser";
2) Launch the Microsoft Edge browser in reading mode";
3) Launch the Microsoft Edge browser to open local html/htm files";
            
Usage:
me_launcher.exe [-r] [-f] \"[url/path]\" 

Options:
	-r:  Launch Microsoft Edge in reading mode.
	-f:  Open a local html/htm file with Microsoft Edge.
	url/path: The URL or the file path to be open in Microsoft Edge.

Note: It is not possible to open local files in reading mode.
