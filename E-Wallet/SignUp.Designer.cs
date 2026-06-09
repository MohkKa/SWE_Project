// ============================================================
//  E-Wallet Financial System
//  File: SignUp.Designer.cs
//  Purpose: Designer layout for registration screen
// ============================================================
namespace E_Wallet
{
    partial class SignUp
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.username = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.password = new System.Windows.Forms.MaskedTextBox();
            this.confirmpassword = new System.Windows.Forms.MaskedTextBox();
            this.pin = new System.Windows.Forms.MaskedTextBox();
            this.phoneno = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cardno = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.bankname = new System.Windows.Forms.ComboBox();
            this.btnRegister = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(364, 66);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(180, 22);
            this.username.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(220, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 17);
            this.label1.TabIndex = 12;
            this.label1.Text = "Username";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(220, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 17);
            this.label2.TabIndex = 11;
            this.label2.Text = "Password";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(220, 155);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(121, 17);
            this.label3.TabIndex = 10;
            this.label3.Text = "Confirm Password";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(220, 197);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 17);
            this.label4.TabIndex = 9;
            this.label4.Text = "PIN (4 digits)";
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(364, 107);
            this.password.Name = "password";
            this.password.PasswordChar = '●';
            this.password.Size = new System.Drawing.Size(180, 22);
            this.password.TabIndex = 1;
            // 
            // confirmpassword
            // 
            this.confirmpassword.Location = new System.Drawing.Point(364, 150);
            this.confirmpassword.Name = "confirmpassword";
            this.confirmpassword.PasswordChar = '●';
            this.confirmpassword.Size = new System.Drawing.Size(180, 22);
            this.confirmpassword.TabIndex = 2;
            // 
            // pin
            // 
            this.pin.Location = new System.Drawing.Point(364, 192);
            this.pin.Name = "pin";
            this.pin.PasswordChar = '●';
            this.pin.Size = new System.Drawing.Size(180, 22);
            this.pin.TabIndex = 3;
            // 
            // phoneno
            // 
            this.phoneno.Location = new System.Drawing.Point(364, 235);
            this.phoneno.Name = "phoneno";
            this.phoneno.Size = new System.Drawing.Size(180, 22);
            this.phoneno.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(220, 240);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 17);
            this.label5.TabIndex = 8;
            this.label5.Text = "Phone Number";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(220, 322);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(92, 17);
            this.label6.TabIndex = 7;
            this.label6.Text = "Card Number";
            // 
            // cardno
            // 
            this.cardno.Location = new System.Drawing.Point(364, 317);
            this.cardno.Name = "cardno";
            this.cardno.Size = new System.Drawing.Size(180, 22);
            this.cardno.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(220, 280);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(81, 17);
            this.label7.TabIndex = 6;
            this.label7.Text = "Bank Name";
            // 
            // bankname
            // 
            this.bankname.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bankname.FormattingEnabled = true;
            this.bankname.Location = new System.Drawing.Point(364, 275);
            this.bankname.Name = "bankname";
            this.bankname.Size = new System.Drawing.Size(180, 24);
            this.bankname.TabIndex = 5;
            // 
            // btnRegister
            // 
            this.btnRegister.Location = new System.Drawing.Point(223, 375);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(120, 32);
            this.btnRegister.TabIndex = 7;
            this.btnRegister.Text = "Register";
            this.btnRegister.UseVisualStyleBackColor = true;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(290, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(190, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Create Account";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(424, 375);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 32);
            this.button1.TabIndex = 13;
            this.button1.Text = "Back";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::E_Wallet.Properties.Resources.user1;
            this.pictureBox1.Location = new System.Drawing.Point(31, 79);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(115, 112);
            this.pictureBox1.TabIndex = 14;
            this.pictureBox1.TabStop = false;
            // 
            // SignUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.bankname);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cardno);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.phoneno);
            this.Controls.Add(this.pin);
            this.Controls.Add(this.confirmpassword);
            this.Controls.Add(this.password);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.username);
            this.Controls.Add(this.btnRegister);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SignUp";
            this.Text = "E-Wallet — Create Account";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox      username;
        private System.Windows.Forms.Label        label1;
        private System.Windows.Forms.Label        label2;
        private System.Windows.Forms.Label        label3;
        private System.Windows.Forms.Label        label4;
        private System.Windows.Forms.MaskedTextBox password;
        private System.Windows.Forms.MaskedTextBox confirmpassword;
        private System.Windows.Forms.MaskedTextBox pin;
        private System.Windows.Forms.TextBox      phoneno;
        private System.Windows.Forms.Label        label5;
        private System.Windows.Forms.Label        label6;
        private System.Windows.Forms.TextBox      cardno;
        private System.Windows.Forms.Label        label7;
        private System.Windows.Forms.ComboBox     bankname;
        // BUG FIX: added missing Register button declaration
        private System.Windows.Forms.Button       btnRegister;
        private System.Windows.Forms.Label        lblTitle;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
