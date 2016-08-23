using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Collections.ObjectModel;
using System.IO;
using System.Diagnostics;


namespace VmApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        protected int count = 0;

        private void Google()
        {
            //System.Diagnostics.Process proc = new System.Diagnostics.Process();
            //proc.StartInfo.FileName = "C:\\Program Files\\Google\\Cloud SDK\\Google.bat";
            //proc.StartInfo.WorkingDirectory = "C:\\Program Files\\Google\\Cloud SDK\\";
            //proc.Start();

            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "C:\\Scripts\\Google1.bat";
            psi.WorkingDirectory = "C:\\Program Files\\Google\\Cloud SDK\\";
            psi.Verb = "runas";
            Process.Start(psi);

            ProcessStartInfo psi1 = new ProcessStartInfo();
            psi1.FileName = "C:\\Scripts\\Google2.bat";
            psi1.WorkingDirectory = "C:\\Program Files\\Google\\Cloud SDK\\";
            psi1.Verb = "runas";
            Process.Start(psi1);

            Random random = new Random();
            int randomNumber = random.Next(0, 1000);
            string zz = "machine" + randomNumber.ToString();
 
            System.IO.StreamWriter SW = new System.IO.StreamWriter("C:\\Scripts\\Google3.bat");
            SW.WriteLine("gcloud compute instances create " + zz + " --image debian-7 --zone us-central1-a");
            SW.Flush();
            SW.Close();
            SW.Dispose();
            SW = null;
           // System.Diagnostics.Process.Start("test.bat");

            ProcessStartInfo psi2 = new ProcessStartInfo();
            psi2.FileName = "C:\\Scripts\\Google3.bat";
            psi2.WorkingDirectory = "C:\\Program Files\\Google\\Cloud SDK\\";
            psi2.Verb = "runas";
            Process.Start(psi2);
        }

        private void AzureVM()
        {
            string path = @"c:\Scripts\VMcreate.ps1";
            string VmName = DateTime.Now.ToString();
            RunspaceConfiguration runspaceConfiguration = RunspaceConfiguration.Create();

            using (Runspace runspace = RunspaceFactory.CreateRunspace(runspaceConfiguration))
            {
                runspace.Open();
                RunspaceInvoke scriptInvoker = new RunspaceInvoke(runspace);
                //scriptInvoker.Invoke("Set-ExecutionPolicy Unrestricted");
                Pipeline pipeline = runspace.CreatePipeline();
                Command scriptCommand = new Command(path);
                Collection<CommandParameter> commandParameters = new Collection<CommandParameter>();
                /*
                foreach (string scriptParameter in parameters)
                {
                    CommandParameter commandParm = new CommandParameter(null, scriptParameter);
                    commandParameters.Add(commandParm);
                    scriptCommand.Parameters.Add(commandParm);
                }
                */
                pipeline.Commands.Add(scriptCommand);
                Collection<PSObject> psObjects;
                psObjects = pipeline.Invoke();
            }

        }

        Collection<PSObject> RunPsScript(string psScriptPath)
        {
            string psScript = string.Empty;
            if (File.Exists(psScriptPath))
                psScript = File.ReadAllText(psScriptPath);
            else
                throw new FileNotFoundException("Wrong path for the script file");

            Runspace runSpace = RunspaceFactory.CreateRunspace();
            runSpace.Open();

            RunspaceInvoke runSpaceInvoker = new RunspaceInvoke(runSpace);
            runSpaceInvoker.Invoke("Set-ExecutionPolicy Unrestricted");

            Pipeline pipeLine = runSpace.CreatePipeline();
            pipeLine.Commands.AddScript(psScript);
            pipeLine.Commands.Add("Out-String");

            Collection<PSObject> returnObjects = pipeLine.Invoke();
            runSpace.Close();

            return returnObjects;
        
        
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (count % 2 == 0)
            {
                Google();
            }
            else
            {
                AzureVM();
            }
            count++;
            
        }
    }
}
