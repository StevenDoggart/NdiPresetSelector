using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace NdiPresetSelector;


public partial class AboutForm : Form
{
    public AboutForm()
    {
        InitializeComponent();
    }


    private void _helpTextBox_Enter(object sender, System.EventArgs e)
    {
        _helpTextBox.Select(0, 0);
    }


    private void _linkLabel_MouseHover(object sender, System.EventArgs e)
    {

        _linkLabel.Font = new Font(_linkLabel.Font, FontStyle.Underline);
    }


    private void _linkLabel_MouseLeave(object sender, System.EventArgs e)
    {
        _linkLabel.Font = new Font(_linkLabel.Font, FontStyle.Regular);
    }


    private void _linkLabel_Click(object sender, System.EventArgs e)
    {
        Process.Start("explorer", _linkLabel.Text);
    }
}