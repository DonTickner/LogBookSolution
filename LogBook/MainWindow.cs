using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Forms;
using Akka.Actor;
using Akka.Actor.Internal;
using Akka.Configuration;
using LogBook.Akka.Actors;
using LogBook.Exceptions;
using LogBook.Objects;
using LogBook.Objects.Forms;

namespace LogBook
{
    public partial class MainWindow : Form
    {
        public ActorSystem MyActorSystem;
        private readonly PageFactory _pageFactory;

        public MainWindow()
        {
            InitializeComponent();
            SetupActorSystem();

            _pageFactory = new PageFactory(this);
            this.DragDrop += MainWindow_DragDrop;
            this.DragEnter += MainWindow_DragEnter;
        }

        #region Events

        #region Page Forms

        /// <summary>
        /// The action to be completed when the MainWindow's MDIChild Activates
        /// </summary>
        private void MainWindow_MdiChildActivate(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild == null)
            {
                tabLogController.Visible = false;
            }
            else
            {
                this.ActiveMdiChild.WindowState = FormWindowState.Maximized;

                if (this.ActiveMdiChild.Tag == null)
                {
                    TabPage tp = new TabPage(this.ActiveMdiChild.Text)
                    {
                        Tag = this.ActiveMdiChild,
                    };
                    tabLogController.TabPages.Add(tp);
                    tabLogController.SelectedTab = tp;
                    this.ActiveMdiChild.Tag = tp;
                }

                if (!tabLogController.Visible)
                {
                    tabLogController.Visible = true;
                }
            }
        }

        private void page_OnClosed(object sender, FormClosedEventArgs e)
        {
            Page senderPage = sender as Page;
            senderPage.FormClosed -= page_OnClosed;
            FindAndRemoveTabPageByLog(senderPage);
        }

        #endregion

        #region Menu

        /// <summary>
        /// The action to be performed when exit is triggered via the Menu
        /// </summary>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show( "Do you wish to close LogBook?", "Exit LogBook", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        /// <summary>
        /// The action to be performed when New Text Page is triggered via the Menu
        /// </summary>
        private void textPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenNewTextPageSelection();
        }

        /// <summary>
        /// The action to be performed when New Log4Net Page is triggered via the menu
        /// </summary>
        private void log4NetPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenNewLog4NetPageSelection();
        }

        #endregion

        #region Main Window

        /// <summary>
        /// The actions to be performed when the user releases a file drag and drop in the Main Window
        /// </summary>
        private void MainWindow_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                return;
            }

            string[] droppedFilePaths = (string[])e.Data.GetData(DataFormats.FileDrop);
            List<string> filePaths = droppedFilePaths.ToList();

            if (!filePaths.Any())
            {
                return;
            }

            foreach (string filePath in filePaths)
            {
                if (!File.Exists(filePath))
                {
                    continue;
                }

                if (filePath.EndsWith(".txt"))
                {
                    OpenNewTextPage(filePath);
                }

                if (filePath.EndsWith(".config"))
                {
                    OpenNewLog4NetPage(filePath);
                }
            }
        }

        /// <summary>
        /// The actions to be performed when the user drags a file into the Main Window
        /// </summary>
        private void MainWindow_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        #endregion
        
        #endregion

        #region Methods

        /// <summary>
        /// Finds a TabPage within the TabFormController based on the Page attached to it and removes it
        /// </summary>
        /// <param name="pageToRelease">The Page whose TabPage is to be removed.</param>
        public void FindAndRemoveTabPageByLog(Page pageToRelease)
        {
            foreach (TabPage page in tabLogController.TabPages)
            {
                if (page.Tag != pageToRelease)
                {
                    continue;
                }

                RemoveTabPage(page);
                break;
            }
        }

        /// <summary>
        /// Performs any work necessary to remove a TabPage from the TabFormController
        /// </summary>
        /// <param name="tabPage">The tab to be removed.</param>
        private void RemoveTabPage(TabPage tabPage)
        {
            tabLogController.TabPages.Remove(tabPage);
            tabPage.Tag = null;
            tabPage?.Dispose();
        }

        /// <summary>
        /// Presents the user with the File Open Dialog to select a Text File and then opens a new <see cref="Page"/> for it.
        /// </summary>
        private void OpenNewTextPageSelection()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                FileName = "Select a Text File",
                Filter = "Text files (*.txt)|*.txt",
                Title = "Open Text File",
                CheckFileExists = true,
                CheckPathExists = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                OpenNewTextPage(openFileDialog.FileName);
            }
        }

        /// <summary>
        /// Presents the user with the File Open Dialog to select a Log4Net .config file and then opens a new <see cref="Page"/> for it.
        /// </summary>
        private void OpenNewLog4NetPageSelection()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                FileName = "Select a Log4Net Config File",
                Filter = "Config files (*.config)|*.config",
                Title = "Open Log4Net Config",
                CheckFileExists = true,
                CheckPathExists = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                OpenNewLog4NetPage(openFileDialog.FileName);
            }
        }

        /// <summary>
        /// Creates a new <see cref="Page"/> for a generic Text file.
        /// </summary>
        private void OpenNewTextPage(string filePath)
        {
            try
            {
                string fileToOepn = filePath;
                if (!File.Exists(fileToOepn))
                {
                    throw new FileNotFoundException(fileToOepn);
                }

                List<Page> newPages = _pageFactory.CreateTextPage(fileToOepn);
                foreach (Page newPage in newPages)
                {
                    newPage.FormClosed += page_OnClosed;
                    newPage.Show();
                }
            }
            catch (FileNotFoundException fe)
            {
                MessageBox.Show($"File not found at location:\n{fe.Message}\nPlease check the file and try again.", "File Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unknown error occured:\n{ex.Message}.", "Unknown Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Creates a new <see cref="Page"/> for a Log4Net config file.
        /// </summary>
        private void OpenNewLog4NetPage(string filePath)
        {
            try
            {
                string fileToOpen = filePath;
                if (!File.Exists(fileToOpen))
                {
                    throw new FileNotFoundException(fileToOpen);
                }

                List<Page> newPages = _pageFactory.CreateNewLog4NetPage(fileToOpen);
                foreach (Page newPage in newPages)
                {
                    newPage.FormClosed += page_OnClosed;
                    newPage.Show();
                }
            }
            catch (FileNotFoundException fe)
            {
                MessageBox.Show($"File not found at location:\n{fe.Message}\nPlease check the file and try again.", "File Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (InvalidLog4NetConfigException e4c)
            {
                MessageBox.Show($"The Config file selected doesn't conform to log4net SDK guidelines. Error:\n{e4c.Message}", "Log4Net Config Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unknown error occured:\n{ex.Message}.", "Unknown Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Akka

        private void SetupActorSystem()
        {
            var config = ConfigurationFactory.ParseString(@"akka.actor.default-dispatcher.shutdown { timeout = 0 }");
            MyActorSystem = ActorSystem.Create("MyActorSystem", config);
        }

        #endregion

        #endregion

    }
}
