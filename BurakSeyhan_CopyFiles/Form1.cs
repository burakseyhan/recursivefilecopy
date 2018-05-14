using BurakSeyhan_CopyFiles.BL.Entity;
using BurakSeyhan_CopyFiles.BL.Manager;

using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BurakSeyhan_CopyFiles
{
    public partial class Form1 : Form
    {
        private FileManager fileManager;

        public Form1()
        {
            InitializeComponent();
            fileManager = new FileManager();
            fileManager.NofityMe += FileManager_NofityMe;
        }

        private void FileManager_NofityMe(int bytes, bool specifyMax, bool withStep, string message)
        {
            if (specifyMax)
            {
                pbState.Invoke(new Action(() => pbState.Maximum = bytes));
                lblInfo.Invoke(new Action(() => lblInfo.Text = message));
            }

            if (withStep)
            {
                pbState.Invoke(new Action(() => pbState.Update()));
                pbState.Invoke(new Action(() => pbState.Step = bytes));
                pbState.Invoke(new Action(() => pbState.PerformStep()));
                lblInfo.Invoke(new Action(() => lblInfo.Text = message));
                lblInfo.Invoke(new Action(() => lblInfo.Refresh()));
            }

            if (bytes == 0 && !specifyMax && !withStep && !string.IsNullOrEmpty(message))
            {
                MessageBox.Show(message);
            }
        }

        private async void btnCopyFiles_Click(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                StartCopy();
            });
        }

        private void StartCopy()
        {
            FileEntity entity = new FileEntity();

            if (entity != null)
            {
                cbFrom.Invoke(new Action(() => entity.From = cbFrom.Text));
                cbTo.Invoke(new Action(() => entity.To = cbTo.Text));

                Console.WriteLine(entity.From);
                fileManager.Initialize(entity.From, entity.To);
            }
            else
            {
                MessageBox.Show("Do not blank");
            }

        }
    }
}
