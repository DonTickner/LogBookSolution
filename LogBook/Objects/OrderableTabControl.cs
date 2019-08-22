using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LogBook.Objects.Forms;

namespace LogBook.Objects
{
    /// <summary>
    /// An extended <see cref="TabControl"/> that allows the Tabs to be ordered by dragging and dropping
    /// </summary>
    public class OrderableTabControl: TabControl
    {
        public OrderableTabControl()
        {
            this.SelectedIndexChanged += new System.EventHandler(this.TabFormController_SelectedIndexChanged);
            this.DragOver += new System.Windows.Forms.DragEventHandler(this.TabFormController_DragOver);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TabFormController_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TabFormController_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TabFormController_MouseUp);
        }
        
        /// <summary>
        /// The action to be performed when the TabFormController's selected Tab is changed
        /// </summary>
        private void TabFormController_SelectedIndexChanged(object sender, EventArgs e)
        {
            (this.SelectedTab?.Tag as Page)?.Select();
        }

        /// <summary>
        /// The action to be performed when a Mouse Hold is performed within the the TabFormController
        /// </summary>
        private void TabFormController_MouseDown(object sender, MouseEventArgs e)
        {
            if (!(sender is TabControl tabFormController))
            {
                throw new Exception("Unknown error occurred when setting TabFormController tag");
            }

            int hoverIndex = GetHoverTabIndex(tabFormController);
            if (hoverIndex >= 0)
            {
                tabFormController.Tag = tabFormController.TabPages[hoverIndex];
            }
        }

        /// <summary>
        /// The action to be performed when a Mouse Hold is released within the TabFormController
        /// </summary>
        private void TabFormController_MouseUp(object sender, MouseEventArgs e)
        {
            if (!(sender is TabControl tabFormController))
            {
                throw new Exception("Unknown error occurred when clearing the TabFormController tag");
            }

            tabFormController.Tag = null;
        }

        /// <summary>
        /// The action to be performed when a Mouse is moved while Left Click is Held within the TabFormController
        /// </summary>
        private void TabFormController_MouseMove(object sender, MouseEventArgs e)
        {
            if (!(sender is TabControl tabFormController))
            {
                throw new Exception("Unknown error occurred when clearing the TabFormController tag");
            }

            // If any button other than the Left mouse button was clicked
            // And if the tabFormController doesn't have its reference Tag set
            if ((e.Button != MouseButtons.Left) || (tabFormController.Tag == null))
            {
                return;
            }

            if (!(tabFormController.Tag is TabPage clickedTab))
            {
                throw new Exception("Unknown error occurred when reading the TabFormController tag");
            }

            tabFormController.DoDragDrop(clickedTab, DragDropEffects.All);
        }

        /// <summary>
        /// The action to be performed when a Drag and Drop is performed within the TabFormController
        /// </summary>
        private void TabFormController_DragOver(object sender, DragEventArgs e)
        {
            if (!(sender is TabControl tabFormController))
            {
                throw new Exception("Unknown error occurred when performing Drag and Drop within the TabFormController");
            }

            if (e.Data.GetData(typeof(TabPage)) == null)
            {
                return;
            }

            if (!(e.Data.GetData(typeof(TabPage)) is TabPage dragTab))
            {
                throw new Exception("Unknown error occurred when determining which Tab has been dragged within the TabFormController");
            }

            int dragTabIndex = tabFormController.TabPages.IndexOf(dragTab);
            int hoverTabIndex = GetHoverTabIndex(tabFormController);

            if (dragTabIndex == hoverTabIndex)
            {
                return;
            }

            if (hoverTabIndex < 0)
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            e.Effect = DragDropEffects.Move;

            SwapTabPages(tabFormController, dragTabIndex, hoverTabIndex);
        }
        
        /// <summary>
        /// Swaps the position of the two TabPages within the passed TabFormController
        /// </summary>
        /// <param name="tabFormController">The TabControl to have its pages swapped</param>
        /// <param name="sourceTabIndex">The index of the source tab to placed at the destination index.</param>
        /// <param name="destinationTabIndex">The index of the tab to be swapped with the source index</param>
        private void SwapTabPages(TabControl tabFormController, int sourceTabIndex, int destinationTabIndex)
        {
            TabPage source = tabFormController.TabPages[sourceTabIndex];
            TabPage destination = tabFormController.TabPages[destinationTabIndex];

            tabFormController.TabPages[sourceTabIndex] = destination;
            tabFormController.TabPages[destinationTabIndex] = source;

            tabFormController.SelectedIndex = destinationTabIndex;

            tabFormController.Refresh();
        }

        /// <summary>
        /// Retrieves the index of the TabPage within the TabControl that contains the Mouse pointer
        /// </summary>
        /// <param name="tabFormController">The TabControl to search for the Mouse within.</param>
        private int GetHoverTabIndex(TabControl tabFormController)
        {
            for (int i = 0; i < tabFormController.TabPages.Count; i++)
            {
                if (tabFormController.GetTabRect(i).Contains(tabFormController.PointToClient(Cursor.Position)))
                    return i;
            }

            return -1;
        }
    }
}
