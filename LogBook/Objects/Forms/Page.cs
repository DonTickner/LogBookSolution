using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Akka.Actor;
using LogBook.Akka.Actors;

namespace LogBook.Objects.Forms
{
    /// <summary>
    /// A page of Log information.
    /// </summary>
    public partial class Page
    {
        /// <summary>
        /// The Main Window to whom this <see cref="Page"/> relates.
        /// </summary>
        public MainWindow ParentMainWindow { get; set; }
        /// <summary>
        /// The detailed breakdown of the Log file that this <see cref="Page"/> is displaying.
        /// </summary>
        public LogFileSignature LogFileSignature { get; set; }

        private ArrowDirection _newLineStartDirection = ArrowDirection.Down;

        private IActorRef _fileTailActor;
        private int _previousRowCount = 0;
        private string _fileName = string.Empty;

        private TimeSpan _initialLoadTime;
        private DateTime _previousTime;

        private int _initialInterval = 250;
        private int _liveInterval = 100;

        private List<int> _offsetRows = new List<int>();

        public sealed override string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

        public Page()
        {
            InitializeComponent();
        }

        #region Events

        /// <summary>
        /// The actions to be performed when the user selects the Dock/Undock button.
        /// </summary>
        private void PageDockButton_Click(object sender, EventArgs e)
        {
            if (this.MdiParent == null)
            {
                this.ControlBox = false;
                this.MdiParent = this.ParentMainWindow;
                pageDockButton.ImageIndex = 1;
            }
            else
            {
                this.MdiParent = null;
                this.Location = new Point(Cursor.Position.X - (this.Width / 2), Cursor.Position.Y);
                this.Size = ParentMainWindow.Size;
                this.ParentMainWindow.FindAndRemoveTabPageByLog(this);
                this.Tag = null;
                this.ControlBox = true;
                pageDockButton.ImageIndex = 0;
            }
        }

        /// <summary>
        /// The actions to be performed when the user selects the Play control button.
        /// </summary>
        private void PlayControlButton_CheckedChanged(object sender, EventArgs e)
        {
            playControlButton.ImageIndex = playControlButton.Checked ? 2 : 3;
            Text = playControlButton.Checked ? $"{_fileName} - Paused" : _fileName;
            nextRecordButton.Enabled = playControlButton.Checked;
        }

        /// <summary>
        /// The actions to be performed when the virtual DataGridView requests a cell value
        /// </summary>
        private void OnCellValueNeeded(object sender,
            DataGridViewCellValueEventArgs e)
        {
            // if (e.RowIndex < resultsDataGridView.RowCount - _offsetRows.Count)
            if (e.RowIndex < resultsDataGridView.RowCount)
            {
                int orientatedRowIndex = _newLineStartDirection == ArrowDirection.Down
                    ? e.RowIndex
                    : resultsDataSet.Tables[0].Rows.Count - e.RowIndex - 1;

                int rowOffset = playControlButton.Checked ? (resultsDataSet.Tables[0].Rows.Count - _previousRowCount) : 0;


                int finalRowIndex = _newLineStartDirection == ArrowDirection.Down
                    ? orientatedRowIndex
                    : orientatedRowIndex - rowOffset;

                // int rowIndex = GetRowOffset(e.RowIndex + rowOffset);
                try
                {
                    e.Value = resultsDataSet.Tables[0].DefaultView.Table.Rows[finalRowIndex][e.ColumnIndex];
                    //while (resultsDataSet.Tables[0].DefaultView.Table.Rows[rowIndex][3].ToString().ToLower() == "debug")
                    //{
                    //    if (!_offsetRows.Contains(rowIndex))
                    //    {
                    //        _offsetRows.Add(rowIndex);
                    //    }
                        
                    //    e.Value = resultsDataSet.Tables[0].DefaultView.Table.Rows[rowIndex++][e.ColumnIndex];
                    //}
                    
                }
                catch
                {
                    e.Value = "MEMORY ERROR";
                }
            }
        }

        /// <summary>
        /// The actions to be performed to determine if new undisplayed rows now exist
        /// </summary>
        private void UpdateTimerUpdate_Tick_NewRows(object sender, EventArgs e)
        {
            if (!playControlButton.Checked)
            {
                resultsProgressBar.Visible = resultsDisplayedLabel.Visible = false;
                return;
            }

            resultsProgressBar.Visible = resultsDisplayedLabel.Visible = _previousRowCount != resultsDataSet.Tables[0].Rows.Count;
            if (!resultsDisplayedLabel.Visible)
            {
                return;
            }

            resultsProgressBar.Minimum = 0;
            resultsProgressBar.Maximum = resultsDataSet.Tables[0].Rows.Count;
            resultsProgressBar.Value = resultsDataGridView.RowCount;
            resultsDisplayedLabel.Text = $" {resultsDataSet.Tables[0].Rows.Count - _previousRowCount} New rows not displayed.";
        }

        /// <summary>
        /// The actions to be performed during the initial load of the file.
        /// </summary>
        private void UpdateTimerUpdate_Tick_InitialRead(object sender, EventArgs e)
        {
            if (null == resultsDataGridView || null == resultsDataSet.Tables[0])
            {
                return;
            }

            if (_previousRowCount != resultsDataSet.Tables[0].Rows.Count)
            {
                _previousRowCount = resultsDataSet.Tables[0].Rows.Count;
                DateTime currentTime = DateTime.Now;
                _initialLoadTime += (currentTime - _previousTime);
                _previousTime = currentTime;
                return;
            }

            updateTimer.Tick -= UpdateTimerUpdate_Tick_InitialRead;
            updateTimer.Tick += UpdateTimerUpdate_Tick_NewRows;
            updateTimer.Tick += UpdateTimerUpdate_Tick_Refresh;
            updateTimer.Interval = _liveInterval;

            firstRecordButton.Enabled = true;
            lastRecordButton.Enabled = true;
            playControlButton.Enabled = true;
            startPositionButton.Enabled = true;

            resultsProgressBar.Visible = false;
            resultsProgressBar.Style = ProgressBarStyle.Blocks;

            loadingPanel.Visible = false;

            resultsLabel.Text = $"Lines: {resultsDataSet.Tables[0].Rows.Count.ToString("##,###")} (Loaded in {_initialLoadTime})";
            resultsDataGridView.RowCount = resultsDataSet.Tables[0].Rows.Count;
            resultsDataGridView.Show();
            resultsDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
        }

        /// <summary>
        /// The actions to be performed when refreshing the UI
        /// </summary>
        private void UpdateTimerUpdate_Tick_Refresh(object sender, EventArgs e)
        {
            if (_previousRowCount == resultsDataSet.Tables[0].Rows.Count ||
                playControlButton.Checked)
            {
                return;
            }

            _previousRowCount = resultsDataGridView.RowCount = resultsDataSet.Tables[0].Rows.Count;
            resultsLabel.Text = $"Lines: {resultsDataSet.Tables[0].Rows.Count.ToString("##,###")}";
            if (_newLineStartDirection == ArrowDirection.Down)
            {
                resultsDataGridView.FirstDisplayedScrollingRowIndex = resultsDataGridView.RowCount - 1;
            }
            resultsDataGridView.Refresh();
        }

        /// <summary>
        /// The actions to be performed when the form closes.
        /// </summary>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            _fileTailActor.Tell(new FileTailActor.GoNowAndDieInWhateverMannerSeemsBestToYou());

            resultsDataGridView.CellValueNeeded -= OnCellValueNeeded;
            updateTimer.Stop();
            updateTimer.Tick -= UpdateTimerUpdate_Tick_Refresh;
            updateTimer.Tick -= UpdateTimerUpdate_Tick_NewRows;

            this.FormClosing -= OnFormClosing;
            pageDockButton.Click -= PageDockButton_Click;
            playControlButton.CheckedChanged -= PlayControlButton_CheckedChanged;
        }

        /// <summary>
        /// The actions to be performed when the user clicks the Next Record button.
        /// </summary>
        private void NextRecordButton_Click(object sender, EventArgs e)
        {
            if ((resultsDataSet.Tables[0].Rows.Count - _previousRowCount) <= 0)
            {
                return;
            }

            if (!nextRecordButton.Enabled)
            {
                return;
            }

            _previousRowCount++;

            if (resultsDataGridView.RowCount < _previousRowCount)
            {
                resultsDataGridView.RowCount += 1;
            }

            resultsDataGridView.FirstDisplayedScrollingRowIndex = _newLineStartDirection == ArrowDirection.Down ? resultsDataGridView.RowCount - 1 : 0;
            resultsLabel.Text = $"Lines: {resultsDataSet.Tables[0].Rows.Count}";
            resultsDataGridView.Refresh();
        }

        /// <summary>
        /// The actions to be performed when the user selects the Last Record button.
        /// </summary>
        private void lastRecordButton_Click(object sender, EventArgs e)
        {
            resultsDataGridView.FirstDisplayedScrollingRowIndex = resultsDataGridView.RowCount - 1;
        }

        /// <summary>
        /// The actions to be performed when the user selects the First Record button.
        /// </summary>
        private void firstRecordButton_Click(object sender, EventArgs e)
        {
            resultsDataGridView.FirstDisplayedScrollingRowIndex = 0;
        }

        /// <summary>
        /// The actions to be performed when the use double clicks the divider of the DataGridView
        /// </summary>
        private void ResultsDataGridView_ColumnDividerDoubleClick(object sender, DataGridViewColumnDividerDoubleClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                resultsDataGridView.AutoResizeColumn(e.ColumnIndex, DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader);
            }
            e.Handled = true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs the necessary setup to new-up a Page
        /// </summary>
        public void Setup(bool doubleBufferGridView = false)
        {
            MdiParent = ParentMainWindow;
            this.ControlBox = false;

            updateTimer.Tick += UpdateTimerUpdate_Tick_InitialRead;
            updateTimer.Interval = _initialInterval;

            // TODO: Enable setting via config
            if (doubleBufferGridView)
            {
                Type dgvType = resultsDataGridView.GetType();
                PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                    BindingFlags.Instance | BindingFlags.NonPublic);
                pi.SetValue(resultsDataGridView, true, null);
            }

            this.FormClosing += OnFormClosing;

            resultsDataGridView.VirtualMode = true;
            resultsDataGridView.ReadOnly = true;
            resultsDataGridView.ColumnCount = LogFileSignature.NumberOfFieldsBeforeMessage + 1;
            resultsDataGridView.Rows.Add();
            resultsDataGridView.Rows.AddCopies(0, 1);

            resultsDataGridView.CellValueNeeded += OnCellValueNeeded;
            resultsDataGridView.ColumnDividerDoubleClick += ResultsDataGridView_ColumnDividerDoubleClick;
            resultsDataGridView.AllowUserToResizeColumns = true;
            resultsDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCellsExceptHeader;
            resultsDataGridView.Hide();
            
            resultsDataSet.Tables.Add(new DataTable());
            for (int i = 0; i <= LogFileSignature.NumberOfFieldsBeforeMessage; i++)
            {
                resultsDataSet.Tables[0].Columns.Add(new DataColumn());
            }

            _fileName = Path.GetFileName(LogFileSignature.FileLocation);
            Text = _fileName;

            Props fileTailActorProps = Props.Create(() => new FileTailActor(resultsDataSet.Tables[0], LogFileSignature));
            _fileTailActor = ParentMainWindow.MyActorSystem.ActorOf(fileTailActorProps, $"fileTailUpdate{Guid.NewGuid()}");

            _previousTime = DateTime.Now;
            this.button1.Enabled = false;
        }

        private void DisplayExceptionMessageBox(string bodyText, string title)
        {
            MessageBox.Show($"{bodyText}", $"{title}", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            //resultsDataSet.Tables[0].DefaultView.RowFilter = "Column4 = 'Debug'";
            //resultsDataGridView.Refresh();
        }

        /// <summary>
        /// Retrieves the offset value of the row index based on filtered columns
        /// </summary>
        /// <param name="row">The beginning row index</param>
        private int GetRowOffset(int row)
        {
            int offset = 0;
            for (int i = 0; i < _offsetRows.Count; i++)
            {
                if (row < _offsetRows[i] &&
                    !_offsetRows.Contains(row + i))
                {
                    return row + i;
                }

                offset++;
            }

            return row + offset;
        }

        private void startPositionButton_Click(object sender, EventArgs e)
        {
            _newLineStartDirection =
                _newLineStartDirection == ArrowDirection.Down ? ArrowDirection.Up : ArrowDirection.Down;

            startPositionButton.ImageIndex = startPositionButton.ImageIndex == 5 ? 6 : 5;
            resultsDataGridView.Refresh();
        }
    }
}
