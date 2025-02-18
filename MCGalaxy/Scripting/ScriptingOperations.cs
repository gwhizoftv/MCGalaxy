﻿/*
    Copyright 2010 MCLawl Team - Written by Valek (Modified by MCGalaxy)

    Edited for use with MCGalaxy
 
    Dual-licensed under the Educational Community License, Version 2.0 and
    the GNU General Public License, Version 3 (the "Licenses"); you may
    not use this file except in compliance with the Licenses. You may
    obtain a copy of the Licenses at
    
    http://www.opensource.org/licenses/ecl2.php
    http://www.gnu.org/licenses/gpl-3.0.html
    
    Unless required by applicable law or agreed to in writing,
    software distributed under the Licenses are distributed on an "AS IS"
    BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
    or implied. See the Licenses for the specific language governing
    permissions and limitations under the Licenses.
 */
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace MCGalaxy.Scripting 
{    
    public static class ScriptingOperations 
    {   
        const int MAX_LOG = 2;
    	
        /// <summary> Attempts to compile the given source code files into a .dll </summary>
        /// <param name="p"> Player to send messages to </param>
        /// <param name="type"> Type of files being compiled (e.g. Plugin, Command) </param>
        /// <param name="srcs"> Path of the source code files </param>
        /// <param name="dst"> Path to the destination .dll </param>
        /// <returns> The compiler results, or null if compilation failed </returns>
        /// <remarks> If dstPath is null, compiles to an in-memory .dll instead. </remarks>
        public static CompilerResults Compile(Player p, ICompiler compiler, string type, string[] srcs, string dst) {
            foreach (string path in srcs) 
            {
                if (File.Exists(path)) continue;
                
                p.Message("File &9{0} &Snot found.", path);
                return null;
            }
            
            CompilerResults results = compiler.Compile(srcs, dst);
            if (!results.Errors.HasErrors) {
                p.Message("{0} compiled successfully.", type);
                return results;
            }
            
            SummariseErrors(results, srcs, p);
            return null;
        }
        
        static void SummariseErrors(CompilerResults results, string[] srcs, Player p) {
            int logged = 0;
            foreach (CompilerError err in results.Errors) 
            {
                p.Message("&W{1} - {0}", err.ErrorText,
                          ICompiler.DescribeError(err, srcs, " #" + err.ErrorNumber));
                logged++;
                if (logged >= MAX_LOG) break;
            }
            
            if (results.Errors.Count > MAX_LOG) {
                p.Message(" &W.. and {0} more", results.Errors.Count - MAX_LOG);
            }
            p.Message("&WCompilation error. See " + ICompiler.ErrorPath + " for more information.");
        }
    }
}
