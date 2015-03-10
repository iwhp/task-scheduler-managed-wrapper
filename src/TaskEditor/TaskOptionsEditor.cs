﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Microsoft.Win32.TaskScheduler
{
	/// <summary>
	/// Dialog that allows tasks to be edited
	/// </summary>
	[ToolboxItem(true), ToolboxItemFilter("System.Windows.Forms"), Description("Dialog allowing the editing of a task.")]
	[Designer("System.ComponentModel.Design.ComponentDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DefaultProperty("Editable"), DesignTimeVisible(true)]
	[System.Drawing.ToolboxBitmap(typeof(TaskEditDialog), "TaskDialog")]
	public partial class TaskOptionsEditor :
#if DEBUG
		Form
#else
		DialogBase
#endif
		, ITaskEditor
	{
		private OptionPanels.OptionPanel curPanel;
		private bool editable, onAssignment;
		private int hoverIndex = -1;
		private System.Collections.Generic.Dictionary<ToolStripMenuItem, OptionPanels.OptionPanel> panels = new System.Collections.Generic.Dictionary<ToolStripMenuItem, OptionPanels.OptionPanel>(10);
		private Task task;
		private TaskScheduler.TaskDefinition td;
		private bool titleSet;

		/// <summary>
		/// Initializes a new instance of the <see cref="TaskOptionsEditor"/> class.
		/// </summary>
		public TaskOptionsEditor()
		{
			InitializeComponent();
			panels.Add(generalItem, new OptionPanels.GeneralOptionPanel());
			panels.Add(triggersItem, new OptionPanels.TriggersOptionPanel());
			panels.Add(actionsItem, new OptionPanels.ActionsOptionPanel());
			panels.Add(securityItem, new OptionPanels.SecurityOptionPanel());
			panels.Add(startupItem, new OptionPanels.StartupOptionPanel());
			panels.Add(runItem, new OptionPanels.RuntimeOptionPanel());
			foreach (var p in menuItemsContainer.Items)
				menuList.Items.Add(p);
			UpdateTitleFont();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TaskOptionsEditor"/> class.
		/// </summary>
		/// <param name="task">The task.</param>
		/// <param name="editable">If set to <c>true</c> the task will be editable in the dialog.</param>
		/// <param name="registerOnAccept">If set to <c>true</c> the task will be registered when Ok is pressed.</param>
		public TaskOptionsEditor(Task task, bool editable = true, bool registerOnAccept = true) : this()
		{
			this.Editable = editable;
			this.Initialize(task);
			this.RegisterTaskOnAccept = registerOnAccept;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TaskOptionsEditor"/> class.
		/// </summary>
		/// <param name="service">A <see cref="TaskService"/> instance.</param>
		/// <param name="td">An optional <see cref="TaskDefinition"/>. Leaving null creates a new task.</param>
		/// <param name="editable">If set to <c>true</c> the task will be editable in the dialog.</param>
		/// <param name="registerOnAccept">If set to <c>true</c> the task will be registered when Ok is pressed.</param>
		public TaskOptionsEditor(TaskService service, TaskDefinition td = null, bool editable = true, bool registerOnAccept = true) : this()
		{
			this.Editable = editable;
			this.Initialize(service, td);
			this.RegisterTaskOnAccept = registerOnAccept;
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="TaskEditDialog"/> is editable.
		/// </summary>
		/// <value><c>true</c> if editable; otherwise, <c>false</c>.</value>
		[DefaultValue(false), Category("Behavior"), Description("Determines whether the task can be edited.")]
		public bool Editable
		{
			get { return editable; }
			set
			{
				if (editable != value)
				{
					editable = value;
					ReinitializeControls();
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this task definition is v2.
		/// </summary>
		/// <value>
		///   <c>true</c> if this task definition is v2; otherwise, <c>false</c>.
		/// </value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsV2 { get; private set; }

		/// <summary>
		/// Gets or sets a value indicating whether to register task when Accept (Ok) button pressed.
		/// </summary>
		/// <value><c>true</c> if updated task is to be registered; otherwise, <c>false</c>.</value>
		[Category("Behavior"), DefaultValue(false)]
		public bool RegisterTaskOnAccept { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether errors are shown in the UI.
		/// </summary>
		/// <value>
		///   <c>true</c> if errors are shown; otherwise, <c>false</c>.
		/// </value>
		[DefaultValue(true), Category("Behavior"), Description("Determines whether errors are shown in the UI.")]
		public bool ShowErrors { get; set; }

		/// <summary>
		/// Gets the current <see cref="Task"/>. This is only the task used to initialize this control. The updates made to the control are not registered.
		/// </summary>
		/// <value>The task.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Task Task
		{
			get
			{
				return task;
			}
			private set
			{
				task = value;
				if (task != null)
				{
					TaskService = task.TaskService;
					if (task.ReadOnly)
						this.Editable = false;
					TaskDefinition = task.Definition;
				}
			}
		}

		/// <summary>
		/// Gets the <see cref="TaskDefinition"/> in its edited state.
		/// </summary>
		/// <value>The task definition.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TaskDefinition TaskDefinition
		{
			get { return td; }
			private set
			{
				if (TaskService == null)
					throw new ArgumentNullException("TaskDefinition cannot be set until TaskService has been set with a valid object.");

				if (value == null)
					throw new ArgumentNullException("TaskDefinition cannot be set to null.");

				onAssignment = true;
				td = value;
				IsV2 = td.Settings.Compatibility >= TaskCompatibility.V2;
				taskNameText.Text = this.Task != null ? this.Task.Name : string.Empty;
				SetVersionComboItems();
				ReinitializeControls();
				onAssignment = false;
			}
		}

		/// <summary>
		/// Gets or sets the folder for the task. If control is initialized with a <see cref="Task"/>, this value will be set to the folder of the registered task.
		/// </summary>
		/// <value>The task folder name.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string TaskFolder { get; set; }

		/// <summary>
		/// Gets or sets the name of the task. If control is initialized with a <see cref="Task"/>, this value will be set to the name of the registered task.
		/// </summary>
		/// <value>The task name.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string TaskName { get; set; }

		/// <summary>
		/// Gets the <see cref="TaskService"/> assigned at initialization.
		/// </summary>
		/// <value>The task service.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TaskService TaskService { get; set; }

		/// <summary>
		/// Gets or sets the title.
		/// </summary>
		/// <value>The title.</value>
		[Category("Appearance"), Description("A string to display in the title bar of the dialog box."), Localizable(true)]
		public string Title
		{
			get { return base.Text; }
			set { base.Text = value; titleSet = true; }
		}

		/// <summary>
		/// Initializes the control for the editing of a new <see cref="TaskDefinition"/>.
		/// </summary>
		/// <param name="service">A <see cref="TaskService"/> instance.</param>
		/// <param name="td">An optional <see cref="TaskDefinition"/>. Leaving null creates a new task.</param>
		public void Initialize(TaskService service, TaskDefinition td = null)
		{
			if (service == null)
				throw new ArgumentNullException("service");
			if (!titleSet)
				this.Text = string.Format(EditorProperties.Resources.TaskEditDlgTitle, "New Task", TaskEditDialog.GetServerString(service));
			this.TaskService = service;
			this.task = null;
			if (!this.IsDesignMode())
			{
				if (td == null)
					this.TaskDefinition = service.NewTask();
				else
					this.TaskDefinition = td;
			}
		}

		/// <summary>
		/// Initializes the control for the editing of an existing <see cref="Task"/>.
		/// </summary>
		/// <param name="task">A <see cref="Task"/> instance.</param>
		public void Initialize(Task task)
		{
			if (task == null)
				throw new ArgumentNullException("task");
			if (!titleSet)
				this.Text = string.Format(EditorProperties.Resources.TaskEditDlgTitle, task.Name, TaskEditDialog.GetServerString(task.TaskService));
			this.Task = task;
		}

		/// <summary>
		/// Reinitializes all the controls based on current <see cref="TaskDefinition" /> values.
		/// </summary>
		public void ReinitializeControls()
		{
			taskNameText.ReadOnly = !(this.Task == null && this.Editable);
			taskVersionCombo.Enabled = this.Editable;
			if (curPanel == null && td != null)
				//this.menuList.Items[0].PerformClick();
				this.menuList.SelectedIndex = 0;
			if (curPanel != null)
				curPanel.Initialize(this);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.FontChanged" /> event when the <see cref="P:System.Windows.Forms.Control.Font" /> property value of the control's container changes.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		protected override void OnParentFontChanged(EventArgs e)
		{
			base.OnParentFontChanged(e);
			UpdateTitleFont();
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void menuItem_Click(object sender, EventArgs e)
		{
			if (sender is ToolStripMenuItem)
			{
				OptionPanels.OptionPanel panel = null;
				if (panels.TryGetValue((ToolStripMenuItem)sender, out panel))
				{
					this.bodyPanel.SuspendLayout();
					panel.Dock = DockStyle.Fill;
					this.panelTitleLabel.Text = panel.Title;
					this.panelImage.Image = panel.Image;
					this.panelImage.Visible = panel.Image != null;
					if (curPanel != null)
						this.bodyPanel.Controls.Remove(curPanel);
					this.bodyPanel.Controls.Add(panel);
					this.bodyPanel.Controls.SetChildIndex(panel, 0);
					curPanel = panel;
					if (td != null)
						panel.Initialize(this);
					this.bodyPanel.ResumeLayout();
				}
			}
		}

		private void menuList_DrawItem(object sender, DrawItemEventArgs e)
		{
			Color sel = SystemColors.ControlLight;
			Color hot = LightenColor(SystemColors.ControlLight, 50);

			Color fc = this.ForeColor;
			Color bc = SystemColors.Window;
			if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
				bc = sel;
			if (e.Index == hoverIndex)
				bc = hot;
			if ((e.State & DrawItemState.Grayed) == DrawItemState.Grayed)
				fc = SystemColors.GrayText;
			using (Brush bgb = new SolidBrush(bc))
				e.Graphics.FillRectangle(bgb, e.Bounds);
			TextRenderer.DrawText(e.Graphics, menuList.Items[e.Index].ToString(), this.Font, AdjustRect(e.Bounds, 4, 0, -4, 0), fc, bc, TextFormatFlags.EndEllipsis | TextFormatFlags.VerticalCenter);
			if ((e.State & DrawItemState.Focus) == DrawItemState.Focus)
				using (Pen bgb = new Pen(sel))
					e.Graphics.DrawRectangle(bgb, AdjustRect(e.Bounds, 0, 0, -1, -1));
		}

		private static Color LightenColor(Color colorIn, int percent)
		{
			if (percent < 0 || percent > 100)
				throw new ArgumentOutOfRangeException("percent");

			return Color.FromArgb(colorIn.A, colorIn.R + (int)(((255f - colorIn.R) / 100f) * percent), 
				colorIn.G + (int)(((255f - colorIn.G) / 100f) * percent), colorIn.B + (int)(((255f - colorIn.B) / 100f) * percent));
		}

		private static Rectangle AdjustRect(Rectangle rect, int x, int y = 0, int w = 0, int h = 0)
		{
			return new Rectangle(rect.X + x, rect.Y + y, rect.Width + w, rect.Height + h);
		}

		private void menuList_MeasureItem(object sender, MeasureItemEventArgs e)
		{
			e.ItemHeight *= 2;
		}

		private void menuList_MouseMove(object sender, MouseEventArgs e)
		{
			int index = menuList.IndexFromPoint(e.Location);
			if (index != hoverIndex)
			{
				hoverIndex = index;
				menuList.Invalidate();
			}
		}

		private void menuList_MouseLeave(object sender, EventArgs e)
		{
			if (hoverIndex > -1)
			{
				hoverIndex = -1;
				menuList.Invalidate();
			}
		}

		private void menuList_SelectedIndexChanged(object sender, EventArgs e)
		{
			menuItem_Click(menuList.SelectedItem as ToolStripMenuItem, EventArgs.Empty);
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			if (this.TaskDefinition.Actions.Count == 0)
			{
				MessageBox.Show(EditorProperties.Resources.TaskMustHaveActionsError, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (this.TaskDefinition.Settings.DeleteExpiredTaskAfter != TimeSpan.Zero && !TaskEditDialog.ValidateOneTriggerExpires(this.TaskDefinition.Triggers))
			{
				MessageBox.Show(EditorProperties.Resources.Error_TaskDeleteMustHaveExpiringTrigger, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (this.TaskDefinition.LowestSupportedVersion > this.TaskDefinition.Settings.Compatibility)
			{
				MessageBox.Show(EditorProperties.Resources.Error_TaskPropertiesIncompatibleSimple, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (RegisterTaskOnAccept)
			{
				if (this.Task != null && this.Task.Definition.Principal.LogonType != TaskLogonType.InteractiveTokenOrPassword && this.Task.Definition.Principal.LogonType != TaskLogonType.Password)
					this.Task.RegisterChanges();
				else
				{
					string user = this.TaskDefinition.Principal.ToString();
					string pwd = null;
					TaskFolder fld = this.TaskService.GetFolder(this.TaskFolder);
					if (this.TaskDefinition.Principal.LogonType == TaskLogonType.InteractiveTokenOrPassword || this.TaskDefinition.Principal.LogonType == TaskLogonType.Password)
					{
						pwd = TaskEditDialog.InvokeCredentialDialog(user, this);
						if (pwd == null)
						{
							//throw new System.Security.Authentication.AuthenticationException(EditorProperties.Resources.UserAuthenticationError);
							MessageBox.Show(EditorProperties.Resources.Error_PasswordMustBeProvided, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
							return;
						}
					}
					this.Task = fld.RegisterTaskDefinition(this.TaskName, this.TaskDefinition, TaskCreation.CreateOrUpdate,
						user, pwd, this.TaskDefinition.Principal.LogonType);
				}
			}
			this.DialogResult = DialogResult.OK;
			Close();
		}

		private void ResetTitle()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TaskOptionsEditor));
			base.Text = resources.GetString("$this.Text");
		}

		private bool ShouldSerializeTitle()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TaskOptionsEditor));
			return base.Text != resources.GetString("$this.Text");
		}

		private void SetVersionComboItems()
		{
			const int expectedVersions = 5;

			this.taskVersionCombo.BeginUpdate();
			this.taskVersionCombo.Items.Clear();
			string[] versions = EditorProperties.Resources.TaskCompatibility.Split('|');
			if (versions.Length != expectedVersions)
				throw new ArgumentOutOfRangeException("Locale specific information about supported Operating Systems is insufficient.");
			int max = (this.TaskService == null) ? expectedVersions - 1 : TaskService.LibraryVersion.Minor;
			TaskCompatibility comp = (td != null) ? td.Settings.Compatibility : TaskCompatibility.V1;
			TaskCompatibility lowestComp = (td != null) ? td.LowestSupportedVersion : TaskCompatibility.V1;
			switch (comp)
			{
				case TaskCompatibility.AT:
					for (int i = max; i > 1; i--)
						this.taskVersionCombo.Items.Add(new ComboItem(versions[i], i, comp >= lowestComp));
					this.taskVersionCombo.SelectedIndex = this.taskVersionCombo.Items.Add(new ComboItem(versions[0], 0));
					break;
				default:
					for (int i = max; i > 0; i--)
						this.taskVersionCombo.Items.Add(new ComboItem(versions[i], i, comp >= lowestComp));
					this.taskVersionCombo.SelectedIndex = this.taskVersionCombo.Items.IndexOf((int)comp);
					break;
			}
			this.taskVersionCombo.EndUpdate();
		}

		private void taskNameText_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			char[] inv = System.IO.Path.GetInvalidFileNameChars();
			e.Cancel = !ValidateText(taskNameText,
				delegate(string s) { return s.Length > 0 && s.IndexOfAny(inv) == -1; },
				EditorProperties.Resources.Error_InvalidNameFormat);
		}

		private void TaskOptionsEditor_HelpButtonClicked(object sender, CancelEventArgs e)
		{

		}

		private void taskVersionCombo_SelectedIndexChanged(object sender, EventArgs e)
		{
			IsV2 = taskVersionCombo.SelectedIndex == -1 ? true : ((ComboItem)taskVersionCombo.SelectedItem).Version > 1;
			TaskCompatibility priorSetting = (td != null) ? td.Settings.Compatibility : TaskCompatibility.V1;
			if (!onAssignment && td != null && taskVersionCombo.SelectedIndex != -1)
				td.Settings.Compatibility = (TaskCompatibility)((ComboItem)taskVersionCombo.SelectedItem).Version;
			try
			{
				if (!onAssignment && td != null)
				{
					td.Validate(true);
					ReinitializeControls();
				}
			}
			catch (InvalidOperationException ex)
			{
				var msg = new System.Text.StringBuilder();
				if (this.ShowErrors)
				{
					msg.AppendLine(EditorProperties.Resources.Error_TaskPropertiesIncompatible);
					foreach (var item in ex.Data.Keys)
						msg.AppendLine(string.Format("- {0} {1}", item, ex.Data[item]));
				}
				else
					msg.Append(EditorProperties.Resources.Error_TaskPropertiesIncompatibleSimple);
				MessageBox.Show(this, msg.ToString(), EditorProperties.Resources.TaskSchedulerName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				this.taskVersionCombo.SelectedIndex = this.taskVersionCombo.Items.IndexOf((int)priorSetting);
				return;
			}
		}

		private void UpdateTitleFont()
		{
			this.panelTitleLabel.Font = new System.Drawing.Font(this.Font.FontFamily, this.Font.Size + 1, System.Drawing.FontStyle.Bold, this.Font.Unit);
		}

		private bool ValidateText(Control ctrl, Predicate<string> pred, string error)
		{
			bool valid = pred(ctrl.Text);
			//errorProvider.SetError(ctrl, valid ? string.Empty : error);
			//OnComponentError(valid ? ComponentErrorEventArgs.Empty : new ComponentErrorEventArgs(null, error));
			//hasError = valid;
			return valid;
		}

		private class ComboItem : IEnableable
		{
			public string Text;
			public int Version;
			private bool enabled;
			public ComboItem(string text, int ver, bool enabled = true) { Text = text; Version = ver; this.enabled = enabled; }

			public bool Enabled
			{
				get { return enabled; }
				set { enabled = value; }
			}

			public override bool Equals(object obj)
			{
				if (obj is ComboItem)
					return Version == ((ComboItem)obj).Version;
				if (obj is int)
					return Version == (int)obj;
				return Text.CompareTo(obj.ToString()) == 0;
			}

			public override int GetHashCode()
			{
				return Version.GetHashCode();
			}

			public override string ToString() { return this.Text; }
		}
	}
}