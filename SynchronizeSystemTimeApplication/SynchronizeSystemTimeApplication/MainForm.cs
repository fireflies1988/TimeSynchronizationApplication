using System;
using System.Windows.Forms;
using Time;

namespace SynchronizeSystemTimeApplication
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            comboBoxTimeServer.SelectedItem = "time.windows.com";
            comboBoxTimeZone.SelectedIndexChanged -= new EventHandler(comboBoxTimeZone_SelectedIndexChanged);
            comboBoxTimeZone.DataSource = TimeZoneInfo.GetSystemTimeZones();
            comboBoxTimeZone.DisplayMember = "DisplayName";
            comboBoxTimeZone.ValueMember = "Id";
            comboBoxTimeZone.SelectedValue = TimeZone.CurrentTimeZone.StandardName;
            comboBoxTimeZone.SelectedIndexChanged += new EventHandler(comboBoxTimeZone_SelectedIndexChanged);
        }

        private void buttonSaveChanges_Click(object sender, EventArgs e)
        {
            try
            {
                SystemTime.SetSystemDateTime(dateTimePicker.Value.ToString("MM/dd/yyyy"), dateTimePicker.Value.ToString("hh:mm:ss tt"));
            }
            catch
            {
                MessageBox.Show(this, "Error to change date and time of your system!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSync_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime dt = SystemTime.GetNetworkTime(comboBoxTimeServer.SelectedItem.ToString());
                dateTimePicker.Value = dt;
                SystemTime.SetSystemDateTime(dateTimePicker.Value.ToString("MM/dd/yyyy"), dateTimePicker.Value.ToString("hh:mm:ss tt"));
            }
            catch
            {
                MessageBox.Show(this, "Error to change date and time of your system!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            dateTimePicker.Value = dateTimePicker.Value.AddSeconds(1);
        }

        private void comboBoxTimeZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SystemTime.SetSystemTimeZone(comboBoxTimeZone.SelectedValue.ToString());
                dateTimePicker.Value = DateTime.Now;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
