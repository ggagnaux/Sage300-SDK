﻿// The MIT License (MIT) 
// Copyright (c) 1994-2018 The Sage Group plc or its licensors.  All rights reserved.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of 
// this software and associated documentation files (the "Software"), to deal in 
// the Software without restriction, including without limitation the rights to use, 
// copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the 
// Software, and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#region Imports
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
#endregion

namespace MergeISVProject
{
	/// <summary>
	/// These are general utility methods
	/// </summary>
	public static class Utilities
	{
		#region Constants
		// IBM437 (OEM United States)
		private const int DefaultCodePage = 437;
		#endregion

		#region Public Methods
		/// <summary>
		/// When this method is called from another class' method,
		/// this method will return the name of the calling
		/// method. How's that for a mouthful!
		/// </summary>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static string GetCurrentMethod()
		{
			var st = new StackTrace();
			var sf = st.GetFrame(1);
			return sf.GetMethod().Name;
		}

		/// <summary>
		/// Get the string equivalant of an ascii character
		/// </summary>
		/// <param name="data">byte array containing the character code</param>
		/// <returns>the string representation of the ascii character</returns>
		public static string ASCII8ToString(byte[] data)
		{
			return Encoding.GetEncoding(DefaultCodePage).GetString(data);
		}

		/// <summary>
		/// Get the string equivalent of an ascii character
		/// </summary>
		/// <param name="data">byte containing the character code</param>
		/// <returns>the string representation of the ascii character</returns>
		public static string ASCII8ToString(byte data)
		{
			var byteArray = new byte[] { data };
			return Encoding.GetEncoding(DefaultCodePage).GetString(byteArray);
		}

        /// <summary>
        /// Get some information from the application
        /// Values are returned via output string parameters
        /// </summary>
        /// <param name="name">Application Name</param>
        /// <param name="ver">Application Version</param>
        /// <param name="copyright">Application Copyright</param>
        /// <param name="buildDate">Application Build Date</param>
        /// <param name="buildYear">Application Build Year</param>
        public static void GetAppInformation(out string name, out string ver, out string copyright, out string buildDate, out string buildYear)
		{
            var currentAssembly = typeof(Program).Assembly;
            var info = FileVersionInfo.GetVersionInfo(typeof(Program).Assembly.Location);
            name = info.InternalName;
            ver = info.ProductVersion;
            copyright = info.LegalCopyright;

            var linkerTime = Assembly.GetExecutingAssembly().GetLinkerTime();
            buildDate = linkerTime.ToString(@"dddd, MMMM dd,yyyy @ HH:mm:ss");
            buildYear = linkerTime.Year.ToString();
        }

        /// <summary>
        /// Get the application timestamp value
        /// This is an extension method for the dotnet 'Assembly' class
        /// </summary>
        /// <param name="assembly">The assembly</param>
        /// <param name="target">The target timezone. Defaults to null</param>
        /// <returns></returns>
        public static DateTime GetLinkerTime(this Assembly assembly, TimeZoneInfo target = null)
        {
            var filePath = assembly.Location;
            const int PeHeaderOffset = 60;
            const int LinkerTimestampOffset = 8;

            var buffer = new byte[2048];

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                stream.Read(buffer, 0, 2048);

            var offset = BitConverter.ToInt32(buffer, PeHeaderOffset);
            var secondsSince1970 = BitConverter.ToInt32(buffer, offset + LinkerTimestampOffset);
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var linkTimeUtc = epoch.AddSeconds(secondsSince1970);

            var tz = target ?? TimeZoneInfo.Local;
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(linkTimeUtc, tz);

            return localTime;
        }
        #endregion
    }
}
