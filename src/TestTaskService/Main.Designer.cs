﻿extern alias GrpCtrl;

namespace TestTaskService
{
	partial class Main
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			GrpCtrl::GroupControls.RadioButtonListItem radioButtonListItem1 = new GrpCtrl::GroupControls.RadioButtonListItem();
			GrpCtrl::GroupControls.RadioButtonListItem radioButtonListItem2 = new GrpCtrl::GroupControls.RadioButtonListItem();
			GrpCtrl::GroupControls.RadioButtonListItem radioButtonListItem3 = new GrpCtrl::GroupControls.RadioButtonListItem();
			GrpCtrl::GroupControls.RadioButtonListItem radioButtonListItem4 = new GrpCtrl::GroupControls.RadioButtonListItem();
			GrpCtrl::GroupControls.RadioButtonListItem radioButtonListItem5 = new GrpCtrl::GroupControls.RadioButtonListItem();
			GrpCtrl::GroupControls.RadioButtonListItem radioButtonListItem6 = new GrpCtrl::GroupControls.RadioButtonListItem();
			GrpCtrl::GroupControls.RadioButtonListItem radioButtonListItem7 = new GrpCtrl::GroupControls.RadioButtonListItem();
			GrpCtrl::GroupControls.RadioButtonListItem radioButtonListItem8 = new GrpCtrl::GroupControls.RadioButtonListItem();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.radioButtonList1 = new GrpCtrl::GroupControls.RadioButtonList();
			this.closeButton = new System.Windows.Forms.Button();
			this.runButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.reconnectLink = new System.Windows.Forms.LinkLabel();
			this.ts = new Microsoft.Win32.TaskScheduler.TaskService();
			this.taskServiceConnectDialog1 = new Microsoft.Win32.TaskScheduler.TaskServiceConnectDialog();
			((System.ComponentModel.ISupportInitialize)(this.ts)).BeginInit();
			this.SuspendLayout();
			// 
			// textBox1
			// 
			this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBox1.Location = new System.Drawing.Point(13, 88);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBox1.Size = new System.Drawing.Size(393, 128);
			this.textBox1.TabIndex = 3;
			// 
			// radioButtonList1
			// 
			this.radioButtonList1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.radioButtonList1.AutoScroll = true;
			this.radioButtonList1.AutoScrollMinSize = new System.Drawing.Size(393, 35);
			radioButtonListItem1.Text = "Short test";
			radioButtonListItem2.Text = "Long test";
			radioButtonListItem3.Text = "Editor test";
			radioButtonListItem4.Text = "Find Task Prop";
			radioButtonListItem5.Text = "Wizard test";
			radioButtonListItem6.Text = "MMC test";
			radioButtonListItem7.Text = "Find Task";
			radioButtonListItem8.Text = "Output XML";
			this.radioButtonList1.Items.Add(radioButtonListItem1);
			this.radioButtonList1.Items.Add(radioButtonListItem2);
			this.radioButtonList1.Items.Add(radioButtonListItem3);
			this.radioButtonList1.Items.Add(radioButtonListItem4);
			this.radioButtonList1.Items.Add(radioButtonListItem5);
			this.radioButtonList1.Items.Add(radioButtonListItem6);
			this.radioButtonList1.Items.Add(radioButtonListItem7);
			this.radioButtonList1.Items.Add(radioButtonListItem8);
			this.radioButtonList1.Location = new System.Drawing.Point(13, 13);
			this.radioButtonList1.Name = "radioButtonList1";
			this.radioButtonList1.RepeatColumns = 4;
			this.radioButtonList1.RepeatDirection = GrpCtrl::GroupControls.RepeatDirection.Horizontal;
			this.radioButtonList1.Size = new System.Drawing.Size(393, 35);
			this.radioButtonList1.TabIndex = 0;
			// 
			// closeButton
			// 
			this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.closeButton.Location = new System.Drawing.Point(331, 222);
			this.closeButton.Name = "closeButton";
			this.closeButton.Size = new System.Drawing.Size(75, 23);
			this.closeButton.TabIndex = 6;
			this.closeButton.Text = "Close";
			this.closeButton.UseVisualStyleBackColor = true;
			this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
			// 
			// runButton
			// 
			this.runButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.runButton.Location = new System.Drawing.Point(250, 222);
			this.runButton.Name = "runButton";
			this.runButton.Size = new System.Drawing.Size(75, 23);
			this.runButton.TabIndex = 5;
			this.runButton.Text = "Run";
			this.runButton.UseVisualStyleBackColor = true;
			this.runButton.Click += new System.EventHandler(this.runButton_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 65);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(34, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Input:";
			// 
			// textBox2
			// 
			this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBox2.Location = new System.Drawing.Point(53, 62);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(353, 20);
			this.textBox2.TabIndex = 2;
			// 
			// reconnectLink
			// 
			this.reconnectLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.reconnectLink.AutoSize = true;
			this.reconnectLink.Location = new System.Drawing.Point(13, 227);
			this.reconnectLink.Name = "reconnectLink";
			this.reconnectLink.Size = new System.Drawing.Size(148, 13);
			this.reconnectLink.TabIndex = 4;
			this.reconnectLink.TabStop = true;
			this.reconnectLink.Text = "Change connection settings...";
			this.reconnectLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.reconnectLink_LinkClicked);
			// 
			// taskServiceConnectDialog1
			// 
			this.taskServiceConnectDialog1.AutoSize = true;
			this.taskServiceConnectDialog1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.taskServiceConnectDialog1.ClientSize = new System.Drawing.Size(444, 181);
			this.taskServiceConnectDialog1.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.taskServiceConnectDialog1.Name = "TSConnectDlg";
			this.taskServiceConnectDialog1.TaskService = this.ts;
			this.taskServiceConnectDialog1.Text = "Select Computer";
			this.taskServiceConnectDialog1.Visible = false;
			// 
			// Main
			// 
			this.AcceptButton = this.runButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.closeButton;
			this.ClientSize = new System.Drawing.Size(418, 257);
			this.Controls.Add(this.reconnectLink);
			this.Controls.Add(this.textBox2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.runButton);
			this.Controls.Add(this.closeButton);
			this.Controls.Add(this.radioButtonList1);
			this.Controls.Add(this.textBox1);
			this.Name = "Main";
			this.Text = "Test Task Service";
			((System.ComponentModel.ISupportInitialize)(this.ts)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox textBox1;
		private GrpCtrl::GroupControls.RadioButtonList radioButtonList1;
		private System.Windows.Forms.Button closeButton;
		private System.Windows.Forms.Button runButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBox2;
		private Microsoft.Win32.TaskScheduler.TaskService ts;
		private System.Windows.Forms.LinkLabel reconnectLink;
		private Microsoft.Win32.TaskScheduler.TaskServiceConnectDialog taskServiceConnectDialog1;
	}
}