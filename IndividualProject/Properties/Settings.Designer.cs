﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IndividualProject.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.9.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Server=localhost; Database = Project1_Individual; User Id = admin; Password = adm" +
            "in")]
        public string connectionString {
            get {
                return ((string)(this["connectionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Users\\giorg\\Documents\\Coding\\AFDEmp\\C#\\Individual Project 1\\CRMTickets\\NewUser" +
            "Requests\\NewUserRequest.txt")]
        public string newUserRequestPath {
            get {
                return ((string)(this["newUserRequestPath"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Users\\giorg\\Documents\\Coding\\AFDEmp\\C#\\Individual Project 1\\CRMTickets\\Technic" +
            "alIssues\\TroubleTicketNotificationToUser_")]
        public string TTnotificationToUser {
            get {
                return ((string)(this["TTnotificationToUser"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Users\\giorg\\Documents\\Coding\\AFDEmp\\C#\\Individual Project 1\\CRMTickets\\NewUser" +
            "Requests")]
        public string newUserRequestFolder {
            get {
                return ((string)(this["newUserRequestFolder"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Users\\giorg\\Documents\\Coding\\AFDEmp\\C#\\Individual Project 1\\CRMTickets\\Technic" +
            "alIssues")]
        public string TTnotificationToUserFolder {
            get {
                return ((string)(this["TTnotificationToUserFolder"]));
            }
        }
    }
}