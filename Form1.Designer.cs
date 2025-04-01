namespace FirstProducer
{
	partial class Form1
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			btnPublish = new Button();
			txtTopic = new TextBox();
			chkUnique = new CheckBox();
			SuspendLayout();
			// 
			// btnPublish
			// 
			btnPublish.Location = new Point(255, 203);
			btnPublish.Name = "btnPublish";
			btnPublish.Size = new Size(165, 60);
			btnPublish.TabIndex = 0;
			btnPublish.Text = "Publish";
			btnPublish.UseVisualStyleBackColor = true;
			btnPublish.Click += btnPublish_Click;
			// 
			// txtTopic
			// 
			txtTopic.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
			txtTopic.Location = new Point(173, 114);
			txtTopic.Multiline = true;
			txtTopic.Name = "txtTopic";
			txtTopic.Size = new Size(344, 43);
			txtTopic.TabIndex = 1;
			// 
			// chkUnique
			// 
			chkUnique.AutoSize = true;
			chkUnique.Location = new Point(523, 182);
			chkUnique.Name = "chkUnique";
			chkUnique.Size = new Size(111, 24);
			chkUnique.TabIndex = 2;
			chkUnique.Text = "RandomKey";
			chkUnique.UseVisualStyleBackColor = true;
			// 
			// Form1
			// 
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(800, 450);
			Controls.Add(chkUnique);
			Controls.Add(txtTopic);
			Controls.Add(btnPublish);
			Name = "Form1";
			Text = "Form1";
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Button btnPublish;
		private TextBox txtTopic;
		private CheckBox chkUnique;
	}
}
