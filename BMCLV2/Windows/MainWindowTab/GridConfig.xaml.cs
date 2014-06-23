﻿using System;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using BMCLV2.Lang;

namespace BMCLV2.Windows.MainWindowTab
{
    /// <summary>
    /// GridConfig.xaml 的交互逻辑
    /// </summary>
    public partial class GridConfig
    {
        public GridConfig()
        {
            InitializeComponent();
        }
        private void btnSaveConfig_Click(object sender, RoutedEventArgs e)
        {
            BmclCore.Config.Autostart = checkAutoStart.IsChecked != null && (bool)checkAutoStart.IsChecked;
            BmclCore.Config.ExtraJvmArg = txtExtJArg.Text;
            BmclCore.Config.Javaw = txtJavaPath.Text;
            BmclCore.Config.Javaxmx = txtJavaXmx.Text;
            BmclCore.Config.Login = listAuth.SelectedItem.ToString();
            BmclCore.Config.LastPlayVer = BmclCore.MainWindow.GridGame.GetSelectedVersion();
            BmclCore.Config.Passwd = Encoding.UTF8.GetBytes(txtPwd.Password);
            BmclCore.Config.Username = txtUserName.Text;
            BmclCore.Config.WindowTransparency = sliderWindowTransparency.Value;
            BmclCore.Config.Report = checkReport.IsChecked != null && checkReport.IsChecked.Value;
            BmclCore.Config.DownloadSource = listDownSource.SelectedIndex;
            BmclCore.Config.Lang = LangManager.GetLangFromResource("LangName");
            Config.Save();
            var dak = new DoubleAnimationUsingKeyFrames();
            dak.KeyFrames.Add(new LinearDoubleKeyFrame(0, TimeSpan.FromSeconds(0)));
            dak.KeyFrames.Add(new LinearDoubleKeyFrame(30, TimeSpan.FromSeconds(0.3)));
            dak.KeyFrames.Add(new LinearDoubleKeyFrame(30, TimeSpan.FromSeconds(2.3)));
            dak.KeyFrames.Add(new LinearDoubleKeyFrame(0, TimeSpan.FromSeconds(2.6)));
            popupSaveSuccess.BeginAnimation(FrameworkElement.HeightProperty, dak);
        }
        private void sliderJavaxmx_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            txtJavaXmx.Text = ((int)sliderJavaxmx.Value).ToString(CultureInfo.InvariantCulture);
        }
        private void txtUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            tip.IsOpen = false;
        }
        private void btnSelectJava_Click(object sender, RoutedEventArgs e)
        {
            var ofJava = new System.Windows.Forms.OpenFileDialog
            {
                RestoreDirectory = true,
                Filter = @"Javaw.exe|Javaw.exe",
                Multiselect = false,
                CheckFileExists = true
            };
            if (ofJava.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtJavaPath.Text = ofJava.FileName;
            }
        }
        private void sliderWindowTransparency_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (BmclCore.MainWindow.top.Background != null)
                BmclCore.MainWindow.top.Background.Opacity = e.NewValue;
        }

        private void listDownSource_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (listDownSource.SelectedIndex)
            {
                case 0:
                    BmclCore.UrlDownloadBase = Resource.Url.URL_DOWNLOAD_BASE;
                    BmclCore.UrlResourceBase = Resource.Url.URL_RESOURCE_BASE;
                    BmclCore.UrlLibrariesBase = Resource.Url.URL_LIBRARIES_BASE;
                    break;
                case 1:
                    BmclCore.UrlDownloadBase = Resource.Url.URL_DOWNLOAD_bangbang93;
                    BmclCore.UrlResourceBase = Resource.Url.URL_RESOURCE_bangbang93;
                    BmclCore.UrlLibrariesBase = Resource.Url.URL_LIBRARIES_bangbang93;
                    break;
                default:
                    goto case 0;
            }
        }
        private void txtJavaXmx_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                sliderJavaxmx.Value = Convert.ToInt32(txtJavaXmx.Text);
            }
            catch (FormatException ex)
            {
                Logger.log(ex);
                MessageBox.Show("请输入一个有效数字");
                txtJavaXmx.Text = (Config.GetMemory() / 4).ToString(CultureInfo.InvariantCulture);
                txtJavaXmx.SelectAll();
            }
        }

        private void txtUserName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                txtPwd.Focus();
                txtPwd.SelectAll();
            }
        }

        private void txtExtJArg_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtExtJArg.Text.IndexOf("-Dfml.ignoreInvalidMinecraftCertificates=true -Dfml.ignorePatchDiscrepancies=true", System.StringComparison.Ordinal) != -1)
            {
                checkOptifine.IsChecked = true;
            }
        }

        private void checkOptifine_Checked(object sender, RoutedEventArgs e)
        {
            if (txtExtJArg.Text.IndexOf("-Dfml.ignoreInvalidMinecraftCertificates=true -Dfml.ignorePatchDiscrepancies=true", System.StringComparison.Ordinal) != -1)
                return;
            txtExtJArg.Text += " -Dfml.ignoreInvalidMinecraftCertificates=true -Dfml.ignorePatchDiscrepancies=true";
        }

        private void checkOptifine_Unchecked(object sender, RoutedEventArgs e)
        {
            txtExtJArg.Text = txtExtJArg.Text.Replace(" -Dfml.ignoreInvalidMinecraftCertificates=true -Dfml.ignorePatchDiscrepancies=true", "");
        }

        private void checkCheckUpdate_Checked(object sender, RoutedEventArgs e)
        {
            BmclCore.Config.CheckUpdate = checkCheckUpdate.IsChecked == true;
        }

        private void comboLang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!BmclCore.MainWindow.loadOk)
                return;
            switch (comboLang.SelectedIndex)
            {
                case 0:
                    LangManager.UseLanguage("zh-cn"); break;
                case 1:
                    LangManager.UseLanguage("zh-tw"); break;
                default:
                    if (comboLang.SelectedItem as string != null)
                        if (BmclCore.Language.ContainsKey(comboLang.SelectedItem as string))
                            LangManager.UseLanguage(BmclCore.Language[comboLang.SelectedItem as string] as string);
                    break;
            }
            BmclCore.MainWindow.changeLanguage();
        }
    }
}