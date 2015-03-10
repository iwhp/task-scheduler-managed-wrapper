﻿using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Microsoft.Win32.TaskScheduler
{
	/// <summary>
	/// Displays a <see cref="TaskCollection"/> in a <see cref="ListView"/> control. Mimics list in MMC.
	/// </summary>
	[System.Drawing.ToolboxBitmap(typeof(Microsoft.Win32.TaskScheduler.TaskEditDialog), "TaskControl")]
	public partial class TaskListView : UserControl
	{
		private TaskCollection coll;

		/// <summary>
		/// Initializes a new instance of the <see cref="TaskListView"/> class.
		/// </summary>
		public TaskListView()
		{
			InitializeComponent();
			smallImageList.Images.Add(new System.Drawing.Icon(EditorProperties.Resources.ts, 0x10, 0x10));
		}

		/// <summary>
		/// Occurs when task selected in the list.
		/// </summary>
		public event EventHandler<TaskSelectedEventArgs> TaskSelected;

		/// <summary>
		/// Gets or sets the <see cref="T:System.Windows.Forms.ContextMenuStrip" /> associated with this control.
		/// </summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ContextMenuStrip" /> for this control, or null if there is no <see cref="T:System.Windows.Forms.ContextMenuStrip" />. The default is null.</returns>
		public override ContextMenuStrip ContextMenuStrip
		{
			get { return listView1.ContextMenuStrip; }
			set
			{
				if (listView1.ContextMenuStrip != value)
				{
					if (listView1.ContextMenuStrip != null)
						listView1.ContextMenuStrip.Opening -= listView1ContextMenuStrip_Opening;
					listView1.ContextMenuStrip = value;
					if (value != null)
						listView1.ContextMenuStrip.Opening += listView1ContextMenuStrip_Opening;
				}
			}
		}

		/// <summary>
		/// Gets or sets the zero-based index of the currently selected item in a <see cref="TaskListView"/>.
		/// </summary>
		/// <value>
		/// A zero-based index of the currently selected item. A value of negative one (-1) is returned if no item is selected.
		/// </value>
		public int SelectedIndex
		{
			get
			{
				return listView1.SelectedIndices.Count == 0 ? -1 : listView1.SelectedIndices[0];
			}
			set
			{
				foreach (int i in listView1.SelectedIndices)
					listView1.Items[i].Selected = false;
				if (value != -1)
					listView1.Items[value].Selected = true;
			}
		}

		/// <summary>
		/// Gets or sets the tasks.
		/// </summary>
		/// <value>The tasks.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), RefreshProperties(RefreshProperties.All)]
		public TaskCollection Tasks
		{
			get { return coll; }
			set
			{
				coll = value;
				listView1.BeginUpdate();
				listView1.Items.Clear();
				if (coll != null)
					foreach (var item in coll)
						listView1.Items.Add(LVIFromTask(item));
				listView1.EndUpdate();
			}
		}

		/// <summary>
		/// Retrieves the item at the specified location.
		/// </summary>
		/// <param name="x">The x-coordinate of the location to search for an item (expressed in client coordinates).</param>
		/// <param name="y">The y-coordinate of the location to search for an item (expressed in client coordinates).</param>
		/// <returns>A <see cref="Task"/> that represents the item at the specified position. If there is no item at the specified location, the method returns <c>null</c>.</returns>
		public Task GetItemAt(int x, int y)
		{
			ListViewItem item = listView1.GetItemAt(x, y);
			return item == null ? null : (Task)item.Tag;
		}

		/// <summary>
		/// Raises the <see cref="TaskSelected"/> event.
		/// </summary>
		/// <param name="e">The <see cref="Microsoft.Win32.TaskScheduler.TaskListView.TaskSelectedEventArgs"/> instance containing the event data.</param>
		protected virtual void OnTaskSelected(TaskSelectedEventArgs e)
		{
			EventHandler<TaskSelectedEventArgs> handler = TaskSelected;
			if (handler != null)
				handler(this, e);
		}

		private void listView1ContextMenuStrip_Opening(object sender, CancelEventArgs e)
		{
			if (listView1.SelectedItems.Count <= 0)
				e.Cancel = true;
		}

		private void listView1_MouseClick(object sender, MouseEventArgs e)
		{
			if (this.ContextMenuStrip != null && e.Button == System.Windows.Forms.MouseButtons.Right)
			{
				ListViewItem item = listView1.GetItemAt(e.X, e.Y);
				if (item != null)
				{
					item.Selected = true;
					if (this.ContextMenuStrip != null)
						this.ContextMenuStrip.Show(listView1, e.Location);
				}
			}
		}

		private void listView1_SelectedIndexChanged(object sender, EventArgs e)
		{
			Task t = null;
			if (listView1.SelectedIndices.Count == 1)
				t = (Task)listView1.SelectedItems[0].Tag;
			OnTaskSelected(new TaskSelectedEventArgs(t));
		}

		private ListViewItem LVIFromTask(Task task)
		{
			bool disabled = task.State == TaskState.Disabled;
			TaskDefinition td = null;
			try { td = task.Definition; } catch { }
			ListViewItem lvi = new ListViewItem(new string[] {
				task.Name,
				TaskEnumGlobalizer.GetString(task.State),
				td == null ? "" : task.Definition.Triggers.ToString(),
				disabled || task.NextRunTime < DateTime.Now ? string.Empty : task.NextRunTime.ToString("G"),
				task.LastRunTime == DateTime.MinValue ? EditorProperties.Resources.Never :  task.LastRunTime.ToString("G"),
				task.LastRunTime == DateTime.MinValue ? string.Empty : task.State == TaskState.Running ? string.Format(EditorProperties.Resources.LastResultRunning, task.LastTaskResult) : ((task.LastTaskResult == 0 ? EditorProperties.Resources.LastResultSuccessful : string.Format("(0x{0:X})", task.LastTaskResult))),
				td == null ? "" : task.Definition.RegistrationInfo.Author,
				string.Empty
				}, 0) { Tag = task };
			return lvi;
		}

		/// <summary>
		/// Event args for when a task is selected.
		/// </summary>
		public class TaskSelectedEventArgs : EventArgs
		{
			/// <summary>
			/// Empty <see cref="TaskSelectedEventArgs"/> class.
			/// </summary>
			public static new readonly TaskSelectedEventArgs Empty = new TaskSelectedEventArgs();

			private TaskSelectedEventArgs() : base() { }

			/// <summary>
			/// Initializes a new instance of the <see cref="TaskSelectedEventArgs"/> class.
			/// </summary>
			/// <param name="task">The task.</param>
			internal TaskSelectedEventArgs(Task task = null)
			{
				this.Task = task;
			}

			/// <summary>
			/// Gets the task.
			/// </summary>
			/// <value>The task.</value>
			public Task Task { get; private set; }
		}
	}
}