using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ConstraintLayout.Constraints;

namespace ConstraintLayoutSample
{
    /// <summary>
    /// Interaction logic for Sample2.xaml
    /// </summary>
    public partial class Sample2 : UserControl
    {
        private int _addCounter;

        public Sample2()
        {
            InitializeComponent();
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            var button = new Button
            {
                Content = (char)('A' + _addCounter++)
            };
            button.Click += Button_Click;

            ConstraintCanvas.Children.Add(button);
            StackConstraint.Items.Add(new StackConstraintItem
            {
                Element = button
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var foundItem = StackConstraint.Items.First(item =>  item.Element == sender);
            ConstraintCanvas.Children.Remove(foundItem.Element);
            StackConstraint.Items.Remove(foundItem);
        }
    }
}
