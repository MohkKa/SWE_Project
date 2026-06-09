// ============================================================
//  E-Wallet Financial System
//  File: Program.cs
//  Purpose: Application entry point
// ============================================================
using System;
using System.Windows.Forms;

namespace E_Wallet
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
