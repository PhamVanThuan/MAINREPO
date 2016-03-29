﻿namespace SAHL.Core.Configuration
 {
     public class ApplicationConfigurationProvider : ConfigurationProvider, IApplicationConfigurationProvider
     {
         private static IApplicationConfigurationProvider instance;
         private static readonly object lockObject = new object();

         public string ApplicationName
         {
             get { return this.Config.AppSettings.Settings["ApplicationName"].Value; }
         }

         public static IApplicationConfigurationProvider Instance
         {
             get
             {
                 lock (lockObject)
                 {
                     if (instance == null)
                     {
                         instance = new ApplicationConfigurationProvider();
                     }
                     return instance;
                 }
             }
             set
             {
                 instance = value;
             }
         }
     }
 }