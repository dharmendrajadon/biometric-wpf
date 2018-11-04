using IriTech_Installer.api;
using IriTech_Installer.models;
using System;
using System.Diagnostics;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Windows;

namespace IriTech_Installer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void OnInstallClick(object sender, RoutedEventArgs e)
        {
            IsButtonsEnabled(false);
            String result = await CreateInstallation();

            String content = "RDService Installed \n\n";
            content += "Please plug-in Iris scanner and click on Initialize Button";
            InfoBox.Text = content;

            if (MessageBox.Show("Installation Completed!") == MessageBoxResult.OK)
            {
                IsButtonsEnabled(true);
            }
        }

        private async void OnInitClick(object sender, RoutedEventArgs e)
        {
            IsButtonsEnabled(false);
            String result = await InitScanner();

            DeviceInfo deviceInfo = await ApiService.GetDeviceInfo();
            if (deviceInfo != null && deviceInfo.Additional_info != null && deviceInfo.Additional_info.Param != null)
            {
                bool isDeviceRegistered = await ApiService.IsDeviceRegistered(deviceInfo.Additional_info.Param.Value);
                if (isDeviceRegistered)
                {
                    String content = "RDService Running \n\n";
                    content += "Scanner Initialization Successful \n\n";
                    content += $"Serial Number: {deviceInfo.Additional_info.Param.Value} \n";
                    content += $"Device Code: {deviceInfo.Dc}";
                    InfoBox.Text = content;
                    MessageBox.Show("Scanner Initialized!");
                }
                else
                {
                    String content = "RDService Stopped \n\n";
                    content += "Scanner Initialization Failed \n\n";
                    content += $"Serial Number: {deviceInfo.Additional_info.Param.Value} \n";
                    content += $"Device Code: {deviceInfo.Dc}";
                    InfoBox.Text = content;
                    String unInitResult = await UnInitScanner();
                    MessageBox.Show("Device Not Registered!");
                }
                IsButtonsEnabled(true);
            }
            else
            {
                IsButtonsEnabled(true);

                MessageBox.Show("Scanner Initialization Failed!");
            }
        }

        private async void OnCaptureClick(object sender, RoutedEventArgs e)
        {
            IsButtonsEnabled(false);
            PidData captureData = await ApiService.CaptureRetina();

            if (captureData != null && captureData.Data != null)
            {

                String content = "Iris Data Captured \n\n";
                content += "=========================================\n\n";
                content += $"Score: {captureData.Resp.QScore}\n";
                content += "-- Data-- \n";
                content += captureData.Data;
                InfoBox.Text = content;

                if (MessageBox.Show("Iris Captured") == MessageBoxResult.OK)
                {
                    IsButtonsEnabled(true);
                }
            }
            else
            {
                String content = "Iris Data Capturing Failed \n\n";
                content += "=========================================\n\n";
                content += $"Score: 0\n";
                content += "-- Data-- \n";
                content += "----------------------------";
                InfoBox.Text = content;

                if (MessageBox.Show("Iris Capturing Failed") == MessageBoxResult.OK)
                {
                    IsButtonsEnabled(true);
                }
            }
        }

        private async void OnUnInstallClick(object sender, RoutedEventArgs e)
        {
            IsButtonsEnabled(false);
            String result = await UnInstallService();

            String content = "RDService Removed \n\n";
            content += "Please install again by using Installation Button";
            InfoBox.Text = content;

            if (MessageBox.Show("Service UnInstallation Successful!") == MessageBoxResult.OK)
            {
                IsButtonsEnabled(true);
            }
        }

        private Task<String> CreateInstallation()
        {
            return Task.Run(() =>
            {
                String zipPath = $"{AppDomain.CurrentDomain.BaseDirectory}/resources/Install.zip";
                String basePath = @"C:\Program Files\Iritech";
                String output = null;
                String architecture = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE", EnvironmentVariableTarget.Machine);

                Process process = null;
                try
                {
                    // Extract Files
                    ZipFile.ExtractToDirectory(zipPath, basePath);

                    // Start Process
                    process = new Process();
                    process.StartInfo.FileName = $"{basePath}/install.bat";
                    process.StartInfo.Arguments = String.Format("{0}", architecture);
                    process.StartInfo.Verb = "runas";
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.Start();
                    process.WaitForExit();
                    output = process.StandardOutput.ReadToEnd();
                    process.Close();
                    process = null;
                }
                catch (Exception ex)
                {
                    output = ex.Message;
                }
                finally
                {
                    if (process != null)
                    {
                        process.Close();
                        process = null;
                    }
                }
                return output;
            });
        }

        private Task<String> InitScanner()
        {
            return Task.Run(() =>
            {
                String basePath = @"C:\Program Files\Iritech";
                String output = null;
                Process process = null;
                try
                {
                    // Start Process
                    process = new Process();
                    process.StartInfo.FileName = $"{basePath}/init.bat";
                    process.StartInfo.Verb = "runas";
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.Start();
                    process.WaitForExit();
                    output = process.StandardOutput.ReadToEnd();
                    process.Close();
                }
                catch (Exception ex)
                {
                    output = ex.Message;
                }
                finally
                {
                    if (process != null)
                    {
                        process.Close();
                        process = null;
                    }
                }
                return output;
            });
        }

        private Task<String> UnInitScanner()
        {
            return Task.Run(() =>
            {
                String basePath = @"C:\Program Files\Iritech";
                String output = null;
                Process process = null;
                try
                {
                    // Start Process
                    process = new Process();
                    process.StartInfo.FileName = $"{basePath}/uninit.bat";
                    process.StartInfo.Verb = "runas";
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.Start();
                    process.WaitForExit();
                    output = process.StandardOutput.ReadToEnd();
                    process.Close();
                }
                catch (Exception ex)
                {
                    output = ex.Message;
                }
                finally
                {
                    if (process != null)
                    {
                        process.Close();
                        process = null;
                    }
                }
                return output;
            });
        }

        private Task<String> UnInstallService()
        {
            return Task.Run(() =>
            {
                String basePath = @"C:\Program Files\Iritech";
                String output = null;
                Process process = null;
                try
                {
                    // Start Process
                    process = new Process();
                    process.StartInfo.FileName = $"{basePath}/uninstall.bat";
                    process.StartInfo.Verb = "runas";
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.Start();
                    process.WaitForExit();
                    output = process.StandardOutput.ReadToEnd();
                    process.Close();
                }
                catch (Exception ex)
                {
                    output = ex.Message;
                }
                finally
                {
                    if (process != null)
                    {
                        process.Close();
                        process = null;
                    }
                }
                return output;
            });
        }

        private void IsButtonsEnabled(bool isEnabled)
        {
            InstallButton.IsEnabled = isEnabled;
            InitButton.IsEnabled = isEnabled;
            CaptureButton.IsEnabled = isEnabled;
            UnInstallButton.IsEnabled = isEnabled;
        }

        private void OnLogoClick(object sender, RoutedEventArgs e)
        {
            Process.Start("http://www.biojini.in");
        }
    }
}
