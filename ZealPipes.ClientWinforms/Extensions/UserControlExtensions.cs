using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealPipes.ClientWinforms.Extensions;

using System;
using System.Drawing;
using System.Windows.Forms;

public static class ControlExtensions
{
    public static void EnableDragging(this Control control)
    {
        bool isDragging = false;
        Point dragStartPoint = Point.Empty;

        control.MouseDown += (sender, e) =>
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                dragStartPoint = e.Location;
                control.Cursor = Cursors.SizeAll; // Change cursor to drag cursor
            }
        };

        control.MouseMove += (sender, e) =>
        {
            if (isDragging)
            {
                var currentScreenPos = control.PointToScreen(e.Location);
                var parentScreenPos = control.Parent.PointToScreen(control.Location);
                var offset = new Size(currentScreenPos.X - parentScreenPos.X - dragStartPoint.X, currentScreenPos.Y - parentScreenPos.Y - dragStartPoint.Y);
                control.Location = control.Location + offset;
            }
        };

        control.MouseUp += (sender, e) =>
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = false;
                control.Cursor = Cursors.Default; // Reset cursor
            }
        };

        control.MouseEnter += (sender, e) =>
        {
            control.Cursor = Cursors.SizeAll; // Change cursor to drag cursor
        };

        control.MouseLeave += (sender, e) =>
        {
            if (!isDragging)
            {
                control.Cursor = Cursors.Default; // Reset cursor
            }
        };
    }
}
