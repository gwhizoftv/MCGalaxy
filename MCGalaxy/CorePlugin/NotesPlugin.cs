﻿/*
    Copyright 2015 MCGalaxy
        
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
using MCGalaxy.Events;

namespace MCGalaxy.Core {

    public sealed class NotesPlugin : Plugin_Simple {
        public override string creator { get { return Server.SoftwareName + " team"; } }
        public override string MCGalaxy_Version { get { return Server.VersionString; } }
        public override string name { get { return "NotesPlugin"; } }

        public override void Load(bool startup) {
            OnModerationActionEvent.Register(HandleModerationAction, Priority.Low, this);
        }
        
        public override void Unload(bool shutdown) {
            OnModerationActionEvent.UnRegister(this);
        }
        
        void HandleModerationAction(ModerationAction action) {
            switch (action.Type) {
                case ModerationActionType.Frozen:
                    AddNote(action, "F"); break;
                case ModerationActionType.Jailed:
                    AddNote(action, "J"); break;
                case ModerationActionType.Kicked:
                    AddNote(action, "K"); break;
                case ModerationActionType.Muted:
                    AddNote(action, "M"); break; 
                case ModerationActionType.Warned:
                    AddNote(action, "W"); break;                    
                case ModerationActionType.Ban:
                    string banType = action.Duration.Ticks == 0 ? "B" : "T";
                    AddNote(action, banType); break;

            }
        }
        
        static void AddNote(ModerationAction action, string type) {
             if (!Server.LogNotes) return;
             string src = action.Actor == null ? "(console)" : action.Actor.name;
             
             string time = DateTime.UtcNow.ToString("dd/MM/yyyy");
             string data = action.Target + " " + type + " " + src + " " + time;
             if (action.Reason != null) {
                 data = " " + action.Reason.Replace(" ", "%20");
             }
             Server.Notes.Append(data);
        }
    }
}
