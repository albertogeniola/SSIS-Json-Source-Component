using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.EnterpriseServices.Internal;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace SSISInstaller
{
    static class Program
    {
        private const string BASE_KEY_PATH_32 = @"SOFTWARE\Microsoft\Microsoft SQL Server\{0}\SSIS\Setup\DTSPath";
        private const string BASE_KEY_PATH_64 = @"SOFTWARE\Wow6432Node\Microsoft\Microsoft SQL Server\{0}\SSIS\Setup\DTSPath";
        private const string SUBDIR = @"PipelineComponents\";
        private const string RELATIVE_DLL_DIR = @"libs";
        private static Form2 installationDialog;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }


        public static bool GetDTSInstallationPath(SQLVersion sqlVersion, out string path,Architecture ark)
        {
            string regKey = null;
            if (ark == Architecture.x32)
                regKey = BASE_KEY_PATH_32;
            else if (ark == Architecture.x64)
                regKey = BASE_KEY_PATH_64;
            else
                throw new Exception("Invalid ARK");

            try
            {
                string keyPath = string.Format(regKey, (int)sqlVersion);
                RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(keyPath);
                path = registryKey.GetValue(null).ToString();
                if (path != null)
                    return true;
                else
                    return false;
            }
            catch (Exception e)
            {
                path = null;
                return false;
            }
        }

        public static string GetSQLServerDescription(SQLVersion ver)
        {
            FieldInfo fi = ver.GetType().GetField(ver.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return null;
        }

        // Esegue l'installazione delle versioni passate per argomento.
        public static void PerformInstallation(Dictionary<SQLVersion,List<string>> toInstall)
        {
            installationDialog = new Form2();
            installationDialog.Show();
            // Per componente scelto, esegui l'installazione
            foreach (SQLVersion k in toInstall.Keys)
            {
                List<string> dtsDir = toInstall[k];
                foreach (string s in dtsDir)
                {
                    string str = Path.Combine(s, SUBDIR);
                    // Copy the appropriate DLL into that folder
                    foreach (string dll in Directory.GetFiles(RELATIVE_DLL_DIR))
                    {
                        try
                        {
                            string destFile = Path.Combine(str, Path.GetFileName(dll));
                            File.Copy(dll, destFile, true);
                            installationDialog.Append("File " + dll + " copied to " + destFile);
                            Publish p = new Publish();
                            try
                            {
                                p.GacRemove(dll);
                                installationDialog.Append("Gac Removed");
                            }
                            catch (Exception e)
                            {
                                // In caso di errore, logga semplicemente
                                installationDialog.Append("Cannot unistall "+dll+" from GAC.");
                            }
                            try { 
                                p.GacInstall(dll);
                                installationDialog.Append("DLL installed into the GAC.");
                            }
                            catch (Exception e)
                            {
                                // In caso di errore, logga semplicemente
                                installationDialog.Append("Cannot istall " + dll + " to GAC.");
                            }
                        }
                        catch (Exception e)
                        {
                            // In caso di errore notifica l'utente
                            MessageBox.Show("Error during GAC installation. " + e.Message + " " + e.StackTrace);
                            installationDialog.Append("Error during GAC installation. " + e.Message + " " + e.StackTrace);
                        }
                    }
                }
            }
        }

        public static string GetArchDescription(Architecture ark)
        {
            FieldInfo fi = ark.GetType().GetField(ark.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return null;
        }
    }



}
