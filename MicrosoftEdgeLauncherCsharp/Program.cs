using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace MicrosoftEdgeLaucherCsharp
{
    /// <summary>
    /// Class Program, which allows for Edge to be runned out of 
    /// </summary>
    class Program
	{
		/// <summary>
		/// Edge Application Name
		/// </summary>
		private const string EdgeName = "Microsoft.MicrosoftEdge_8wekyb3d8bbwe!MicrosoftEdge";

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        static void Main(string[] args)
		{
			//call to unmanaged code to activate "modern" app
			var launcher = new ApplicationActivationManager();
			Debug.WriteLine("Checking args...");
			uint pid;
			switch (args.Length) {
				case 0:
                    {
                        // Calling Microsoft Edge as is
                        // me_launcher
                        Debug.WriteLine("calling without arguments...");
                        launcher.ActivateApplication(EdgeName, null, ActivateOptions.None, out pid);
                        break;
                    }
				case 1:
					{
                        // Calling microsoft edge to open a website
                        // me_launcher "[website]"
                        var par = args[0];

                        if (par == "-h")
                            Showguide();
                        else {
                            // == Checking the nature of the parameter
                            // Define the regex expressions
                            var regex_url = new Regex(@"^((https?|ftp)\://|www).*$");

                            // Check flags
                            bool sanity = regex_url.IsMatch(par);

                            if (sanity) {
                                // Launch the application
                                launcher.ActivateApplication(EdgeName,
                                        par,
                                        ActivateOptions.None,
                                        out pid);
                            } else {
                                Console.WriteLine("Something went wrong in the address parsing...");
                                Showguide();
                            }
                        }
						break;
					}
                default:
                    {
                        // Calling microsoft edge with options
                        // me_launcher [-opts] "https://[website]"

                        int noptpar = args.Length - 1;
                        bool filemode = false;
                        bool readmode = false;

                        // Parsing the parameters
                        for (int i = 0; i < noptpar; i++) {
                            string par = args[i];

                            if (par == "-r") {
                                readmode = true;
                                continue;
                            }

                            if (par == "-f") {
                                filemode = true;
                                continue;
                            }

                            // If you get here something is wrong...
                            Console.Write("Error parsing the parameters: ");
                            Console.WriteLine("\"" + par + "\" was not recognized as valid parameter!");
                            Showguide();
                        }

                        // Check that both the options have been selected
                        if (filemode && readmode) {
                            Console.WriteLine("It is not possible to open local file in read mode");
                            Showguide();
                            break;
                        }

                        
                        // Extract the target from the parameters
                        string target = args[noptpar];

                        string extended_target = target;
                        if (filemode)
                            extended_target = "file://" + extended_target;
                        
                        if (readmode)
                            extended_target = "read:" + extended_target;

                        // Launch the application
                        launcher.ActivateApplication(EdgeName,
                                extended_target,
                                ActivateOptions.None,
                                out pid);
                        break;
                    }
			} // ENDOFSWITCH

			//Console.Read();
		}

        private static void Showguide()
        { 
            Console.WriteLine("\nThe following application launches the Microsoft Edge browser.");
            Console.WriteLine("It is possible to:");
            Console.WriteLine("\t 1) Launch the Microsoft Edge browser");
            Console.WriteLine("\t 2) Launch the Microsoft Edge browser in reading mode");
            Console.WriteLine("\t 3) Launch the Microsoft Edge browser to open local html/htm files");
            
            Console.WriteLine("Usage:");
            Console.WriteLine("\tme_launcher.exe [-r] [-f] \"[url/path]\" ");
            Console.WriteLine("Options:");
            Console.WriteLine("\t -r:  Launch Microsoft Edge in reading mode.");
            Console.WriteLine("\t -f:  Open a local html/htm file with Microsoft Edge.");
            Console.WriteLine("\t url/path: The URL or the file path to be open in Microsoft Edge.");
            Console.WriteLine("Note: It is not possible to open local files in reading mode.");
        }

        /// <summary>
        /// Enum ActivateOptions
        /// </summary>
        public enum ActivateOptions
		{
			/// <summary>
			/// The none
			/// </summary>
			None = 0x00000000,  // No flags set
			/// <summary>
			/// The design mode
			/// </summary>
			DesignMode = 0x00000001,  // The application is being activated for design mode, and thus will not be able to
			// to create an immersive window. Window creation must be done by design tools which
			// load the necessary components by communicating with a designer-specified service on
			// the site chain established on the activation manager.  The splash screen normally
			// shown when an application is activated will also not appear.  Most activations
			// will not use this flag.
			/// <summary>
			/// The no error UI
			/// </summary>
			NoErrorUI = 0x00000002,  // Do not show an error dialog if the app fails to activate.                                
			/// <summary>
			/// The no splash screen
			/// </summary>
			NoSplashScreen = 0x00000004,  // Do not show the splash screen when activating the app.
		}

		/// <summary>
		/// Interface IApplicationActivationManager
		/// </summary>
		[ComImport, Guid("2e941141-7f97-4756-ba1d-9decde894a3d"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
			interface IApplicationActivationManager
			{
				// Activates the specified immersive application for the "Launch" contract, passing the provided arguments
				// string into the application.  Callers can obtain the process Id of the application instance fulfilling this contract.
				/// <summary>
				/// Activates the application.
				/// </summary>
				/// <param name="appUserModelId">The application user model identifier.</param>
				/// <param name="arguments">The arguments.</param>
				/// <param name="options">The options.</param>
				/// <param name="processId">The process identifier.</param>
				/// <returns>IntPtr.</returns>
				IntPtr ActivateApplication([In] String appUserModelId, [In] String arguments, [In] ActivateOptions options, [Out] out UInt32 processId);
				/// <summary>
				/// Activates for file.
				/// </summary>
				/// <param name="appUserModelId">The application user model identifier.</param>
				/// <param name="itemArray">The item array.</param>
				/// <param name="verb">The verb.</param>
				/// <param name="processId">The process identifier.</param>
				/// <returns>IntPtr.</returns>
				IntPtr ActivateForFile([In] String appUserModelId, [In] IntPtr /*IShellItemArray* */ itemArray, [In] String verb, [Out] out UInt32 processId);
				/// <summary>
				/// Activates for protocol.
				/// </summary>
				/// <param name="appUserModelId">The application user model identifier.</param>
				/// <param name="itemArray">The item array.</param>
				/// <param name="processId">The process identifier.</param>
				/// <returns>IntPtr.</returns>
				IntPtr ActivateForProtocol([In] String appUserModelId, [In] IntPtr /* IShellItemArray* */itemArray, [Out] out UInt32 processId);
			}

		/// <summary>
		/// Class ApplicationActivationManager.
		/// </summary>
		/// <remarks>
		///     implementation was made from community members at stackoverflow http://stackoverflow.com/questions/12925748/iapplicationactivationmanageractivateapplication-in-c
		/// </remarks>
		[ComImport, Guid("45BA127D-10A8-46EA-8AB7-56EA9078943C")]//Application Activation Manager
			class ApplicationActivationManager : IApplicationActivationManager
		{
			/// <summary>
			/// Activates the application.
			/// </summary>
			/// <param name="appUserModelId">The application user model identifier.</param>
			/// <param name="arguments">The arguments.</param>
			/// <param name="options">The options.</param>
			/// <param name="processId">The process identifier.</param>
			/// <returns>IntPtr.</returns>
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)/*, PreserveSig*/]
				public extern IntPtr ActivateApplication([In] String appUserModelId, [In] String arguments, [In] ActivateOptions options, [Out] out UInt32 processId);
			/// <summary>
			/// Activates for file.
			/// </summary>
			/// <param name="appUserModelId">The application user model identifier.</param>
			/// <param name="itemArray">The item array.</param>
			/// <param name="verb">The verb.</param>
			/// <param name="processId">The process identifier.</param>
			/// <returns>IntPtr.</returns>
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
				public extern IntPtr ActivateForFile([In] String appUserModelId, [In] IntPtr /*IShellItemArray* */ itemArray, [In] String verb, [Out] out UInt32 processId);
			/// <summary>
			/// Activates for protocol.
			/// </summary>
			/// <param name="appUserModelId">The application user model identifier.</param>
			/// <param name="itemArray">The item array.</param>
			/// <param name="processId">The process identifier.</param>
			/// <returns>IntPtr.</returns>
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
				public extern IntPtr ActivateForProtocol([In] String appUserModelId, [In] IntPtr /* IShellItemArray* */itemArray, [Out] out UInt32 processId);
		}
	}
}
